namespace Library.Application.Services.Bands.Queries.GetBands;

public class GetBandsQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetBandsQuery, GetBandsResult>
{
    public async ValueTask<GetBandsResult> Handle(GetBandsQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var filteredQuery = context.Bands.AsNoTracking();

        if (query.Filter is not null)
            filteredQuery = filteredQuery.Where(query.Filter.Criteria);

        var totalCount = await filteredQuery.LongCountAsync(cancellationToken);

        var bandsQuery = filteredQuery
            .Include(b => b.BandCountries)
            .Include(b => b.BandGenres);

        var sorted = query.SortBy switch
        {
            BandSortBy.Oldest     => bandsQuery.OrderBy(b => b.CreatedAt),
            BandSortBy.Name       => bandsQuery.OrderBy(b => b.Name),
            BandSortBy.FormedYear => bandsQuery.OrderByDescending(b => b.Activity.FormedYear),
            _                     => bandsQuery.OrderByDescending(b => b.CreatedAt)
        };

        var bands = await sorted
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var countryIds = bands.SelectMany(b => b.BandCountries.Select(bc => bc.CountryId)).Distinct().ToList();
        var genreIds   = bands.SelectMany(b => b.BandGenres.Select(bg => bg.GenreId)).Distinct().ToList();

        var countries = await context.Countries.AsNoTracking()
            .Where(c => countryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken);

        var genres = await context.Genres.AsNoTracking()
            .Where(g => genreIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        var bandDtos = bands.Select(b => new BandCardDto(
            b.Id.Value,
            b.Name,
            b.Slug,
            urlResolver.Resolve(b.LogoUrl),
            b.Status,
            b.Activity.FormedYear,
            b.Activity.DisbandedYear,
            b.BandGenres
                .Where(bg => bg.IsPrimary && genres.ContainsKey(bg.GenreId))
                .Select(bg => new GenreDto(genres[bg.GenreId].Id.Value, genres[bg.GenreId].Name, genres[bg.GenreId].Slug, bg.IsPrimary))
                .FirstOrDefault(),
            b.BandCountries
                .Where(bc => countries.ContainsKey(bc.CountryId))
                .Select(bc => new CountryDto(countries[bc.CountryId].Id.Value, countries[bc.CountryId].Name, countries[bc.CountryId].Code))
                .ToList()
        )).ToList();

        return new GetBandsResult(new PaginatedResult<BandCardDto>(pageIndex, pageSize, totalCount, bandDtos));
    }
}
