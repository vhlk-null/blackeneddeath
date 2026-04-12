namespace UserContent.Domain.Models;

public class AlbumRating
{
    public Guid UserId { get; set; }
    public Guid AlbumId { get; set; }
    public int Rating { get; set; }
    public DateTime RatedAt { get; set; }
    public UserProfileInfo User { get; set; } = null!;
    public Album Album { get; set; } = null!;
}
