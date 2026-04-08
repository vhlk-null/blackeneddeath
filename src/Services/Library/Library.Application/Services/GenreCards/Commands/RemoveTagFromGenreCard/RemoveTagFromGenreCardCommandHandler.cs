namespace Library.Application.Services.GenreCards.Commands.RemoveTagFromGenreCard;

public class RemoveTagFromGenreCardCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<RemoveTagFromGenreCardCommand, RemoveTagFromGenreCardResult>
{
    public async ValueTask<RemoveTagFromGenreCardResult> Handle(RemoveTagFromGenreCardCommand command, CancellationToken cancellationToken)
    {
        GenreCardId cardId = GenreCardId.Of(command.GenreCardId);

        GenreCard card = await context.GenreCards
                             .Include(c => c.GenreCardTags)
                             .FirstOrDefaultAsync(c => c.Id == cardId, cancellationToken)
                         ?? throw new GenreCardNotFoundException(command.GenreCardId);

        card.RemoveTag(TagId.Of(command.TagId));
        await context.SaveChangesAsync(cancellationToken);

        return new RemoveTagFromGenreCardResult(true);
    }
}
