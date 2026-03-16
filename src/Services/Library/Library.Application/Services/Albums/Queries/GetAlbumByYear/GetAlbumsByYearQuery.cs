namespace Library.Application.Services.Albums.Queries.GetAlbumByYear;

public record GetAlbumsByYearQuery(int ReleaseDate) : BuildingBlocks.CQRS.IQuery<GetAlbumsByYearResult>;
public record GetAlbumsByYearResult(IEnumerable<AlbumDto> Albums);
