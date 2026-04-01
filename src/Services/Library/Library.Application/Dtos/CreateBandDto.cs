namespace Library.Application.Dtos;

public record CreateBandDto(
    string Name,
    string? Bio,
    int? FormedYear,
    int? DisbandedYear,
    BandStatus Status,
    List<Guid> CountryIds,
    List<Guid> GenreIds,
    string? Facebook = null,
    string? Youtube = null,
    string? Instagram = null,
    string? Twitter = null,
    string? Website = null);
