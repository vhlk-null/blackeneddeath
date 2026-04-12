namespace UserContent.Application.Exceptions;

public class BandRatingNotFoundException : NotFoundException
{
    public BandRatingNotFoundException(Guid bandId) : base("BandRating", bandId)
    {
    }
}
