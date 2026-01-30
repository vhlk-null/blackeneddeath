namespace UserContent.API.UserContent.FavoriteAlbums.DeleteUserFavoriteAlbums
{
    public record DeleteFavoriteAlbumCommand(Guid AlbumId, Guid UserId) : ICommand<DeleteFavoriteAlbumResult>;
    public record DeleteFavoriteAlbumResult(bool IsSuccess);

    public class DeleteFavoriteAlbumCommandHandler : ICommandHandler<DeleteFavoriteAlbumCommand, DeleteFavoriteAlbumResult>
    {
        public async ValueTask<DeleteFavoriteAlbumResult> Handle(DeleteFavoriteAlbumCommand request, CancellationToken cancellationToken)
        {
            // TODO: delete album from user's favorite albums
            // TODO: update cache

            return new DeleteFavoriteAlbumResult(true);
        }
    }
}
