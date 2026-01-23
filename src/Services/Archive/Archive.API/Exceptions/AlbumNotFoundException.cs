namespace Archive.API.Exceptions
{
    public class AlbumNotFoundException : Exception
    {
        public AlbumNotFoundException(): base("Album not found!")
        {
        }
    }
}
