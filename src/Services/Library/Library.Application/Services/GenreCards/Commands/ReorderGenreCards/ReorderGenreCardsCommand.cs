namespace Library.Application.Services.GenreCards.Commands.ReorderGenreCards;

public record ReorderGenreCardsCommand(List<Guid> OrderedIds) : BuildingBlocks.CQRS.ICommand<ReorderGenreCardsResult>;

public record ReorderGenreCardsResult(bool IsSuccess);
