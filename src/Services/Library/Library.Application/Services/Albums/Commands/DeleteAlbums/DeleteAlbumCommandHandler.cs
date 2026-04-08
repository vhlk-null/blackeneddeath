namespace Library.Application.Services.Albums.Commands.DeleteAlbums;

public class DeleteAlbumCommandHandler(ILibraryDbContext context, IStorageService storage)
    : BuildingBlocks.CQRS.ICommandHandler<DeleteAlbumCommand, DeleteAlbumResult>
{
    public async ValueTask<DeleteAlbumResult> Handle(
        DeleteAlbumCommand command,
        CancellationToken cancellationToken)
    {
        Album album = await context.Albums.FindAsync([AlbumId.Of(command.AlbumId)], cancellationToken)
                      ?? throw new AlbumNotFoundException(command.AlbumId);

        if (album.CoverUrl is not null)
            await storage.DeleteFileAsync(album.CoverUrl, cancellationToken);

        album.Delete();
        context.Albums.Remove(album);
        await context.SaveChangesAsync(cancellationToken);

        return new DeleteAlbumResult(true);
    }
}
