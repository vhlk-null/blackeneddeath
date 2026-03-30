namespace Library.Application.Services.GenreCards.Commands.DeleteGenreCard;

public class DeleteGenreCardCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<DeleteGenreCardCommand, DeleteGenreCardResult>
{
    public async ValueTask<DeleteGenreCardResult> Handle(DeleteGenreCardCommand command, CancellationToken cancellationToken)
    {
        var card = await context.GenreCards.FindAsync([GenreCardId.Of(command.Id)], cancellationToken)
            ?? throw new GenreCardNotFoundException(command.Id);

        card.Remove();
        context.GenreCards.Remove(card);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteGenreCardResult(true);
    }
}
