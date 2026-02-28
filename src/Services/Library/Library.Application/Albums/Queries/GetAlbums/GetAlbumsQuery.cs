namespace Library.Application.Albums.Queries.GetAlbums;

public record GetAlbumsQuery : BuildingBlocks.CQRS.IQuery<GetAlbumsResult>;
public record GetAlbumsResult(IEnumerable<AlbumDto> Albums);
