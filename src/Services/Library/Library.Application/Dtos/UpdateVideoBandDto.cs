namespace Library.Application.Dtos;

public record UpdateVideoBandDto(
    Guid Id,
    Guid BandId,
    string Name,
    int? Year,
    Guid? CountryId,
    VideoType VideoType,
    string YoutubeLink,
    string? Info);
