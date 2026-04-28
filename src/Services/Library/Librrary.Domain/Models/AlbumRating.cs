namespace Library.Domain.Models;

public class AlbumRating
{
    public Guid UserId { get; private set; }
    public AlbumId AlbumId { get; private set; } = null!;
    public int Rating { get; private set; }
    public DateTime RatedAt { get; private set; }

    public Album Album { get; private set; } = null!;

    private AlbumRating() { }

    public static AlbumRating Create(Guid userId, AlbumId albumId, int rating)
    {
        if (rating is < 1 or > 10)
            throw new DomainException("Rating must be between 1 and 10.");

        return new AlbumRating
        {
            UserId = userId,
            AlbumId = albumId,
            Rating = rating,
            RatedAt = DateTime.UtcNow
        };
    }

    public void Update(int rating)
    {
        if (rating is < 1 or > 10)
            throw new DomainException("Rating must be between 1 and 10.");

        Rating = rating;
        RatedAt = DateTime.UtcNow;
    }
}
