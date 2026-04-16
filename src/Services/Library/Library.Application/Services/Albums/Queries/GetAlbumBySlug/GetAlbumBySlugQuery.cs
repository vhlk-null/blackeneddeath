namespace Library.Application.Services.Albums.Queries.GetAlbumBySlug;

public record GetAlbumBySlugQuery(string Slug, bool ApprovedOnly = true, int SimilarPageNumber = 1, int SimilarPageSize = 4) : BuildingBlocks.CQRS.IQuery<GetAlbumBySlugResult>;
public record GetAlbumBySlugResult(AlbumDetailDto Album);
