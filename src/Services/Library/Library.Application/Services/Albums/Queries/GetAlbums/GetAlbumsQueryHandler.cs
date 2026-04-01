namespace Library.Application.Services.Albums.Queries.GetAlbums;

public class GetAlbumsQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetAlbumsQuery, GetAlbumsResult>
{
    public async ValueTask<GetAlbumsResult> Handle(GetAlbumsQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var filteredQuery = context.Albums.AsNoTracking();

        if (query.Filter is not null)
            filteredQuery = filteredQuery.Where(query.Filter.Criteria);

        var totalCount = await filteredQuery.LongCountAsync(cancellationToken);

        var albumsQuery = filteredQuery
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres);

        var sorted = query.SortBy switch
        {
            AlbumSortBy.Oldest      => albumsQuery.OrderBy(a => a.CreatedAt),
            AlbumSortBy.ReleaseDate => albumsQuery.OrderByDescending(a => a.AlbumRelease.ReleaseYear),
            AlbumSortBy.Title       => albumsQuery.OrderBy(a => a.Title),
            _                       => albumsQuery.OrderByDescending(a => a.CreatedAt)
        };

        var albums = await sorted
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var bandIds  = albums.SelectMany(a => a.AlbumBands.Select(ab => ab.BandId)).Distinct().ToList();
        var genreIds = albums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId)).Distinct().ToList();

        var bands = await context.Bands.AsNoTracking()
            .Where(b => bandIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id, cancellationToken);

        var genres = await context.Genres.AsNoTracking()
            .Where(g => genreIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        var albumDtos = albums.Select(a => new AlbumCardDto(
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
                .ToList()
        )).ToList();

        return new GetAlbumsResult(new PaginatedResult<AlbumCardDto>(pageIndex, pageSize, totalCount, albumDtos));
    }
}
