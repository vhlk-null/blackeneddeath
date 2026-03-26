namespace Library.Application.Services.Genres.Queries.GetGenres;

public class GetGenresQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetGenresQuery, GetGenresResult>
{
    public async ValueTask<GetGenresResult> Handle(GetGenresQuery query, CancellationToken cancellationToken)
    {
        var genres = await context.Genres
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var genreDtos = genres.Select(g => g.ToGenreDetailDto()).ToList();

        return new GetGenresResult(genreDtos);
    }
}
