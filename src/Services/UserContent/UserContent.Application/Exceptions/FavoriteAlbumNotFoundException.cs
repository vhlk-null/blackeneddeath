namespace UserContent.Application.Exceptions;

public class FavoriteAlbumNotFoundException : NotFoundException
{
    public FavoriteAlbumNotFoundException(Guid id) : base("FavoriteAlbum", id)
    {
    }
}
