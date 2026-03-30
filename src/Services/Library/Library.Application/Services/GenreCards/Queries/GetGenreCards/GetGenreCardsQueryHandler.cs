namespace Library.Application.Services.GenreCards.Queries.GetGenreCards;

public class GetGenreCardsQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetGenreCardsQuery, GetGenreCardsResult>
{
    public async ValueTask<GetGenreCardsResult> Handle(GetGenreCardsQuery query, CancellationToken cancellationToken)
    {
        var cards = await context.GenreCards
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var dtos = cards
            .Select(c => c.ToGenreCardDto(urlResolver))
            .ToList();

        return new GetGenreCardsResult(dtos);
    }
}
