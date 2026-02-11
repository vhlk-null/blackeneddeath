namespace UserContent.API.Exceptions
{
    public class FavoriteAlbumNotFoundException : NotFoundException
    {
        public FavoriteAlbumNotFoundException(Guid Id) : base("FavoriteAlbum", Id)
        {
        }
    }
}
