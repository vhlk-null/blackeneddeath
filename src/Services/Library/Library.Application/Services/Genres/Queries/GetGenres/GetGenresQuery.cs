namespace Library.Application.Services.Genres.Queries.GetGenres;

public record GetGenresQuery(PaginationRequest PaginationRequest) : BuildingBlocks.CQRS.IQuery<GetGenresResult>;

public record GetGenresResult(PaginatedResult<GenreDetailDto> Genres);
