namespace Library.Application.Dtos;

public record CreateBandDto(
    string Name,
    string? Bio,
    int? FormedYear,
    int? DisbandedYear,
    BandStatus Status,
    List<Guid> CountryIds,
    List<Guid> GenreIds);
