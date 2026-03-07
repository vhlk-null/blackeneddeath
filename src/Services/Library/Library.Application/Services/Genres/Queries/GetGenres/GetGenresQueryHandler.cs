namespace Library.Application.Services.Genres.Queries.GetGenres;

public class GetGenresQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetGenresQuery, GetGenresResult>
{
    public async ValueTask<GetGenresResult> Handle(GetGenresQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var totalCount = await context.Genres.LongCountAsync(cancellationToken);

        var genres = await context.Genres
            .AsNoTracking()
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var genreDtos = genres.Select(g => g.ToGenreDetailDto()).ToList();

        return new GetGenresResult(new PaginatedResult<GenreDetailDto>(pageIndex, pageSize, totalCount, genreDtos));
    }
}
