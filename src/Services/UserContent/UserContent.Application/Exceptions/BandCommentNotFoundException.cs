namespace UserContent.Application.Exceptions;

public class BandCommentNotFoundException : NotFoundException
{
    public BandCommentNotFoundException(Guid id) : base("BandComment", id)
    {
    }
}
