namespace Library.Application.Dtos;

public record CreateVideoBandDto(
    Guid BandId,
    string Name,
    int? Year,
    Guid? CountryId,
    VideoType VideoType,
    string YoutubeLink,
    string? Info);
