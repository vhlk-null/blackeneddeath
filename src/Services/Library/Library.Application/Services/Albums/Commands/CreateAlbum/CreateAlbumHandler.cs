namespace Library.Application.Services.Albums.Commands.CreateAlbum;

public class CreateAlbumHandler(ILibraryDbContext context, IStorageService storage) : BuildingBlocks.CQRS.ICommandHandler<CreateAlbumCommand, CreateAlbumResult>
{
    public async ValueTask<CreateAlbumResult> Handle(CreateAlbumCommand command, CancellationToken cancellationToken)
    {
        await ValidateReferencedEntitiesAsync(command.Album, cancellationToken);

        string? coverKey = null;
        if (command.CoverImage is not null && command.CoverImageContentType is not null && command.CoverImageFileName is not null)
        {
            var folder = $"albums/{SlugHelper.Generate(command.Album.Title)}";
            var extension = Path.GetExtension(command.CoverImageFileName);
            coverKey = await storage.UploadFileAsync(folder, $"{Guid.NewGuid()}{extension}", command.CoverImage, command.CoverImageContentType, cancellationToken);
        }

        var slug = await GenerateUniqueSlugAsync(command.Album.Title, command.Album.ReleaseDate, cancellationToken);
        var album = CreateNewAlbum(command.Album, coverKey, slug);

        if (command.Album.Tracks is { Count: > 0 })
        {
            var tracks = command.Album.Tracks
                .Select(t => Track.Create(TrackId.Of(Guid.NewGuid()), t.Title, t.Duration))
                .ToList();

            await context.Tracks.AddRangeAsync(tracks, cancellationToken);

            foreach (var (track, dto) in tracks.Zip(command.Album.Tracks))
                album.AddTrack(track.Id, dto.TrackNumber);
        }

        context.Albums.Add(album);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateAlbumResult(album.Id.Value);
    }

    private async Task ValidateReferencedEntitiesAsync(CreateAlbumDto album, CancellationToken cancellationToken)
    {
        foreach (var id in album.BandIds)
        {
            if (!await context.Bands.AnyAsync(b => b.Id == BandId.Of(id), cancellationToken))
                throw new BandNotFoundException(id);
        }

        foreach (var id in album.CountryIds)
        {
            if (!await context.Countries.AnyAsync(c => c.Id == CountryId.Of(id), cancellationToken))
                throw new CountryNotFoundException(id);
        }

        foreach (var id in album.GenreIds)
        {
            if (!await context.Genres.AnyAsync(g => g.Id == GenreId.Of(id), cancellationToken))
                throw new GenreNotFoundException(id);
        }

        if (album.LabelIds is { Count: > 0 })
        {
            var labelId = album.LabelIds[0];
            if (!await context.Labels.AnyAsync(l => l.Id == LabelId.Of(labelId), cancellationToken))
                throw new LabelNotFoundException(labelId);
        }

        foreach (var id in album.TagIds)
        {
            if (!await context.Tags.AnyAsync(t => t.Id == TagId.Of(id), cancellationToken))
                throw new TagNotFoundException(id);
        }
    }

    private async Task<string> GenerateUniqueSlugAsync(string title, int releaseYear, CancellationToken cancellationToken)
    {
        var baseSlug = $"{SlugHelper.Generate(title)}-{releaseYear}";
        var slug = baseSlug;
        var counter = 1;

        while (await context.Albums.AnyAsync(a => a.Slug == slug, cancellationToken))
            slug = $"{baseSlug}-{++counter}";

        return slug;
    }

    private Album CreateNewAlbum(CreateAlbumDto album, string? coverKey, string slug)
    {
        var albumRelease = AlbumRelease.Of(album.ReleaseDate, album.Format);
        var labelId = album.LabelIds is { Count: > 0 } ? LabelId.Of(album.LabelIds[0]) : null;

        var newAlbum = Album.Create(album.Title, album.Type, albumRelease, coverKey, labelId, slug: slug);

        foreach (var id in album.BandIds)
            newAlbum.AddBand(BandId.Of(id));

        foreach (var id in album.CountryIds)
            newAlbum.AddCountry(CountryId.Of(id));

        foreach (var (id, index) in album.GenreIds.Select((id, i) => (id, i)))
            newAlbum.AddGenre(GenreId.Of(id), isPrimary: index == 0);

        foreach (var id in album.TagIds)
            newAlbum.AddTag(TagId.Of(id));

        foreach (var link in album.StreamingLinks)
            newAlbum.AddStreamingLink(link.Platform, link.EmbedCode);

        return newAlbum;
    }
}
