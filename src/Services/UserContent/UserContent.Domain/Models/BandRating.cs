namespace UserContent.Domain.Models;

public class BandRating
{
    public Guid UserId { get; set; }
    public Guid BandId { get; set; }
    public int Rating { get; set; }
    public DateTime RatedAt { get; set; }
    public UserProfileInfo User { get; set; } = null!;
    public Band Band { get; set; } = null!;
}
