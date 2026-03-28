namespace Library.Application.Dtos;

public record UpdateAlbumDto(
    Guid Id,
    string Title,
    int ReleaseDate,
    AlbumType Type,
    AlbumFormat Format,
    List<Guid>? LabelIds,
    List<Guid> BandIds,
    List<Guid> CountryIds,
    List<Guid> GenreIds,
    List<Guid> TagIds,
    List<StreamingLinkDto> StreamingLinks,
    List<TrackInputDto>? Tracks);
