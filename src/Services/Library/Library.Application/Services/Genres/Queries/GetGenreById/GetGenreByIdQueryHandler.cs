namespace Library.Application.Genres.Queries.GetGenreById;

public class GetGenreByIdQueryHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.IQueryHandler<GetGenreByIdQuery, GetGenreByIdResult>
{
    public async ValueTask<GetGenreByIdResult> Handle(GetGenreByIdQuery query, CancellationToken cancellationToken)
    {
        var genreId = GenreId.Of(query.Id);

        var genre = await context.Genres
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == genreId, cancellationToken)
            ?? throw new GenreNotFoundException(query.Id);

        return new GetGenreByIdResult(genre.ToGenreDetailDto());
    }
}
