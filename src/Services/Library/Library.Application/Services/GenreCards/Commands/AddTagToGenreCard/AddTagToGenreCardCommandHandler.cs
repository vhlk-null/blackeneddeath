namespace Library.Application.Services.GenreCards.Commands.AddTagToGenreCard;

public class AddTagToGenreCardCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<AddTagToGenreCardCommand, AddTagToGenreCardResult>
{
    public async ValueTask<AddTagToGenreCardResult> Handle(AddTagToGenreCardCommand command, CancellationToken cancellationToken)
    {
        var cardId = GenreCardId.Of(command.GenreCardId);

        var card = await context.GenreCards
            .Include(c => c.GenreCardTags)
            .FirstOrDefaultAsync(c => c.Id == cardId, cancellationToken)
            ?? throw new GenreCardNotFoundException(command.GenreCardId);

        var tagId = TagId.Of(command.TagId);

        if (!await context.Tags.AnyAsync(t => t.Id == tagId, cancellationToken))
            throw new TagNotFoundException(command.TagId);

        card.AddTag(tagId);
        await context.SaveChangesAsync(cancellationToken);

        return new AddTagToGenreCardResult(true);
    }
}
