namespace UserContent.Application.UserContent.FavoriteAlbums.DeleteFavoriteAlbum;

public class DeleteFavoriteAlbumCommandHandler(IUserContentRepository repo)
    : ICommandHandler<DeleteFavoriteAlbumCommand, DeleteFavoriteAlbumResult>
{
    public async ValueTask<DeleteFavoriteAlbumResult> Handle(DeleteFavoriteAlbumCommand request, CancellationToken cancellationToken)
    {
        var favoriteAlbum = await repo.GetFavoriteAlbumAsync(request.UserId, request.AlbumId, cancellationToken)
            ?? throw new FavoriteAlbumNotFoundException(request.AlbumId);

        repo.Remove(favoriteAlbum);

        return new DeleteFavoriteAlbumResult(true);
    }
}
