namespace UserContent.API.UserContent.FavoriteAlbums.AddFavoriteAlbum
{
    public record AddAlbumToFavoriteCommand(Guid albumId, Guid userId) : ICommand<AddAlbumToFavoriteResult>;
    public record AddAlbumToFavoriteResult(Guid userId);

    public class AddFavoriteAlbumCommandHandler : ICommandHandler<AddAlbumToFavoriteCommand, AddAlbumToFavoriteResult>
    {
        public async ValueTask<AddAlbumToFavoriteResult> Handle(AddAlbumToFavoriteCommand request, CancellationToken cancellationToken)
        {
            // TODO: add album to favorite user's albums
            // TODO: update cache

            return new AddAlbumToFavoriteResult(new Guid());
        }
    }
}
