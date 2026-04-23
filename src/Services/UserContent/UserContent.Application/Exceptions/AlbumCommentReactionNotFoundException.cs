namespace UserContent.Application.Exceptions;

public class AlbumCommentReactionNotFoundException : NotFoundException
{
    public AlbumCommentReactionNotFoundException(Guid commentId, Guid userId)
        : base($"Album comment reaction for comment {commentId} by user {userId} was not found.")
    {
    }
}
