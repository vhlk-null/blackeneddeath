namespace Library.Application.Albums.Queries.GetAlbumBySlug;

public class GetAlbumBySlugQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetAlbumBySlugQuery, GetAlbumBySlugResult>
{
    public async ValueTask<GetAlbumBySlugResult> Handle(GetAlbumBySlugQuery query, CancellationToken cancellationToken)
    {
        var album = await context.Albums
            .AsNoTracking()
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Include(a => a.AlbumTracks)
            .Include(a => a.StreamingLinks)
            .FirstOrDefaultAsync(a => a.Slug == query.Slug, cancellationToken)
            ?? throw new AlbumBySlugNotFoundException(query.Slug);

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

        return new GetAlbumBySlugResult(album.ToAlbumDto(bands, genres, countries, tracks));
    }
}
