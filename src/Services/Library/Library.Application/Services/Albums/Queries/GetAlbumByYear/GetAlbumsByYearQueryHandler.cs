namespace Library.Application.Services.Albums.Queries.GetAlbumByYear;

public class GetAlbumsByYearQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetAlbumsByYearQuery, GetAlbumsByYearResult>
{
    public async ValueTask<GetAlbumsByYearResult> Handle(GetAlbumsByYearQuery query, CancellationToken cancellationToken)
    {
        var albums = await context.Albums
            .AsNoTracking()
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Include(a => a.AlbumTracks)
            .Include(a => a.StreamingLinks)
            .Where(a => a.AlbumRelease.ReleaseYear == query.ReleaseDate)
            .ToListAsync(cancellationToken);

        var bandIds = albums.SelectMany(a => a.AlbumBands.Select(ab => ab.BandId)).Distinct().ToList();
        var genreIds = albums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId)).Distinct().ToList();
        var countryIds = albums.SelectMany(a => a.AlbumCountries.Select(ac => ac.CountryId)).Distinct().ToList();
        var trackIds = albums.SelectMany(a => a.AlbumTracks.Select(at => at.TrackId)).Distinct().ToList();

        var bands = await context.Bands.AsNoTracking()
            .Where(b => bandIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id, cancellationToken);

        var genres = await context.Genres.AsNoTracking()
            .Where(g => genreIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        var countries = await context.Countries.AsNoTracking()
            .Where(c => countryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken);

        var tracks = await context.Tracks.AsNoTracking()
            .Where(t => trackIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);

        var albumDtos = albums
            .Select(a => a.ToAlbumDto(bands, genres, countries, tracks))
            .ToList();

        return new GetAlbumsByYearResult(albumDtos);
    }
}
