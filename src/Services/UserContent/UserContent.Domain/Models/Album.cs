namespace UserContent.Domain.Models;

public class Album
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? CoverUrl { get; set; }
    public int ReleaseDate { get; set; }
    public double? AverageRating { get; set; }
    public int RatingsCount { get; set; }
    public List<FavoriteAlbum> FavoriteAlbums { get; set; } = new();
    public List<AlbumRating> Ratings { get; set; } = new();
}