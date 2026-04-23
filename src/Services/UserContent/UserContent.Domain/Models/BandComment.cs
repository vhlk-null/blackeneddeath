namespace UserContent.Domain.Models;

public class BandComment
{
    public Guid Id { get; set; }
    public Guid BandId { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public Guid? ParentCommentId { get; set; }
    public Guid? ReplyToCommentId { get; set; }
    public string? ReplyToUsername { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Band Band { get; set; } = null!;
    public BandComment? ParentComment { get; set; }
    public ICollection<BandComment> Replies { get; set; } = [];
    public ICollection<BandCommentReaction> Reactions { get; set; } = [];
}
