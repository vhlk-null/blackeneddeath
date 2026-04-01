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

        var albumTrackIds = album.AlbumTracks.Select(x => x.TrackId).ToList();
        var albumTracks = await context.Tracks
            .Where(t => albumTrackIds.Contains(t.Id))
            .ToListAsync(cancellationToken);

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
        await ReconcileTracksAsync(album, albumTracks, command.Album, cancellationToken);

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
        var incomingOrdered = dto.GenreIds.Select(GenreId.Of).ToList();
        var incomingIds = incomingOrdered.ToHashSet();

        foreach (var id in currentIds.Except(incomingIds))
            album.RemoveGenre(id);

        foreach (var (genreId, index) in incomingOrdered.Select((id, i) => (id, i)))
        {
            if (!currentIds.Contains(genreId))
                album.AddGenre(genreId, isPrimary: index == 0);
        }
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
        var incomingByPlatform = dto.StreamingLinks.ToDictionary(x => x.Platform);

        foreach (var link in album.StreamingLinks.ToList())
        {
            if (!incomingByPlatform.ContainsKey(link.Platform))
                album.RemoveStreamingLink(link.Id);
        }

        var currentByPlatform = album.StreamingLinks.ToDictionary(x => x.Platform);

        foreach (var incoming in dto.StreamingLinks)
        {
            if (currentByPlatform.TryGetValue(incoming.Platform, out var existing))
            {
                if (existing.EmbedCode != incoming.EmbedCode)
                    album.UpdateStreamingLink(existing.Id, incoming.EmbedCode);
            }
            else
            {
                album.AddStreamingLink(incoming.Platform, incoming.EmbedCode);
            }
        }
    }

    private async Task ReconcileTracksAsync(Album album, List<Track> currentTracks, UpdateAlbumDto dto, CancellationToken cancellationToken)
    {
        if (dto.Tracks is null or { Count: 0 })
            return;

        var incomingByNumber = dto.Tracks.ToDictionary(t => t.TrackNumber);
        var tracksById       = currentTracks.ToDictionary(t => t.Id);
        var currentByNumber  = album.AlbumTracks
            .Where(at => tracksById.ContainsKey(at.TrackId))
            .ToDictionary(at => at.TrackNumber, at => tracksById[at.TrackId]);

        // Remove tracks no longer in the list
        foreach (var (number, track) in currentByNumber)
        {
            if (!incomingByNumber.ContainsKey(number))
            {
                album.RemoveTrack(track.Id);
                context.Tracks.Remove(track);
            }
        }

        // Update existing or add new
        foreach (var incoming in dto.Tracks)
        {
            if (currentByNumber.TryGetValue(incoming.TrackNumber, out var existing))
            {
                if (existing.Title != incoming.Title)
                    existing.UpdateTitle(incoming.Title);

                if (existing.Duration != incoming.Duration)
                    existing.UpdateDuration(incoming.Duration);
            }
            else
            {
                var newTrack = Track.Create(TrackId.Of(Guid.NewGuid()), incoming.Title, incoming.Duration);
                await context.Tracks.AddAsync(newTrack, cancellationToken);
                album.AddTrack(newTrack.Id, incoming.TrackNumber);
            }
        }
    }

    private static string Slugify(string value) =>
        Regex.Replace(value.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
}
