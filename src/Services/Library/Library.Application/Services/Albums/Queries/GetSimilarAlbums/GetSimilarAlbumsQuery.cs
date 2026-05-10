namespace Library.Application.Services.Albums.Queries.GetSimilarAlbums;

public record GetSimilarAlbumsQuery(string Slug, bool ApprovedOnly = true, int PageNumber = 1, int PageSize = 4)
    : BuildingBlocks.CQRS.IQuery<GetSimilarAlbumsResult>;

public record GetSimilarAlbumsResult(PaginatedResult<AlbumSummaryDto> SimilarAlbums);
