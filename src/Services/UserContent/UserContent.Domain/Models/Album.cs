namespace UserContent.Domain.Models;

public class Album
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? CoverUrl { get; set; }
    public int ReleaseDate { get; set; }
    public List<FavoriteAlbum> FavoriteAlbums { get; set; } = new();
}