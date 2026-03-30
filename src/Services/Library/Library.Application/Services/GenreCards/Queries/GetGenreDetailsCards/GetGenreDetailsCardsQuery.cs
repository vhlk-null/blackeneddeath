
namespace Library.Application.Services.GenreCards.Queries.GetGenreDetailsCards;

public record GetGenreDetailsCardsQuery() : BuildingBlocks.CQRS.IQuery<GetGenreDetailsCardsResult>;

public record GetGenreDetailsCardsResult(IReadOnlyList<GenreCardDetailDto> GenreCards);
