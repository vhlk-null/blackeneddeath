namespace UserContent.API.Models
{
    public class FavoriteAlbum
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AlbumId { get; set; }
        public required string AlbumTitle { get; set; }
        public string? CoverUrl { get; set; }
        public int ReleaseDate { get; set; }
        public DateTime AddedDate { get; set; }
        public int? UserRating { get; set; }
        public string? UserReview { get; set; }
        public UserProfileInfo User { get; set; } = null!;
    }
}