namespace Library.Application.Dtos;

public record BandDto(
    Guid Id,
    string Name,
    string? Slug,
    string? Bio,
    string? LogoUrl,
    int? FormedYear,
    int? DisbandedYear,
    BandStatus Status,
    List<CountryDto> Countries,
    List<AlbumSummaryDto> Albums,
    List<GenreDto> Genres,
    string? Facebook,
    string? Youtube,
    string? Instagram,
    string? Twitter,
    string? Website);

public record BandDetailDto(
    Guid Id,
    string Name,
    string? Slug,
    string? Bio,
    string? LogoUrl,
    int? FormedYear,
    int? DisbandedYear,
    BandStatus Status,
    List<CountryDto> Countries,
    List<AlbumSummaryDto> Albums,
    List<GenreDto> Genres,
    string? Facebook,
    string? Youtube,
    string? Instagram,
    string? Twitter,
    string? Website,
    List<BandCardDto> SimilarBands);
