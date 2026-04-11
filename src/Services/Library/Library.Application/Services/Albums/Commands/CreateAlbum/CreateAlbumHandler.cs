namespace Library.Application.Services.Albums.Commands.CreateAlbum;

public class CreateAlbumHandler(ILibraryDbContext context, IStorageService storage, IHttpContextAccessor httpContextAccessor) : BuildingBlocks.CQRS.ICommandHandler<CreateAlbumCommand, CreateAlbumResult>
{
    public async ValueTask<CreateAlbumResult> Handle(CreateAlbumCommand command, CancellationToken cancellationToken)
    {
        await ValidateReferencedEntitiesAsync(command.Album, cancellationToken);

        string? coverKey = null;
        if (command.CoverImage is not null && command.CoverImageContentType is not null && command.CoverImageFileName is not null)
        {
            string folder = $"albums/{SlugHelper.Generate(command.Album.Title)}";
            string extension = Path.GetExtension(command.CoverImageFileName);
            coverKey = await storage.UploadFileAsync(folder, $"{Guid.NewGuid()}{extension}", command.CoverImage, command.CoverImageContentType, cancellationToken);
        }

        string slug = await GenerateUniqueSlugAsync(command.Album.Title, command.Album.ReleaseDate, cancellationToken);
        LabelId? labelId = await ResolveLabelAsync(command.Album, cancellationToken);
        List<BandId> bandIds = await ResolveBandIdsAsync(command.Album, cancellationToken);
        Album album = CreateNewAlbum(command.Album, coverKey, slug, labelId, bandIds);

        if (command.Album.Tracks is { Count: > 0 })
        {
            List<Track> tracks = command.Album.Tracks
                .Select(t => Track.Create(TrackId.Of(Guid.NewGuid()), t.Title, t.Duration))
                .ToList();

            await context.Tracks.AddRangeAsync(tracks, cancellationToken);

            foreach ((Track track, TrackInputDto dto) in tracks.Zip(command.Album.Tracks))
                album.AddTrack(track.Id, dto.TrackNumber);
        }

        if (httpContextAccessor.HttpContext?.User.IsInRole("admin") == true)
            album.Approve();

        context.Albums.Add(album);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateAlbumResult(album.Id.Value);
    }

    private async Task ValidateReferencedEntitiesAsync(CreateAlbumDto album, CancellationToken cancellationToken)
    {
        foreach (Guid id in album.BandIds)
        {
            if (!await context.Bands.AnyAsync(b => b.Id == BandId.Of(id), cancellationToken))
                throw new BandNotFoundException(id);
        }


        foreach (Guid id in album.CountryIds)
        {
            if (!await context.Countries.AnyAsync(c => c.Id == CountryId.Of(id), cancellationToken))
                throw new CountryNotFoundException(id);
        }

        foreach (Guid id in album.GenreIds)
        {
            if (!await context.Genres.AnyAsync(g => g.Id == GenreId.Of(id), cancellationToken))
                throw new GenreNotFoundException(id);
        }

        if (album.LabelIds is { Count: > 0 })
        {
            Guid labelId = album.LabelIds[0];
            if (!await context.Labels.AnyAsync(l => l.Id == LabelId.Of(labelId), cancellationToken))
                throw new LabelNotFoundException(labelId);
        }

        foreach (Guid id in album.TagIds)
        {
            if (!await context.Tags.AnyAsync(t => t.Id == TagId.Of(id), cancellationToken))
                throw new TagNotFoundException(id);
        }
    }

    private async Task<string> GenerateUniqueSlugAsync(string title, int releaseYear, CancellationToken cancellationToken)
    {
        string baseSlug = $"{SlugHelper.Generate(title)}-{releaseYear}";
        string slug = baseSlug;
        int counter = 1;

        while (await context.Albums.AnyAsync(a => a.Slug == slug, cancellationToken))
            slug = $"{baseSlug}-{++counter}";

        return slug;
    }

    private async Task<LabelId?> ResolveLabelAsync(CreateAlbumDto album, CancellationToken cancellationToken)
    {
        if (album.LabelNames is { Count: > 0 })
        {
            string name = album.LabelNames[0].Trim();
            Label? existing = await context.Labels.FirstOrDefaultAsync(l => l.Name.ToLower() == name.ToLower(), cancellationToken);
            if (existing is not null)
                return existing.Id;

            Label newLabel = Label.Create(LabelId.Of(Guid.NewGuid()), name);
            await context.Labels.AddAsync(newLabel, cancellationToken);
            return newLabel.Id;
        }

        return album.LabelIds is { Count: > 0 } ? LabelId.Of(album.LabelIds[0]) : null;
    }

    private async Task<List<BandId>> ResolveBandIdsAsync(CreateAlbumDto album, CancellationToken cancellationToken)
    {
        List<BandId> result = album.BandIds.Select(BandId.Of).ToList();

        if (album.BandNames is not { Count: > 0 })
            return result;

        foreach (string name in album.BandNames)
        {
            string trimmed = name.Trim();
            Band? existing = await context.Bands.FirstOrDefaultAsync(b => b.Name.ToLower() == trimmed.ToLower(), cancellationToken);
            if (existing is not null)
            {
                result.Add(existing.Id);
                continue;
            }

            Band newBand = Band.Create(trimmed, null, null, BandActivity.Of(null, null), BandStatus.Unknown);
            await context.Bands.AddAsync(newBand, cancellationToken);
            result.Add(newBand.Id);
        }

        return result;
    }

    private Album CreateNewAlbum(CreateAlbumDto album, string? coverKey, string slug, LabelId? labelId, List<BandId> bandIds)
    {
        AlbumRelease albumRelease = AlbumRelease.Of(album.ReleaseDate, album.Format);

        Album newAlbum = Album.Create(album.Title, album.Type, albumRelease, coverKey, labelId, slug: slug);

        foreach (BandId bandId in bandIds)
            newAlbum.AddBand(bandId);

        foreach (Guid id in album.CountryIds)
            newAlbum.AddCountry(CountryId.Of(id));

        foreach ((Guid id, int index) in album.GenreIds.Select((id, i) => (id, i)))
            newAlbum.AddGenre(GenreId.Of(id), isPrimary: index == 0);

        foreach (Guid id in album.TagIds)
            newAlbum.AddTag(TagId.Of(id));

        foreach (StreamingLinkDto link in album.StreamingLinks)
            newAlbum.AddStreamingLink(link.Platform, link.EmbedCode);

        return newAlbum;
    }
}
