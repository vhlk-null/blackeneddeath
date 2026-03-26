namespace Library.Application.Services.Albums.Commands.UpdateAlbum;

public class UpdateAlbumCommandHandler(ILibraryDbContext context) : BuildingBlocks.CQRS.ICommandHandler<UpdateAlbumCommand, UpdateAlbumResult>
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
            .FirstOrDefaultAsync(a => a.Id == albumId, cancellationToken)
            ?? throw new AlbumNotFoundException(command.Album.Id);

        if (command.Album.Label?.Id is Guid labelGuid && labelGuid != Guid.Empty)
        {
            var lid = LabelId.Of(labelGuid);
            if (!await context.Labels.AnyAsync(l => l.Id == lid, cancellationToken))
                throw new LabelNotFoundException(labelGuid);
        }

        UpdateAlbum(album, command.Album);

        await context.SaveChangesAsync(cancellationToken);

        return new UpdateAlbumResult(true);
    }

    private static void UpdateAlbum(Album album, AlbumDto dto)
    {
        var albumRelease = AlbumRelease.Of(dto.ReleaseDate, dto.Format);
        var labelId = dto.Label?.Id is Guid lid && lid != Guid.Empty ? LabelId.Of(lid) : null;

        album.Update(dto.Title, dto.Type, albumRelease, dto.CoverUrl, labelId);

        ReconcileBands(album, dto);
        ReconcileCountries(album, dto);
        ReconcileGenres(album, dto);
        ReconcileStreamingLinks(album, dto);
        ReconcileTracks(album, dto);
    }

    private static void ReconcileBands(Album album, AlbumDto dto)
    {
        var currentIds = album.AlbumBands.Select(x => x.BandId).ToHashSet();
        var incomingIds = dto.Bands
            .Where(x => x.Id.HasValue && x.Id.Value != Guid.Empty)
            .Select(x => BandId.Of(x.Id!.Value))
            .ToHashSet();

        foreach (var id in currentIds.Except(incomingIds))
            album.RemoveBand(id);

        foreach (var id in incomingIds.Except(currentIds))
            album.AddBand(id);
    }

    private static void ReconcileCountries(Album album, AlbumDto dto)
    {
        var currentIds = album.AlbumCountries.Select(x => x.CountryId).ToHashSet();
        var incomingIds = dto.Countries.Select(x => CountryId.Of(x.Id)).ToHashSet();

        foreach (var id in currentIds.Except(incomingIds))
            album.RemoveCountry(id);

        foreach (var id in incomingIds.Except(currentIds))
            album.AddCountry(id);
    }

    private static void ReconcileGenres(Album album, AlbumDto dto)
    {
        var currentIds = album.AlbumGenres.Select(x => x.GenreId).ToHashSet();
        var incomingIds = dto.Genres.Select(x => GenreId.Of(x.Id)).ToHashSet();

        foreach (var id in currentIds.Except(incomingIds))
            album.RemoveGenre(id);

        foreach (var genre in dto.Genres.Where(g => !currentIds.Contains(GenreId.Of(g.Id))))
            album.AddGenre(GenreId.Of(genre.Id), genre.IsPrimary);
    }

    private static void ReconcileStreamingLinks(Album album, AlbumDto dto)
    {
        var currentPlatforms = album.StreamingLinks.Select(x => x.Platform).ToHashSet();
        var incomingPlatforms = dto.StreamingLinks.Select(x => x.Platform).ToHashSet();

        foreach (var link in album.StreamingLinks.Where(l => !incomingPlatforms.Contains(l.Platform)).ToList())
            album.RemoveStreamingLink(link.Id);

        foreach (var link in dto.StreamingLinks.Where(l => !currentPlatforms.Contains(l.Platform)))
            album.AddStreamingLink(link.Platform, link.EmbedCode);
    }

    private static void ReconcileTracks(Album album, AlbumDto dto)
    {
        var currentIds = album.AlbumTracks.Select(x => x.TrackId).ToHashSet();
        var incomingIds = dto.Tracks.Select(x => TrackId.Of(x.Id)).ToHashSet();

        foreach (var id in currentIds.Except(incomingIds))
            album.RemoveTrack(id);

        foreach (var track in dto.Tracks.Where(t => !currentIds.Contains(TrackId.Of(t.Id))))
            album.AddTrack(TrackId.Of(track.Id), track.TrackNumber);
    }
}
