namespace BuildingBlocks.Models
{
    public class StreamingLink
    {
        public Guid Id { get; set; }

        public StreamingPlatform Platform { get; set; }

        public string EmbedCode { get; set; } = null!;

        public Guid AddedByUserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid AlbumId { get; set; }

        public Album Album { get; set; } = null!;
    }
}