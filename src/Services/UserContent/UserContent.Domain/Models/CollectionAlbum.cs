namespace UserContent.Domain.Models;

public class CollectionAlbum
{
    public Guid CollectionId { get; set; }
    public Guid AlbumId { get; set; }
    public DateTime AddedDate { get; set; }
    public Collection Collection { get; set; } = null!;
    public Album Album { get; set; } = null!;
}
