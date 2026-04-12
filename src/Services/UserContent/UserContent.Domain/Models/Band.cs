namespace UserContent.Domain.Models;

public class Band
{
    public Guid BandId { get; set; }
    public required string BandName { get; set; }
    public string? LogoUrl { get; set; }
    public int ReleaseDate { get; set; }
    public double? AverageRating { get; set; }
    public int RatingsCount { get; set; }
    public List<FavoriteBand> FavoriteBands { get; set; } = new();
    public List<BandRating> Ratings { get; set; } = new();
}