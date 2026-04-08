namespace Library.Application.Services.GenreCards.Queries.GetGenreCards;

public class GetGenreCardsQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetGenreCardsQuery, GetGenreCardsResult>
{
    public async ValueTask<GetGenreCardsResult> Handle(GetGenreCardsQuery query, CancellationToken cancellationToken)
    {
        List<GenreCard> cards = await context.GenreCards
            .AsNoTracking()
            .OrderBy(c => c.OrderNumber)
            .ToListAsync(cancellationToken);

        List<GenreCardDto> dtos = cards
            .Select(c => c.ToGenreCardDto(urlResolver))
            .ToList();

        return new GetGenreCardsResult(dtos);
    }
}
