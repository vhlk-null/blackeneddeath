namespace Library.Application.Services.Albums.Queries.GetAlbums;

public enum AlbumSortBy { ReleaseDate, Title, CreatedAt }

public record GetAlbumsQuery(
    PaginationRequest PaginationRequest,
    AlbumSortBy SortBy = AlbumSortBy.ReleaseDate,
    SortDir SortDir = SortDir.Desc,
    ISpecification<Album>? Filter = null,
    bool ApprovedOnly = true) : BuildingBlocks.CQRS.IQuery<GetAlbumsResult>;

public record GetAlbumsResult(PaginatedResult<AlbumCardDto> Albums);
