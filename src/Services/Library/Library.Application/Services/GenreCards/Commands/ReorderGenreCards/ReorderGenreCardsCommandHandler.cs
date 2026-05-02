namespace Library.Application.Services.GenreCards.Commands.ReorderGenreCards;

public class ReorderGenreCardsCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<ReorderGenreCardsCommand, ReorderGenreCardsResult>
{
    public async ValueTask<ReorderGenreCardsResult> Handle(ReorderGenreCardsCommand command, CancellationToken cancellationToken)
    {
        List<GenreCard> cards = await context.GenreCards.ToListAsync(cancellationToken);

        for (int i = 0; i < command.OrderedIds.Count; i++)
        {
            GenreCard? card = cards.FirstOrDefault(c => c.Id.Value == command.OrderedIds[i]);
            card?.SetOrderNumber(i);
        }

        await context.SaveChangesAsync(cancellationToken);

        return new ReorderGenreCardsResult(true);
    }
}
