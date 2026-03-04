namespace UserContent.Application.Exceptions;

public class FavoriteBandNotFoundException : NotFoundException
{
    public FavoriteBandNotFoundException(Guid id) : base("FavoriteBand", id)
    {
    }
}
