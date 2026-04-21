namespace Library.Application.Dtos;

public record AlbumSummaryDto(
    Guid Id,
    string Title,
    string Slug,
    int ReleaseDate,
    string? CoverUrl,
    AlbumType Type,
    AlbumFormat Format,
    IReadOnlyList<GenreDto> Genres,
    IReadOnlyList<CountryDto> Countries,
    Guid BandId,
    string BandName,
    bool IsExplicit);
