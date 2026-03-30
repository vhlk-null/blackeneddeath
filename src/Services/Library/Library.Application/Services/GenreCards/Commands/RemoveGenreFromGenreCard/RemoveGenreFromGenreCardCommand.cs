namespace Library.Application.Services.GenreCards.Commands.RemoveGenreFromGenreCard;

public record RemoveGenreFromGenreCardCommand(Guid GenreCardId, Guid GenreId)
    : BuildingBlocks.CQRS.ICommand<RemoveGenreFromGenreCardResult>;

public record RemoveGenreFromGenreCardResult(bool IsSuccess);
