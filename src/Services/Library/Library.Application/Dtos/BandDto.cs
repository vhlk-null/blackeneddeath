namespace Library.Application.Dtos;

public record BandDto(
    Guid Id,
    string Name,
    string? Bio,
    string? LogoUrl,
    int? FormedYear,
    int? DisbandedYear,
    BandStatus Status,
    List<CountryDto> Countries,
    List<AlbumSummaryDto> Albums,
    List<GenreDto> Genres);
