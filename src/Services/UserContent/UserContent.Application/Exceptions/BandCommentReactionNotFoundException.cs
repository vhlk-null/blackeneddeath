namespace UserContent.Application.Exceptions;

public class BandCommentReactionNotFoundException : NotFoundException
{
    public BandCommentReactionNotFoundException(Guid commentId, Guid userId)
        : base($"Band comment reaction for comment {commentId} by user {userId} was not found.")
    {
    }
}
