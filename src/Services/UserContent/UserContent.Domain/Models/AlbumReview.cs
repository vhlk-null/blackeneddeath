namespace UserContent.Domain.Models;

public class AlbumReview
{
    public Guid Id { get; set; }
    public Guid AlbumId { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Body { get; set; }
    public int? Rating { get; set; }
    public DateTime? RatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public Album Album { get; set; } = null!;
}
