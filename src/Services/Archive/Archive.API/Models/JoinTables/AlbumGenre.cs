namespace Archive.API.Models.JoinTables
{
    public class AlbumGenre
    {
        public Guid AlbumId { get; set; }

        public Album Album { get; set; }

        public Guid GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
