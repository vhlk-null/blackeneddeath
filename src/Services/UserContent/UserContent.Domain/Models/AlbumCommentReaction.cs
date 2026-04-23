namespace UserContent.Domain.Models;

public class AlbumCommentReaction
{
    public Guid UserId { get; set; }
    public Guid CommentId { get; set; }
    public bool IsLike { get; set; }

    public UserProfileInfo User { get; set; } = null!;
    public AlbumComment Comment { get; set; } = null!;
}
