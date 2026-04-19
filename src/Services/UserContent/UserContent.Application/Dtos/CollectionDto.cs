namespace UserContent.Application.Dtos;

public record CollectionAlbumItemDto(
    Guid Id,
    string Title,
    string? Slug,
    string? CoverUrl,
    int ReleaseDate,
    string? BandNames);

public record CollectionBandItemDto(
    Guid Id,
    string Name,
    string? Slug,
    string? LogoUrl,
    int? FormedYear);

public record CollectionDto(
    Guid Id,
    Guid UserId,
    string Name,
    string? Description,
    string Type,
    DateTime CreatedAt,
    int AlbumsCount,
    int BandsCount,
    string? CoverUrl);

public record CollectionDetailDto(
    Guid Id,
    Guid UserId,
    string Name,
    string? Description,
    string Type,
    DateTime CreatedAt,
    int AlbumsCount,
    int BandsCount,
    List<CollectionAlbumItemDto> Albums,
    List<CollectionBandItemDto> Bands,
    string? CoverUrl);

public record CollectionSummaryDto(Guid Id, Guid UserId, string Name, string Type, string? CoverUrl);

public record CreateCollectionRequest(Guid UserId, string Name, string? Description, CollectionType Type);
public record UpdateCollectionRequest(string Name, string? Description);
