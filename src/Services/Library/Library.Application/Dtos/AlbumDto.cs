namespace Library.Application.Dtos;

public record AlbumDto(
    Guid Id,
    string Title,
    string? Slug,
    int ReleaseDate,
    string? CoverUrl,
    AlbumType Type,
    AlbumFormat Format,
    LabelDto? Label,
    List<BandSummaryDto> Bands,
    List<CountryDto> Countries,
    List<StreamingLinkDto> StreamingLinks,
    List<TrackDto> Tracks,
    List<GenreDto> Genres,
    List<TagDto> Tags);
