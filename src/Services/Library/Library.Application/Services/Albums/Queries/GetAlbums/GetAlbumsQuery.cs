namespace Library.Application.Services.Albums.Queries.GetAlbums;

public record GetAlbumsQuery(PaginationRequest PaginationRequest) : BuildingBlocks.CQRS.IQuery<GetAlbumsResult>;
public record GetAlbumsResult(PaginatedResult<AlbumDto> Albums);
