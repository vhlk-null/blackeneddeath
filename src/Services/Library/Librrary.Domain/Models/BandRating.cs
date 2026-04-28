namespace Library.Domain.Models;

public class BandRating
{
    public Guid UserId { get; private set; }
    public BandId BandId { get; private set; } = null!;
    public int Rating { get; private set; }
    public DateTime RatedAt { get; private set; }

    public Band Band { get; private set; } = null!;

    private BandRating() { }

    public static BandRating Create(Guid userId, BandId bandId, int rating)
    {
        if (rating is < 1 or > 10)
            throw new DomainException("Rating must be between 1 and 10.");

        return new BandRating
        {
            UserId = userId,
            BandId = bandId,
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
