namespace UserContent.API.Models
{
    public class UserProfile
    {
        public Guid UserId { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public string? Website { get; set; }
        public List<string> FavoriteGenres { get; set; } = new();
        public User User { get; set; } = default!;
    }
}
