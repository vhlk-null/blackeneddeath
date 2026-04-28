namespace Library.Application.Dtos;

public record BandCardDto(
    Guid Id,
    string Name,
    string? Slug,
    string? LogoUrl,
    BandStatus Status,
    int? FormedYear,
    int? DisbandedYear,
    GenreDto? PrimaryGenre,
    List<CountryDto> Countries,
    double? AverageRating = null,
    int RatingsCount = 0);
