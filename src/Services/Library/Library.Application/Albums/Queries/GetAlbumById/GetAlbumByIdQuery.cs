namespace Library.Application.Albums.Queries.GetAlbumById;

public record GetAlbumByIdQuery(Guid Id) : BuildingBlocks.CQRS.IQuery<GetAlbumByIdResult>;
public record GetAlbumByIdResult(Album Album);