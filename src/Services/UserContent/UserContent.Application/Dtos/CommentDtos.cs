namespace UserContent.Application.Dtos;

public record CommentDto(
    Guid Id,
    Guid UserId,
    string Username,
    string Body,
    Guid? ParentCommentId,
    Guid? ReplyToCommentId,
    string? ReplyToUsername,
    int Likes,
    int Dislikes,
    bool? UserReaction,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    List<CommentDto> Replies);

public record ReactToCommentRequest(Guid UserId, bool IsLike);

public record CreateAlbumCommentRequest(Guid AlbumId, Guid UserId, string Username, string Body, Guid? ParentCommentId, Guid? ReplyToCommentId, string? ReplyToUsername);
public record CreateBandCommentRequest(Guid BandId, Guid UserId, string Username, string Body, Guid? ParentCommentId, Guid? ReplyToCommentId, string? ReplyToUsername);
public record UpdateCommentRequest(string Body);
