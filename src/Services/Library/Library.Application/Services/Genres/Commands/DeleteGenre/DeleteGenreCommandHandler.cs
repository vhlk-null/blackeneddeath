namespace Library.Application.Services.Genres.Commands.DeleteGenre;

public class DeleteGenreCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<DeleteGenreCommand, DeleteGenreResult>
{
    public async ValueTask<DeleteGenreResult> Handle(DeleteGenreCommand command, CancellationToken cancellationToken)
    {
        var genre = await context.Genres.FindAsync([GenreId.Of(command.Id)], cancellationToken)
            ?? throw new GenreNotFoundException(command.Id);

        genre.Remove();
        context.Genres.Remove(genre);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteGenreResult(true);
    }
}
