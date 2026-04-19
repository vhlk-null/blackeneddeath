namespace UserContent.Domain.Models;

public class Collection
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Type { get; set; }
    public string? CoverUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserProfileInfo User { get; set; } = null!;
    public List<CollectionAlbum> CollectionAlbums { get; set; } = new();
    public List<CollectionBand> CollectionBands { get; set; } = new();
}
