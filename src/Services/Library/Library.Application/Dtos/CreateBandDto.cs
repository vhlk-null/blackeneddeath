namespace Library.Application.Dtos;

public record CreateBandDto(
    string Name,
    string? Bio,
    int? FormedYear,
    int? DisbandedYear,
    BandStatus Status,
    List<Guid> CountryIds,
    List<BandGenreInputDto> Genres);

public record BandGenreInputDto(Guid Id, bool IsPrimary);
