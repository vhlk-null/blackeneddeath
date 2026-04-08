namespace Library.Application.Services.Genres.Queries.GetGenreById;

public class GetGenreByIdQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetGenreByIdQuery, GetGenreByIdResult>
{
    public async ValueTask<GetGenreByIdResult> Handle(GetGenreByIdQuery query, CancellationToken cancellationToken)
    {
        GenreId genreId = GenreId.Of(query.Id);

        Genre genre = await context.Genres
                          .AsNoTracking()
                          .FirstOrDefaultAsync(g => g.Id == genreId, cancellationToken)
                      ?? throw new GenreNotFoundException(query.Id);

        return new GetGenreByIdResult(genre.ToGenreDetailDto());
    }
}
