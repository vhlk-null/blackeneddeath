namespace UserContent.API.Models
{
    public class FavoriteBand
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BandId { get; set; }
        public string BandName { get; set; } = default!; 
        public string? LogoUrl { get; set; }
        public int? FormedYear { get; set; }

        public DateTime AddedDate { get; set; }
        public bool IsFollowing { get; set; }
    }
}
