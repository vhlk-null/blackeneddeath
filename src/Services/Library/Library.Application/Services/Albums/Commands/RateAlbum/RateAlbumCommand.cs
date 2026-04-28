namespace Library.Application.Services.Albums.Commands.RateAlbum;

public record RateAlbumCommand(Guid UserId, Guid AlbumId, int Rating) : BuildingBlocks.CQRS.ICommand<RateAlbumResult>;

public record RateAlbumResult(Guid AlbumId, int UserRating, double? AverageRating, int RatingsCount);
