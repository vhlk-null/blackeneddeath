using Microsoft.EntityFrameworkCore.Query;

namespace Library.Application.Services.Albums.Queries.GetAlbums;

public class GetAlbumsQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetAlbumsQuery, GetAlbumsResult>
{
    public async ValueTask<GetAlbumsResult> Handle(GetAlbumsQuery query, CancellationToken cancellationToken)
    {
        int pageIndex = query.PaginationRequest.PageIndex;
        int pageSize = query.PaginationRequest.PageSize;

        IQueryable<Album> filteredQuery = context.Albums.AsNoTracking();

        if (query.Filter is not null)
            filteredQuery = filteredQuery.Where(query.Filter.Criteria);

        long totalCount = await filteredQuery.LongCountAsync(cancellationToken);

        IIncludableQueryable<Album, IReadOnlyList<AlbumGenre>> albumsQuery = filteredQuery
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres);

        IOrderedQueryable<Album> sorted = query.SortBy switch
        {
            AlbumSortBy.Oldest      => albumsQuery.OrderBy(a => a.CreatedAt),
            AlbumSortBy.ReleaseDate => albumsQuery.OrderByDescending(a => a.AlbumRelease.ReleaseYear),
            AlbumSortBy.Title       => albumsQuery.OrderBy(a => a.Title),
            _                       => albumsQuery.OrderByDescending(a => a.CreatedAt)
        };

        List<Album> albums = await sorted
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        List<BandId> bandIds  = albums.SelectMany(a => a.AlbumBands.Select(ab => ab.BandId)).Distinct().ToList();
        List<GenreId> genreIds = albums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId)).Distinct().ToList();

        Dictionary<BandId, Band> bands = await context.Bands.AsNoTracking()
            .Include(b => b.BandCountries)
            .Where(b => bandIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id, cancellationToken);

        Dictionary<GenreId, Genre> genres = await context.Genres.AsNoTracking()
            .Where(g => genreIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        List<CountryId> countryIds = bands.Values.SelectMany(b => b.BandCountries.Select(bc => bc.CountryId)).Distinct().ToList();

        Dictionary<CountryId, Country> countries = await context.Countries.AsNoTracking()
            .Where(c => countryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken);

        List<AlbumCardDto> albumDtos = albums.Select(a => new AlbumCardDto(
            a.Id.Value,
            a.Title,
            a.Slug,
            a.AlbumRelease.ReleaseYear,
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
            a.AlbumBands
                .Where(ab => bands.ContainsKey(ab.BandId))
                .SelectMany(ab => bands[ab.BandId].BandCountries)
                .Select(bc => bc.CountryId)
                .Distinct()
                .Where(cid => countries.ContainsKey(cid))
                .Select(cid => new CountryDto(countries[cid].Id.Value, countries[cid].Name, countries[cid].Code))
                .ToList()
        )).ToList();

        return new GetAlbumsResult(new PaginatedResult<AlbumCardDto>(pageIndex, pageSize, totalCount, albumDtos));
    }
}
