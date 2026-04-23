namespace UserContent.Application.Dtos;

public record CommentDto(
    Guid Id,
    Guid UserId,
    string Username,
    string Body,
    Guid? ParentCommentId,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    List<CommentDto> Replies);

public record CreateAlbumCommentRequest(Guid AlbumId, Guid UserId, string Username, string Body, Guid? ParentCommentId);
public record CreateBandCommentRequest(Guid BandId, Guid UserId, string Username, string Body, Guid? ParentCommentId);
public record UpdateCommentRequest(string Body);
