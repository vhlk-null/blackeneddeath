namespace UserContent.Domain.Models;

public class UserProfileInfo
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public DateTime RegisteredDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public string? Bio { get; set; }
    public int FavoriteBandsCount { get; set; }
    public int FavoriteAlbumsCount { get; set; }
    public int ReviewsCount { get; set; }
    public List<FavoriteAlbum> FavoriteAlbums { get; set; } = new();
    public List<FavoriteBand> FavoriteBands { get; set; } = new();
}