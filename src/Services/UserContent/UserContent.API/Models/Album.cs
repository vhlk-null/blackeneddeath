namespace UserContent.API.Models
{
    public class Album
    {
        public Guid AlbumId { get; set; }
        public required string AlbumTitle { get; set; }
        public string? CoverUrl { get; set; }
        public int ReleaseDate { get; set; }
        public List<FavoriteAlbum> FavoriteAlbums { get; set; } = new();
    }
}
