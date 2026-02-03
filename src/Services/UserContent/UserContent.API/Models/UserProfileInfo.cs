namespace UserContent.API.Models
{
    public class UserProfileInfo
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? AvatarUrl { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string? Bio { get; set; }
        public int FavoriteBandsCount { get; set; }
        public int FavoriteAlbumsCount { get; set; }
        public int ReviewsCount { get; set; }
        public List<FavoriteBand> FavoriteBands { get; set; } = new();
        public List<FavoriteAlbum> FavoriteAlbums { get; set; } = new();
    }
}
