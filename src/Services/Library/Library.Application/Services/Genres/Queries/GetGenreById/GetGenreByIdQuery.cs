namespace Library.Application.Services.Genres.Queries.GetGenreById;

public record GetGenreByIdQuery(Guid Id) : BuildingBlocks.CQRS.IQuery<GetGenreByIdResult>;

public record GetGenreByIdResult(GenreDetailDto Genre);
