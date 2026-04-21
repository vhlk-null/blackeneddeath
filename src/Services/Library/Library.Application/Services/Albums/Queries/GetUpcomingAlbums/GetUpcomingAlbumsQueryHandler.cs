namespace Library.Application.Services.Albums.Queries.GetUpcomingAlbums;

public class GetUpcomingAlbumsQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetUpcomingAlbumsQuery, GetUpcomingAlbumsResult>
{
    public async ValueTask<GetUpcomingAlbumsResult> Handle(GetUpcomingAlbumsQuery query, CancellationToken cancellationToken)
    {
        int currentYear = DateTime.UtcNow.Year;
        int pageIndex = query.PaginationRequest.PageIndex;
        int pageSize = query.PaginationRequest.PageSize;

        IQueryable<Album> baseQuery = context.Albums.AsNoTracking()
            .Where(a => a.IsApproved
                && a.AlbumRelease.ReleaseMonth != null
                && a.AlbumRelease.ReleaseDay != null
                && a.AlbumRelease.ReleaseYear >= currentYear);

        long totalCount = await baseQuery.LongCountAsync(cancellationToken);

        List<Album> albums = await baseQuery
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .OrderBy(a => a.AlbumRelease.ReleaseYear)
            .ThenBy(a => a.AlbumRelease.ReleaseMonth)
            .ThenBy(a => a.AlbumRelease.ReleaseDay)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        List<BandId> bandIds = albums.SelectMany(a => a.AlbumBands.Select(ab => ab.BandId)).Distinct().ToList();
        List<GenreId> genreIds = albums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId)).Distinct().ToList();
        List<CountryId> countryIds = albums.SelectMany(a => a.AlbumCountries.Select(ac => ac.CountryId)).Distinct().ToList();

        Dictionary<BandId, Band> bands = await context.Bands.AsNoTracking()
            .Include(b => b.BandCountries)
            .Where(b => bandIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id, cancellationToken);

        Dictionary<GenreId, Genre> genres = await context.Genres.AsNoTracking()
            .Where(g => genreIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        Dictionary<CountryId, Country> countries = await context.Countries.AsNoTracking()
            .Where(c => countryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken);

        List<AlbumCardDto> albumDtos = albums.Select(a => new AlbumCardDto(
            a.Id.Value,
            a.Title,
            a.Slug,
            a.AlbumRelease.ReleaseYear,
            a.AlbumRelease.ReleaseMonth,
            a.AlbumRelease.ReleaseDay,
            urlResolver.Resolve(a.CoverUrl),
            a.Type,
            a.AlbumRelease.Format,
            a.AlbumGenres
                .Where(ag => ag.IsPrimary && genres.ContainsKey(ag.GenreId))
                .Select(ag => new GenreDto(genres[ag.GenreId].Id.Value, genres[ag.GenreId].Name, genres[ag.GenreId].Slug, ag.IsPrimary))
                .FirstOrDefault(),
            a.AlbumBands
                .Where(ab => bands.ContainsKey(ab.BandId))
                .Select(ab => new BandRefDto(bands[ab.BandId].Id.Value, bands[ab.BandId].Name, bands[ab.BandId].Slug))
                .ToList(),
            a.AlbumCountries
                .Select(ac => ac.CountryId)
                .Where(cid => countries.ContainsKey(cid))
                .Select(cid => new CountryDto(countries[cid].Id.Value, countries[cid].Name, countries[cid].Code))
                .ToList(),
            a.IsExplicit
        )).ToList();

        return new GetUpcomingAlbumsResult(new PaginatedResult<AlbumCardDto>(pageIndex, pageSize, totalCount, albumDtos));
    }
}
