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
        Album album = CreateNewAlbum(command.Album, coverKey, slug);

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

    private Album CreateNewAlbum(CreateAlbumDto album, string? coverKey, string slug)
    {
        AlbumRelease albumRelease = AlbumRelease.Of(album.ReleaseDate, album.Format);
        LabelId? labelId = album.LabelIds is { Count: > 0 } ? LabelId.Of(album.LabelIds[0]) : null;

        Album newAlbum = Album.Create(album.Title, album.Type, albumRelease, coverKey, labelId, slug: slug);

        foreach (Guid id in album.BandIds)
            newAlbum.AddBand(BandId.Of(id));

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
