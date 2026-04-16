namespace Library.Application.Albums.Queries.GetAlbumBySlug;

public record GetAlbumBySlugQuery(string Slug) : BuildingBlocks.CQRS.IQuery<GetAlbumBySlugResult>;
public record GetAlbumBySlugResult(AlbumDto Album);
