namespace Library.Application.Services.Bands.Queries.GetSimilarBands;

public class GetSimilarBandsQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetSimilarBandsQuery, GetSimilarBandsResult>
{
    public async ValueTask<GetSimilarBandsResult> Handle(GetSimilarBandsQuery query, CancellationToken cancellationToken)
    {
        Band band = await context.Bands
                        .AsNoTracking()
                        .Include(b => b.BandGenres)
                        .FirstOrDefaultAsync(b => b.Slug == query.Slug && (!query.ApprovedOnly || b.IsApproved), cancellationToken)
                    ?? throw new BandBySlugNotFoundException(query.Slug);

        List<GenreId> genreIds = band.BandGenres.Select(bg => bg.GenreId).ToList();

        IQueryable<Band> baseFilter = context.Bands.AsNoTracking()
            .Where(b => b.Id != band.Id && (!query.ApprovedOnly || b.IsApproved))
            .Where(b => b.BandGenres.Any(bg => genreIds.Contains(bg.GenreId)));

        int totalCount = await baseFilter.CountAsync(cancellationToken);

        List<BandId> pageIds = await baseFilter
            .OrderBy(_ => EF.Functions.Random())
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(b => b.Id)
            .ToListAsync(cancellationToken);

        List<Band> similarBands = await context.Bands.AsNoTracking()
            .Include(b => b.BandGenres)
            .Include(b => b.BandCountries)
            .Where(b => pageIds.Contains(b.Id))
            .ToListAsync(cancellationToken);

        List<CountryId> allCountryIds = similarBands
            .SelectMany(b => b.BandCountries.Select(bc => bc.CountryId))
            .Distinct().ToList();

        List<GenreId> allGenreIds = similarBands
            .SelectMany(b => b.BandGenres.Select(bg => bg.GenreId))
            .Distinct().ToList();

        Dictionary<CountryId, Country> countries = (await context.GetAllCountriesAsync(cancellationToken))
            .Where(c => allCountryIds.Contains(c.Id))
            .ToDictionary(c => c.Id);

        Dictionary<GenreId, Genre> genres = (await context.GetAllGenresAsync(cancellationToken))
            .Where(g => allGenreIds.Contains(g.Id))
            .ToDictionary(g => g.Id);

        List<BandCardDto> items = similarBands.Select(b => new BandCardDto(
            b.Id.Value,
            b.Name,
            b.Slug,
            urlResolver.Resolve(b.LogoUrl),
            b.Status,
            b.Activity.FormedYear,
            b.Activity.DisbandedYear,
            b.BandGenres.Where(bg => bg.IsPrimary && genres.ContainsKey(bg.GenreId))
                .Select(bg => new GenreDto(genres[bg.GenreId].Id.Value, genres[bg.GenreId].Name, genres[bg.GenreId].Slug, bg.IsPrimary))
                .FirstOrDefault(),
            b.BandCountries
                .Where(bc => countries.ContainsKey(bc.CountryId))
                .Select(bc => new CountryDto(countries[bc.CountryId].Id.Value, countries[bc.CountryId].Name, countries[bc.CountryId].Code))
                .ToList())).ToList();

        return new GetSimilarBandsResult(new PaginatedResult<BandCardDto>(query.PageNumber, query.PageSize, totalCount, items));
    }
}
