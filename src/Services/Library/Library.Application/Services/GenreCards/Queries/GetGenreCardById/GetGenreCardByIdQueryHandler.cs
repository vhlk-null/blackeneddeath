namespace Library.Application.Services.GenreCards.Queries.GetGenreCardById;

public class GetGenreCardByIdQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetGenreCardByIdQuery, GetGenreCardByIdResult>
{
    public async ValueTask<GetGenreCardByIdResult> Handle(GetGenreCardByIdQuery query, CancellationToken cancellationToken)
    {
        GenreCardId cardId = GenreCardId.Of(query.Id);

        GenreCard card = await context.GenreCards
                             .AsNoTracking()
                             .FirstOrDefaultAsync(c => c.Id == cardId, cancellationToken)
                         ?? throw new GenreCardNotFoundException(query.Id);

        return new GetGenreCardByIdResult(card.ToGenreCardDto(urlResolver));
    }
}
