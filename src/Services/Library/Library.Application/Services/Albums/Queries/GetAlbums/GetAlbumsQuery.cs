namespace Library.Application.Services.Albums.Queries.GetAlbums;

public enum AlbumSortBy { Newest, Oldest, ReleaseDate, Title }

public record GetAlbumsQuery(
    PaginationRequest PaginationRequest,
    AlbumSortBy SortBy = AlbumSortBy.Newest,
    ISpecification<Album>? Filter = null,
    bool ApprovedOnly = true) : BuildingBlocks.CQRS.IQuery<GetAlbumsResult>;

public record GetAlbumsResult(PaginatedResult<AlbumCardDto> Albums);
