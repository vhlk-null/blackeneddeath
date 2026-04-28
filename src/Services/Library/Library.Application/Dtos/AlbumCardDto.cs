namespace Library.Application.Dtos;

public record AlbumCardDto(
    Guid Id,
    string Title,
    string? Slug,
    int ReleaseDate,
    int? ReleaseMonth,
    int? ReleaseDay,
    string? CoverUrl,
    AlbumType Type,
    AlbumFormat Format,
    GenreDto? PrimaryGenre,
    List<BandRefDto> Bands,
    List<CountryDto> Countries,
    bool IsExplicit,
    double? AverageRating = null,
    int RatingsCount = 0);
