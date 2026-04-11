namespace Library.Application.Dtos;

public record CreateAlbumDto(
    string Title,
    int ReleaseDate,
    AlbumType Type,
    AlbumFormat Format,
    List<Guid>? LabelIds,
    List<string>? LabelNames,
    List<Guid> BandIds,
    List<string>? BandNames,
    List<Guid> CountryIds,
    List<Guid> GenreIds,
    List<Guid> TagIds,
    List<StreamingLinkDto> StreamingLinks,
    List<TrackInputDto>? Tracks);

public record TrackInputDto(string Title, int TrackNumber, string? Duration = null);
