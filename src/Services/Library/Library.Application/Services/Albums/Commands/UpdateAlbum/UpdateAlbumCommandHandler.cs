using System.Text.RegularExpressions;

namespace Library.Application.Services.Albums.Commands.UpdateAlbum;

public class UpdateAlbumCommandHandler(ILibraryDbContext context, IStorageService storage) : BuildingBlocks.CQRS.ICommandHandler<UpdateAlbumCommand, UpdateAlbumResult>
{
    public async ValueTask<UpdateAlbumResult> Handle(UpdateAlbumCommand command, CancellationToken cancellationToken)
    {
        var albumId = AlbumId.Of(command.Album.Id);

        var album = await context.Albums
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Include(a => a.StreamingLinks)
            .Include(a => a.AlbumTracks)
            .Include(a => a.AlbumTags)
            .FirstOrDefaultAsync(a => a.Id == albumId, cancellationToken)
            ?? throw new AlbumNotFoundException(command.Album.Id);

        if (command.Album.LabelIds is { Count: > 0 })
        {
            var labelGuid = command.Album.LabelIds[0];
            if (!await context.Labels.AnyAsync(l => l.Id == LabelId.Of(labelGuid), cancellationToken))
                throw new LabelNotFoundException(labelGuid);
        }

        var coverKey = album.CoverUrl;
        if (command.CoverImage is not null && command.CoverImageContentType is not null && command.CoverImageFileName is not null)
        {
            if (album.CoverUrl is not null)
                await storage.DeleteFileAsync(album.CoverUrl, cancellationToken);

            var folder = $"albums/{Slugify(command.Album.Title)}";
            var extension = Path.GetExtension(command.CoverImageFileName);
            coverKey = await storage.UploadFileAsync(folder, $"{Guid.NewGuid()}{extension}", command.CoverImage, command.CoverImageContentType, cancellationToken);
        }

        var albumRelease = AlbumRelease.Of(command.Album.ReleaseDate, command.Album.Format);
        var labelId = command.Album.LabelIds is { Count: > 0 } ? LabelId.Of(command.Album.LabelIds[0]) : null;

        album.Update(command.Album.Title, command.Album.Type, albumRelease, coverKey, labelId);

        ReconcileBands(album, command.Album);
        ReconcileCountries(album, command.Album);
        ReconcileGenres(album, command.Album);
        ReconcileTags(album, command.Album);
        ReconcileStreamingLinks(album, command.Album);

        await context.SaveChangesAsync(cancellationToken);

        return new UpdateAlbumResult(true);
    }

    private static void ReconcileBands(Album album, UpdateAlbumDto dto)
    {
        var currentIds = album.AlbumBands.Select(x => x.BandId).ToHashSet();
        var incomingIds = dto.BandIds.Select(BandId.Of).ToHashSet();

        foreach (var id in currentIds.Except(incomingIds))
            album.RemoveBand(id);

        foreach (var id in incomingIds.Except(currentIds))
            album.AddBand(id);
    }

    private static void ReconcileCountries(Album album, UpdateAlbumDto dto)
    {
        var currentIds = album.AlbumCountries.Select(x => x.CountryId).ToHashSet();
        var incomingIds = dto.CountryIds.Select(CountryId.Of).ToHashSet();

        foreach (var id in currentIds.Except(incomingIds))
            album.RemoveCountry(id);

        foreach (var id in incomingIds.Except(currentIds))
            album.AddCountry(id);
    }

    private static void ReconcileGenres(Album album, UpdateAlbumDto dto)
    {
        var currentIds = album.AlbumGenres.Select(x => x.GenreId).ToHashSet();
        var incomingIds = dto.GenreIds.Select(GenreId.Of).ToHashSet();

        foreach (var id in currentIds.Except(incomingIds))
            album.RemoveGenre(id);

        foreach (var id in incomingIds.Except(currentIds))
            album.AddGenre(id, isPrimary: false);
    }

    private static void ReconcileTags(Album album, UpdateAlbumDto dto)
    {
        var currentIds = album.AlbumTags.Select(x => x.TagId).ToHashSet();
        var incomingIds = dto.TagIds.Select(TagId.Of).ToHashSet();

        foreach (var id in currentIds.Except(incomingIds))
            album.RemoveTag(id);

        foreach (var id in incomingIds.Except(currentIds))
            album.AddTag(id);
    }

    private static void ReconcileStreamingLinks(Album album, UpdateAlbumDto dto)
    {
        var currentPlatforms = album.StreamingLinks.Select(x => x.Platform).ToHashSet();
        var incomingPlatforms = dto.StreamingLinks.Select(x => x.Platform).ToHashSet();

        foreach (var link in album.StreamingLinks.Where(l => !incomingPlatforms.Contains(l.Platform)).ToList())
            album.RemoveStreamingLink(link.Id);

        foreach (var link in dto.StreamingLinks.Where(l => !currentPlatforms.Contains(l.Platform)))
            album.AddStreamingLink(link.Platform, link.EmbedCode);
    }

    private static string Slugify(string value) =>
        Regex.Replace(value.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
}
