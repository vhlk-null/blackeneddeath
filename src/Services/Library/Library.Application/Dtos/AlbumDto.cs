namespace Library.Application.Dtos;

public record AlbumDto(
    Guid Id,
    string Title,
    int ReleaseDate,
    string? CoverUrl,
    AlbumType Type,
    AlbumFormat Format,
    string? Label,
    List<BandSummaryDto> Bands,
    List<CountryDto> Countries,
    List<StreamingLinkDto> StreamingLinks,
    List<TrackDto> Tracks,
    List<GenreDto> Genres);
