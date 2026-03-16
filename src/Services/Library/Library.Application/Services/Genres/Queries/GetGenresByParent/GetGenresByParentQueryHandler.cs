namespace Library.Application.Services.Genres.Queries.GetGenresByParent;

public class GetGenresByParentQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetGenresByParentQuery, GetGenresByParentResult>
{
    public async ValueTask<GetGenresByParentResult> Handle(GetGenresByParentQuery query, CancellationToken cancellationToken)
    {
        var parentId = GenreId.Of(query.ParentId);

        var genres = await context.Genres
            .AsNoTracking()
            .Where(g => g.ParentGenreId == parentId)
            .ToListAsync(cancellationToken);

        return new GetGenresByParentResult(genres.Select(g => g.ToGenreDetailDto()));
    }
}
