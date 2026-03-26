namespace Library.Application.Services.Genres.Queries.GetGenres;

public record GetGenresQuery() : BuildingBlocks.CQRS.IQuery<GetGenresResult>;

public record GetGenresResult(IReadOnlyList<GenreDetailDto> Genres);
