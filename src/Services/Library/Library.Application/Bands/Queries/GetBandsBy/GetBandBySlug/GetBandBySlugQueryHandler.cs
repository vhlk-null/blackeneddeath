namespace Library.Application.Bands.Queries.GetBandsBy.GetBandBySlug;

public class GetBandBySlugQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetBandBySlugQuery, GetBandBySlugResult>
{
    public async ValueTask<GetBandBySlugResult> Handle(GetBandBySlugQuery query, CancellationToken cancellationToken)
    {
        var band = await context.Bands
            .AsNoTracking()
            .Include(b => b.BandCountries)
            .Include(b => b.BandGenres)
            .FirstOrDefaultAsync(b => b.Slug == query.Slug, cancellationToken)
            ?? throw new BandBySlugNotFoundException(query.Slug);

        var albumIds = await context.AlbumBands.AsNoTracking()
            .Where(ab => ab.BandId == band.Id)
            .Select(ab => ab.AlbumId)
            .ToListAsync(cancellationToken);

        var countryIds = band.BandCountries.Select(bc => bc.CountryId).ToList();
        var genreIds = band.BandGenres.Select(bg => bg.GenreId).ToList();

        var albums = await context.Albums.AsNoTracking()
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Where(a => albumIds.Contains(a.Id))
            .ToListAsync(cancellationToken);

        var allCountryIds = countryIds
            .Concat(albums.SelectMany(a => a.AlbumCountries.Select(ac => ac.CountryId)))
            .Distinct().ToList();

        var countries = await context.Countries.AsNoTracking()
            .Where(c => allCountryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken);

        var genres = await context.Genres.AsNoTracking()
            .Where(g => genreIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        var albumsByBand = albums.ToLookup(_ => band.Id);

        return new GetBandBySlugResult(band.ToBandDto(countries, genres, albumsByBand));
    }
}
