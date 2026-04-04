namespace Library.Application.Services.Albums.Queries.GetAlbumNames;

public record GetAlbumNamesQuery() : BuildingBlocks.CQRS.IQuery<GetAlbumNamesResult>;
public record GetAlbumNamesResult(List<NameIdDto> Albums);
