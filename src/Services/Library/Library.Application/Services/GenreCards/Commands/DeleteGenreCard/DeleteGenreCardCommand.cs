namespace Library.Application.Services.GenreCards.Commands.DeleteGenreCard;

public record DeleteGenreCardCommand(Guid Id) : BuildingBlocks.CQRS.ICommand<DeleteGenreCardResult>;

public record DeleteGenreCardResult(bool IsSuccess);
