using Library.API.Models.JoinTables;

namespace Library.API.Models
{
    public class Track
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ICollection<AlbumTrack> Albums { get; set; }
    }
}
