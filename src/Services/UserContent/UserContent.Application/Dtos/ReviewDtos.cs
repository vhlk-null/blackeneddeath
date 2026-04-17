namespace UserContent.Application.Dtos;

public enum ReviewOrderBy { Newest, Oldest, HighestRated, LowestRated }

public record ReviewDto(Guid Id, Guid UserId, string Username, string Title, string Body, DateTime CreatedAt, int? UserRating);

public record CreateAlbumReviewRequest(Guid AlbumId, Guid UserId, string Username, string Title, string Body, int? UserRating);
public record CreateBandReviewRequest(Guid BandId, Guid UserId, string Username, string Title, string Body, int? UserRating);
public record UpdateReviewRequest(string Title, string Body, int? UserRating);
