namespace UserContent.Application.Exceptions;

public class BandReviewNotFoundException : NotFoundException
{
    public BandReviewNotFoundException(Guid id) : base("BandReview", id)
    {
    }
}
