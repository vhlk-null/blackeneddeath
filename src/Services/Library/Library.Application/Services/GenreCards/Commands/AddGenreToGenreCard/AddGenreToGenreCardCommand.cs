namespace Library.Application.Services.GenreCards.Commands.AddGenreToGenreCard;

public record AddGenreToGenreCardCommand(Guid GenreCardId, Guid GenreId)
    : BuildingBlocks.CQRS.ICommand<AddGenreToGenreCardResult>;

public record AddGenreToGenreCardResult(bool IsSuccess);
