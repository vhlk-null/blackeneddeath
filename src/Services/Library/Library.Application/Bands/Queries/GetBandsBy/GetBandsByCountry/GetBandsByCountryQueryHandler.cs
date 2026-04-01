namespace Library.Application.Bands.Queries.GetBandsBy.GetBandsByCountry;

public class GetBandsByCountryQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetBandsByCountryQuery, GetBandsByCountryResult>
{
    public async ValueTask<GetBandsByCountryResult> Handle(GetBandsByCountryQuery query, CancellationToken cancellationToken)
    {
        var countryId = CountryId.Of(query.CountryId);

        var bands = await context.Bands
            .AsNoTracking()
            .Include(b => b.BandCountries)
            .Include(b => b.BandGenres)
            .Where(b => b.BandCountries.Any(bc => bc.CountryId == countryId))
            .ToListAsync(cancellationToken);

        var bandIds = bands.Select(b => b.Id).ToList();
        var countryIds = bands.SelectMany(b => b.BandCountries.Select(bc => bc.CountryId)).Distinct().ToList();
        var genreIds = bands.SelectMany(b => b.BandGenres.Select(bg => bg.GenreId)).Distinct().ToList();

        var albumBands = await context.AlbumBands.AsNoTracking()
            .Where(ab => bandIds.Contains(ab.BandId))
            .ToListAsync(cancellationToken);

        var albumIds = albumBands.Select(ab => ab.AlbumId).Distinct().ToList();

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

        var albumsById = albums.ToDictionary(a => a.Id);
        var albumsByBand = albumBands.ToLookup(ab => ab.BandId, ab => albumsById[ab.AlbumId]);

        var bandDtos = bands
            .Select(b => b.ToBandDto(countries, genres, albumsByBand))
            .ToList();

        return new GetBandsByCountryResult(bandDtos);
    }
}
