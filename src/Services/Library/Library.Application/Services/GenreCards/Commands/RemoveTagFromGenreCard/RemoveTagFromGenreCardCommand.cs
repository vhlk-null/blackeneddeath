namespace Library.Application.Services.GenreCards.Commands.RemoveTagFromGenreCard;

public record RemoveTagFromGenreCardCommand(Guid GenreCardId, Guid TagId)
    : BuildingBlocks.CQRS.ICommand<RemoveTagFromGenreCardResult>;

public record RemoveTagFromGenreCardResult(bool IsSuccess);
