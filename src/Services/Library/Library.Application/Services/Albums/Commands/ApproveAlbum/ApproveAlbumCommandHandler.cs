namespace Library.Application.Services.Albums.Commands.ApproveAlbum;

public class ApproveAlbumCommandHandler(ILibraryDbContext context)
    : BuildingBlocks.CQRS.ICommandHandler<ApproveAlbumCommand, ApproveAlbumResult>
{
    public async ValueTask<ApproveAlbumResult> Handle(
        ApproveAlbumCommand command,
        CancellationToken cancellationToken)
    {
        Album album = await context.Albums.FindAsync([AlbumId.Of(command.AlbumId)], cancellationToken)
                      ?? throw new AlbumNotFoundException(command.AlbumId);

        album.Approve();
        await context.SaveChangesAsync(cancellationToken);

        return new ApproveAlbumResult(true);
    }
}
