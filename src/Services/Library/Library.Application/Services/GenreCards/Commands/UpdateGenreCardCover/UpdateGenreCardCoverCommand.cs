namespace Library.Application.Services.GenreCards.Commands.UpdateGenreCardCover;

public record UpdateGenreCardCoverCommand(
    Guid GenreCardId,
    Stream CoverImage,
    string CoverImageContentType,
    string CoverImageFileName)
    : BuildingBlocks.CQRS.ICommand<UpdateGenreCardCoverResult>;

public record UpdateGenreCardCoverResult(bool IsSuccess);
