namespace UserContent.Domain.Models;

public class FavoriteAlbum
{
    public Guid UserId { get; set; }
    public Guid AlbumId { get; set; }
    public DateTime AddedDate { get; set; }
    public int? UserRating { get; set; }
    public string? UserReview { get; set; }
    public UserProfileInfo User { get; set; } = null!;
    public Album Album { get; set; } = null!;
}