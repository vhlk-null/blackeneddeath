namespace UserContent.API.UserContent.FavoriteAlbums.AddFavoriteAlbum
{
    public record AddAlbumToFavoriteCommand(Guid albumId) : ICommand<AddAlbumToFavoriteResult>;
    public record AddAlbumToFavoriteResult(bool IsSuccess);

    public class AddFavoriteAlbumCommandHandler : ICommandHandler<AddAlbumToFavoriteCommand, AddAlbumToFavoriteResult>
    {
        public async Task<AddAlbumToFavoriteResult> Handle(AddAlbumToFavoriteCommand request, CancellationToken cancellationToken)
        {
            // TODO: add album to favorite user's albums
            // TODO: update cache

            return new AddAlbumToFavoriteResult(true);
        }
    }
}
