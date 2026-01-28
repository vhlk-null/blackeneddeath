namespace UserContent.API.Models
{
    public class FavoriteAlbum
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AlbumId { get; set; }

        public string Title { get; set; } 
        public DateTime AddedDate { get; set; }
        public int? UserRating { get; set; } // 1-10
        public string? UserReview { get; set; }
    }
}
