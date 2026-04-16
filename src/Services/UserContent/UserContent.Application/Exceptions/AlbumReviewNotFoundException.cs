namespace UserContent.Application.Exceptions;

public class AlbumReviewNotFoundException : NotFoundException
{
    public AlbumReviewNotFoundException(Guid id) : base("AlbumReview", id)
    {
    }
}
