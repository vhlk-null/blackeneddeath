namespace Library.Application.Services.GenreCards.Queries.GetGenreCards;

public record GetGenreCardsQuery() : BuildingBlocks.CQRS.IQuery<GetGenreCardsResult>;

public record GetGenreCardsResult(IReadOnlyList<GenreCardDto> GenreCards);
