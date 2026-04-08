namespace Library.Application.Services.Genres.Queries.GetGenres;

public class GetGenresQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetGenresQuery, GetGenresResult>
{
    public async ValueTask<GetGenresResult> Handle(GetGenresQuery query, CancellationToken cancellationToken)
    {
        List<Genre> genres = await context.Genres
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        List<GenreDetailDto> genreDtos = genres.Select(g => g.ToGenreDetailDto()).ToList();

        return new GetGenresResult(genreDtos);
    }
}
