namespace Library.Application.Services.Albums.Queries.GetAlbumById;

public record GetAlbumByIdQuery(Guid Id, bool ApprovedOnly = true, int SimilarPageNumber = 1, int SimilarPageSize = 4) : BuildingBlocks.CQRS.IQuery<GetAlbumByIdResult>;
public record GetAlbumByIdResult(AlbumDetailDto Album);
