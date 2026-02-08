namespace UserContent.API.Models
{
    public class FavoriteBand
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BandId { get; set; }
        public required string BandName { get; set; }
        public string? LogoUrl { get; set; }
        public DateTime AddedDate { get; set; }

        public int ReleaseDate { get; set; }
        public bool IsFollowing { get; set; }
        public UserProfileInfo User { get; set; } = null!;
    }
}
