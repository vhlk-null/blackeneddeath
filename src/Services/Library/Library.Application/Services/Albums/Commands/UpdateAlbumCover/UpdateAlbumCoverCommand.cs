namespace Library.Application.Services.Albums.Commands.UpdateAlbumCover;

public record UpdateAlbumCoverCommand(
    Guid AlbumId,
    Stream CoverImage,
    string CoverImageContentType,
    string CoverImageFileName)
    : BuildingBlocks.CQRS.ICommand<UpdateAlbumCoverResult>;

public record UpdateAlbumCoverResult(bool IsSuccess);
