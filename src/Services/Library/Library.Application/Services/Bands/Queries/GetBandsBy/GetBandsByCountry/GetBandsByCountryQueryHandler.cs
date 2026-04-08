namespace Library.Application.Services.Bands.Queries.GetBandsBy.GetBandsByCountry;

public class GetBandsByCountryQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetBandsByCountryQuery, GetBandsByCountryResult>
{
    public async ValueTask<GetBandsByCountryResult> Handle(GetBandsByCountryQuery query, CancellationToken cancellationToken)
    {
        CountryId countryId = CountryId.Of(query.CountryId);

        List<Band> bands = await context.Bands
            .AsNoTracking()
            .Include(b => b.BandCountries)
            .Include(b => b.BandGenres)
            .Where(b => b.BandCountries.Any(bc => bc.CountryId == countryId))
            .ToListAsync(cancellationToken);

        List<BandId> bandIds = bands.Select(b => b.Id).ToList();
        List<CountryId> countryIds = bands.SelectMany(b => b.BandCountries.Select(bc => bc.CountryId)).Distinct().ToList();
        List<GenreId> genreIds = bands.SelectMany(b => b.BandGenres.Select(bg => bg.GenreId)).Distinct().ToList();

        List<AlbumBand> albumBands = await context.AlbumBands.AsNoTracking()
            .Where(ab => bandIds.Contains(ab.BandId))
            .ToListAsync(cancellationToken);

        List<AlbumId> albumIds = albumBands.Select(ab => ab.AlbumId).Distinct().ToList();

        List<Album> albums = await context.Albums.AsNoTracking()
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Where(a => albumIds.Contains(a.Id))
            .ToListAsync(cancellationToken);

        List<CountryId> allCountryIds = countryIds
            .Concat(albums.SelectMany(a => a.AlbumCountries.Select(ac => ac.CountryId)))
            .Distinct().ToList();

        Dictionary<CountryId, Country> countries = await context.Countries.AsNoTracking()
            .Where(c => allCountryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken);

        Dictionary<GenreId, Genre> genres = await context.Genres.AsNoTracking()
            .Where(g => genreIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        Dictionary<AlbumId, Album> albumsById = albums.ToDictionary(a => a.Id);
        ILookup<BandId, Album> albumsByBand = albumBands.ToLookup(ab => ab.BandId, ab => albumsById[ab.AlbumId]);

        List<BandDto> bandDtos = bands
            .Select(b => b.ToBandDto(countries, genres, albumsByBand, urlResolver))
            .ToList();

        return new GetBandsByCountryResult(bandDtos);
    }
}
