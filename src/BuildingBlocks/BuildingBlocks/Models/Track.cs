using BuildingBlocks.Models.JoinTables;

namespace BuildingBlocks.Models
{
    public class Track
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ICollection<AlbumTrack> Albums { get; set; }
    }
}
