namespace UserContent.Domain.Models;

public class BandReview
{
    public Guid Id { get; set; }
    public Guid BandId { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int? Rating { get; set; }
    public DateTime? RatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public Band Band { get; set; } = null!;
}
