namespace Library.Application.Services.GenreCards.Commands.AddGenreToGenreCard;

public class AddGenreToGenreCardCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<AddGenreToGenreCardCommand, AddGenreToGenreCardResult>
{
    public async ValueTask<AddGenreToGenreCardResult> Handle(AddGenreToGenreCardCommand command, CancellationToken cancellationToken)
    {
        var cardId = GenreCardId.Of(command.GenreCardId);

        var card = await context.GenreCards
            .Include(c => c.GenreCardGenres)
            .FirstOrDefaultAsync(c => c.Id == cardId, cancellationToken)
            ?? throw new GenreCardNotFoundException(command.GenreCardId);

        var genreId = GenreId.Of(command.GenreId);

        if (!await context.Genres.AnyAsync(g => g.Id == genreId, cancellationToken))
            throw new GenreNotFoundException(command.GenreId);

        card.AddGenre(genreId);
        await context.SaveChangesAsync(cancellationToken);

        return new AddGenreToGenreCardResult(true);
    }
}
