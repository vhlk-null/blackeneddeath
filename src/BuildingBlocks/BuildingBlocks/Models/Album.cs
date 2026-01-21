using BuildingBlocks.Models.JoinTables;

namespace BuildingBlocks.Models
{
    public class Album
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public string? CoverUrl { get; set; }
        
        public AlbumType Type { get; set; }
        public AlbumFormat Format { get; set; }
        public string? Label { get; set; }

        // Navigation properties
        public ICollection<Country> Countries { get; set; }
        public ICollection<StreamingLink> StreamingLinks { get; set; }
        public ICollection<AlbumTrack> Tracks { get; set; }
        public ICollection<AlbumGenre> Genres { get; set; }
    }
}
