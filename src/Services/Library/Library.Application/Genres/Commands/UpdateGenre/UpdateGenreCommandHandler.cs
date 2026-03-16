namespace Library.Application.Genres.Commands.UpdateGenre;

public class UpdateGenreCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<UpdateGenreCommand, UpdateGenreResult>
{
    public async ValueTask<UpdateGenreResult> Handle(UpdateGenreCommand command, CancellationToken cancellationToken)
    {
        var genre = await context.Genres.FindAsync([GenreId.Of(command.Id)], cancellationToken)
            ?? throw new GenreNotFoundException(command.Id);

        var parentGenreId = command.ParentGenreId.HasValue
            ? GenreId.Of(command.ParentGenreId.Value)
            : null;

        genre.Update(command.Name, parentGenreId);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateGenreResult(true);
    }
}
