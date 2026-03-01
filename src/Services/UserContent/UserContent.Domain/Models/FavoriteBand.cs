namespace UserContent.Domain.Models
{
    public class FavoriteBand
    {
        public Guid UserId { get; set; }
        public Guid BandId { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsFollowing { get; set; }
        public UserProfileInfo User { get; set; } = null!;
        public Band Band { get; set; } = null!;
    }
}
