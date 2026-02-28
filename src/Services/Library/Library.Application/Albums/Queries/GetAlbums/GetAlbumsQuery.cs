namespace Library.Application.Albums.Queries.GetAlbums;

public record GetAlbumsByIdQuery() : BuildingBlocks.CQRS.IQuery<GetAlbumsByIdResult>;
public record GetAlbumsByIdResult(IEnumerable<AlbumDto> Albums);