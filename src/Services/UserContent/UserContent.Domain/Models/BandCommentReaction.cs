namespace UserContent.Domain.Models;

public class BandCommentReaction
{
    public Guid UserId { get; set; }
    public Guid CommentId { get; set; }
    public bool IsLike { get; set; }

    public UserProfileInfo User { get; set; } = null!;
    public BandComment Comment { get; set; } = null!;
}
