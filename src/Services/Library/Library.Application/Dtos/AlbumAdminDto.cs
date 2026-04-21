namespace Library.Application.Dtos;

public record AlbumAdminDto(
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
    bool IsApproved,
    string? CreatedBy);
