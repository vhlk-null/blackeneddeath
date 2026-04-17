namespace UserContent.Domain.Models;

public class Band
{
    public Guid BandId { get; set; }
    public required string BandName { get; set; }
    public string? Slug { get; set; }
    public string? LogoUrl { get; set; }
    public int? FormedYear { get; set; }
    public int? DisbandedYear { get; set; }
    public int Status { get; set; }
    public string? PrimaryGenreName { get; set; }
    public string? PrimaryGenreSlug { get; set; }
    public string? CountryNames { get; set; }
    public string? CountryCodes { get; set; }
    public double? AverageRating { get; set; }
    public int RatingsCount { get; set; }
    public List<FavoriteBand> FavoriteBands { get; set; } = new();
    public List<BandReview> Reviews { get; set; } = new();
}