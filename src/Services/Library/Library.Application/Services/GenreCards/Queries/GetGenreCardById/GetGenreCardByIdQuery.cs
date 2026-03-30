namespace Library.Application.Services.GenreCards.Queries.GetGenreCardById;

public record GetGenreCardByIdQuery(Guid Id) : BuildingBlocks.CQRS.IQuery<GetGenreCardByIdResult>;

public record GetGenreCardByIdResult(GenreCardDto GenreCard);
