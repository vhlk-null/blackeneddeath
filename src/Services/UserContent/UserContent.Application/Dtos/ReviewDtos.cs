namespace UserContent.Application.Dtos;

public record ReviewDto(Guid Id, Guid UserId, string Username, string Title, string Body, DateTime CreatedAt, int? UserRating);

public record CreateAlbumReviewRequest(Guid AlbumId, Guid UserId, string Username, string Title, string Body);
public record CreateBandReviewRequest(Guid BandId, Guid UserId, string Username, string Title, string Body);
