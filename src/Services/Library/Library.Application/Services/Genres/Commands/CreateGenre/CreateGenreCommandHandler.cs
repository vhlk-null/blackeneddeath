namespace Library.Application.Genres.Commands.CreateGenre;

public class CreateGenreCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<CreateGenreCommand, CreateGenreResult>
{
    public async ValueTask<CreateGenreResult> Handle(CreateGenreCommand command, CancellationToken cancellationToken)
    {
        var parentGenreId = command.ParentGenreId.HasValue
            ? GenreId.Of(command.ParentGenreId.Value)
            : null;

        var genre = Genre.Create(GenreId.Of(Guid.NewGuid()), command.Name, parentGenreId);

        context.Genres.Add(genre);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateGenreResult(genre.Id.Value);
    }
}
