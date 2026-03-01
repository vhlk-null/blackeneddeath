namespace Library.Application.Albums.Commands.DeleteAlbums;

public class DeleteAlbumCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<DeleteAlbumCommand, DeleteAlbumResult>
{
    public async ValueTask<DeleteAlbumResult> Handle(
        DeleteAlbumCommand command,
        CancellationToken cancellationToken)
    {
        var album = await context.Albums.FindAsync([AlbumId.Of(command.AlbumId)], cancellationToken)
            ?? throw new AlbumNotFoundException(command.AlbumId);

        context.Albums.Remove(album);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteAlbumResult(true);
    }
}
