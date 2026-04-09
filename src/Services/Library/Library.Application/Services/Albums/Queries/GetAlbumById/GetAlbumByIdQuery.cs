namespace Library.Application.Services.Albums.Queries.GetAlbumById;

public record GetAlbumByIdQuery(Guid Id, bool ApprovedOnly = true) : BuildingBlocks.CQRS.IQuery<GetAlbumByIdResult>;
public record GetAlbumByIdResult(AlbumDetailDto Album);
