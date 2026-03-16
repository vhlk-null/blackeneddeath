namespace Library.Application.Services.Albums.Queries.GetAlbumById;

public record GetAlbumByIdQuery(Guid Id) : BuildingBlocks.CQRS.IQuery<GetAlbumByIdResult>;
public record GetAlbumByIdResult(AlbumDto Album);