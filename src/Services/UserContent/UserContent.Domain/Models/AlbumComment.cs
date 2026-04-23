namespace UserContent.Domain.Models;

public class AlbumComment
{
    public Guid Id { get; set; }
    public Guid AlbumId { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public Guid? ParentCommentId { get; set; }
    public Guid? ReplyToCommentId { get; set; }
    public string? ReplyToUsername { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Album Album { get; set; } = null!;
    public AlbumComment? ParentComment { get; set; }
    public ICollection<AlbumComment> Replies { get; set; } = [];
    public ICollection<AlbumCommentReaction> Reactions { get; set; } = [];
}
