namespace Library.Application.Services.GenreCards.Commands.RemoveGenreFromGenreCard;

public class RemoveGenreFromGenreCardCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<RemoveGenreFromGenreCardCommand, RemoveGenreFromGenreCardResult>
{
    public async ValueTask<RemoveGenreFromGenreCardResult> Handle(RemoveGenreFromGenreCardCommand command, CancellationToken cancellationToken)
    {
        GenreCardId cardId = GenreCardId.Of(command.GenreCardId);

        GenreCard card = await context.GenreCards
                             .Include(c => c.GenreCardGenres)
                             .FirstOrDefaultAsync(c => c.Id == cardId, cancellationToken)
                         ?? throw new GenreCardNotFoundException(command.GenreCardId);

        card.RemoveGenre(GenreId.Of(command.GenreId));
        await context.SaveChangesAsync(cancellationToken);

        return new RemoveGenreFromGenreCardResult(true);
    }
}
