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
    GenreDto? ParentGenre,
    List<GenreDto> Subgenres,
    string? Facebook,
    string? Youtube,
    string? Instagram,
    string? Twitter,
    string? Website);
