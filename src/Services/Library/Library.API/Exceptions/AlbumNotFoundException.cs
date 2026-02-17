namespace Library.API.Exceptions
{
    public class AlbumNotFoundException : NotFoundException
    {
        public AlbumNotFoundException(Guid Id) : base("Album", Id)
        {
        }
    }
}
