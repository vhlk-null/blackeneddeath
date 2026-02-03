namespace UserContent.API.UserContent.FavoriteAlbums.DeleteUserFavoriteAlbums
{
    public record DeleteFavoriteAlbumCommand(Guid AlbumId, Guid UserId) : ICommand<DeleteFavoriteAlbumResult>;
    public record DeleteFavoriteAlbumResult(bool IsSuccess);

    public class DeleteFavoriteAlbumCommandHandler(IRepository<UserContentContext> repo) : ICommandHandler<DeleteFavoriteAlbumCommand, DeleteFavoriteAlbumResult>
    {
        public async ValueTask<DeleteFavoriteAlbumResult> Handle(DeleteFavoriteAlbumCommand request, CancellationToken cancellationToken)
        {
            var favoriteAlbum = await repo.GetByAsync<FavoriteAlbum>(
                fa => fa.AlbumId == request.AlbumId && fa.UserId == request.UserId)
                ?? throw new FavoriteAlbumNotFoundException(request.AlbumId);

            repo.Delete(favoriteAlbum);

            return new DeleteFavoriteAlbumResult(true);
        }
    }
}
