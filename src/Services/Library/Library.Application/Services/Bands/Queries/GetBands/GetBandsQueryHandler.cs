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

        var bandIds = bands.Select(b => b.Id).ToList();
        var countryIds = bands.SelectMany(b => b.BandCountries.Select(bc => bc.CountryId)).Distinct().ToList();
        var genreIds = bands.SelectMany(b => b.BandGenres.Select(bg => bg.GenreId)).Distinct().ToList();

        var albumBands = await context.AlbumBands.AsNoTracking()
            .Where(ab => bandIds.Contains(ab.BandId))
            .ToListAsync(cancellationToken);

        var albumIds = albumBands.Select(ab => ab.AlbumId).Distinct().ToList();

        var albums = await context.Albums.AsNoTracking()
            .Where(a => albumIds.Contains(a.Id))
            .ToListAsync(cancellationToken);

        var countries = await context.Countries.AsNoTracking()
            .Where(c => countryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken);

        var genres = await context.Genres.AsNoTracking()
            .Where(g => genreIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        var albumsById = albums.ToDictionary(a => a.Id);
        var albumsByBand = albumBands.ToLookup(ab => ab.BandId, ab => albumsById[ab.AlbumId]);

        var bandDtos = bands
            .Select(b => b.ToBandDto(countries, genres, albumsByBand, urlResolver))
            .ToList();

        return new GetBandsResult(new PaginatedResult<BandDto>(pageIndex, pageSize, totalCount, bandDtos));
    }
}
