namespace UserContent.API.Models
{
    public class FavoriteAlbum
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AlbumId { get; set; }

        public string AlbumTitle { get; set; } = default!;
        public string? CoverUrl { get; set; }
        public int? ReleaseYear { get; set; }

        public DateTime AddedDate { get; set; }
        public int? UserRating { get; set; }
        public string? UserReview { get; set; }
    }
}