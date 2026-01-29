using Archive.API.Models.JoinTables;

namespace Archive.API.Models
{
    public class Track
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ICollection<AlbumTrack> Albums { get; set; }
    }
}
