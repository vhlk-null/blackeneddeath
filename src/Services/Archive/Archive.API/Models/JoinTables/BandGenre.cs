namespace Archive.API.Models.JoinTables
{
    public class BandGenre
    {
        public Guid BandId { get; set; }
        public Band Band { get; set; }

        public Guid GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
