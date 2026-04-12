namespace UserContent.Application.Exceptions;

public class AlbumRatingNotFoundException : NotFoundException
{
    public AlbumRatingNotFoundException(Guid albumId) : base("AlbumRating", albumId)
    {
    }
}
