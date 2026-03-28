namespace Library.Application.Dtos;

public record CreateAlbumDto(
    string Title,
    int ReleaseDate,
    AlbumType Type,
    AlbumFormat Format,
    Guid? LabelId,
    List<Guid> BandIds,
    List<Guid> CountryIds,
    List<Guid> GenreIds,
    List<Guid> TagIds,
    List<StreamingLinkDto> StreamingLinks,
    List<TrackInputDto>? Tracks);

public record TrackInputDto(string Title, int TrackNumber);
