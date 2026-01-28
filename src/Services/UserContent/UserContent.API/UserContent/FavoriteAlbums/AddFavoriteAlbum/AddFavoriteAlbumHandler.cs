using BuildingBlocks.Models;

namespace UserContent.API.UserContent.FavoriteAlbums.AddFavoriteAlbum
{

    public record AddAlbumToFavoriteCommand(Guid albumId);
    public record AddAlbumToFavoriteResult(Album album);
    public class StoreUserFavoriteAlbums
    {
    }
}
