namespace Library.Application.Services.GenreCards.Commands.AddTagToGenreCard;

public record AddTagToGenreCardCommand(Guid GenreCardId, Guid TagId)
    : BuildingBlocks.CQRS.ICommand<AddTagToGenreCardResult>;

public record AddTagToGenreCardResult(bool IsSuccess);
