namespace Library.Application.Services.GenreCards.Queries.GetAlbumsByGenreCard;

public record GetAlbumsByGenreCardQuery(Guid GenreCardId, PaginationRequest PaginationRequest)
    : BuildingBlocks.CQRS.IQuery<GetAlbumsByGenreCardResult>;

public record GetAlbumsByGenreCardResult(PaginatedResult<AlbumDto> Albums);
