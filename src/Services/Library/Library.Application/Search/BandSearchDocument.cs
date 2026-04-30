namespace Library.Application.Search;

public record BandSearchDocument(
    string Id,
    string Name,
    string? Slug,
    string? LogoUrl,
    int? FormedYear,
    int? DisbandedYear,
    string Status,
    List<string> Genres,
    List<string> Countries,
    long CreatedAt,
    bool IsApproved = false,
    double? AverageRating = null,
    int RatingsCount = 0);
