namespace Library.Application.Services.Albums.Queries.GetAlbumById;

public class GetAlbumByIdQueryHandler(ILibraryDbContext context) : BuildingBlocks.CQRS.IQueryHandler<GetAlbumByIdQuery, GetAlbumByIdResult>
{
    public async ValueTask<GetAlbumByIdResult> Handle(GetAlbumByIdQuery query, CancellationToken cancellationToken)
    {
        var albumId = AlbumId.Of(query.Id);

        var album = await context.Albums
            .AsNoTracking()
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Include(a => a.AlbumTracks)
            .Include(a => a.StreamingLinks)
            .FirstOrDefaultAsync(a => a.Id == albumId, cancellationToken)
            ?? throw new AlbumNotFoundException(query.Id);

        var bands = await context.Bands.AsNoTracking()
            .Where(b => album.AlbumBands.Select(ab => ab.BandId).Contains(b.Id))
            .ToDictionaryAsync(b => b.Id, cancellationToken);

        var genres = await context.Genres.AsNoTracking()
            .Where(g => album.AlbumGenres.Select(ag => ag.GenreId).Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        var countries = await context.Countries.AsNoTracking()
            .Where(c => album.AlbumCountries.Select(ac => ac.CountryId).Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken);

        var tracks = await context.Tracks.AsNoTracking()
            .Where(t => album.AlbumTracks.Select(at => at.TrackId).Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);

        return new GetAlbumByIdResult(album.ToAlbumDto(bands, genres, countries, tracks));
    }
}
