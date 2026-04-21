namespace Library.Application.Services.Albums.Queries.GetUpcomingAlbums;

public record GetUpcomingAlbumsQuery(PaginationRequest PaginationRequest)
    : BuildingBlocks.CQRS.IQuery<GetUpcomingAlbumsResult>;

public record GetUpcomingAlbumsResult(PaginatedResult<AlbumCardDto> Albums);
