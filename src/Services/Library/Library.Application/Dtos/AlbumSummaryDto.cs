namespace Library.Application.Dtos;

public record AlbumSummaryDto(
    Guid Id,
    string Title,
    string Slug,
    int ReleaseDate,
    int? ReleaseMonth,
    int? ReleaseDay,
    string? CoverUrl,
    AlbumType Type,
    AlbumFormat Format,
    IReadOnlyList<GenreDto> Genres,
    IReadOnlyList<CountryDto> Countries,
    Guid BandId,
    string BandName,
    bool IsExplicit);
