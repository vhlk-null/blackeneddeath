namespace UserContent.API.UserContent.UserProfile
{
    public record GetUserProfileQuery(Guid UserId) : IQuery<GetUserProfileResponse>;

    public record GetUserProfileResponse(
    Guid UserId,
    string Username,
    string? AvatarUrl,
    DateTime RegisteredDate,
    string? Bio,
    string? Location,
    int FriendsCount,
    List<FavoriteAlbum> FavoriteAlbums,
    List<FavoriteBand> FavoriteBands);
    public class GetUserProfileHandler
    {
    }
}
