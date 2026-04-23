namespace UserContent.Application.Exceptions;

public class AlbumCommentNotFoundException : NotFoundException
{
    public AlbumCommentNotFoundException(Guid id) : base("AlbumComment", id)
    {
    }
}
