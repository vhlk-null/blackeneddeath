namespace Library.Application.Dtos;

public record AlbumSummaryDto(
    Guid Id,
    string Title,
    int ReleaseDate,
    string? CoverUrl,
    AlbumType Type,
    AlbumFormat Format);
