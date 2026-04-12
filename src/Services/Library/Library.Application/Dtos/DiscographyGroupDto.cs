namespace Library.Application.Dtos;

public record DiscographyGroupDto(
    Guid? BandId,
    string? BandName,
    string? BandSlug,
    string Label,
    List<AlbumSummaryDto> Albums);
