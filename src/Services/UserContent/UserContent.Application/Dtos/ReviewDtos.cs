namespace UserContent.Application.Dtos;

public record ReviewDto(Guid Id, Guid UserId, string Username, string Title, string Body, int Grade, DateTime CreatedAt);

public record CreateAlbumReviewRequest(Guid AlbumId, Guid UserId, string Username, string Title, string Body, int Grade);
public record CreateBandReviewRequest(Guid BandId, Guid UserId, string Username, string Title, string Body, int Grade);
