namespace Library.Application.Dtos;

public record VideoBandDto(
    Guid Id,
    Guid BandId,
    string Name,
    int? Year,
    Guid? CountryId,
    VideoType VideoType,
    string YoutubeLink,
    string? Info);
