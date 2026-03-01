namespace UserContent.Application.UserContent.UserProfile.GetUserProfile;

public record GetUserProfileQuery(Guid UserId) : IQuery<GetUserProfileResult>;
public record GetUserProfileResult(UserProfileDto UserProfile);

public class GetUserProfileQueryValidator : AbstractValidator<GetUserProfileQuery>
{
    public GetUserProfileQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
    }
}

public record FavoriteAlbumDto(
    Guid AlbumId,
    string AlbumTitle,
    string? CoverUrl,
    int ReleaseDate,
    DateTime AddedDate,
    int? UserRating,
    string? UserReview);

public record FavoriteBandDto(
    Guid BandId,
    string BandName,
    string? LogoUrl,
    int ReleaseDate,
    DateTime AddedDate,
    bool IsFollowing);

public record UserProfileDto(
    Guid UserId,
    string Username,
    string Email,
    string? AvatarUrl,
    DateTime RegisteredDate,
    DateTime? LastLoginDate,
    string? Bio,
    int FavoriteBandsCount,
    int FavoriteAlbumsCount,
    int ReviewsCount,
    List<FavoriteAlbumDto> FavoriteAlbums,
    List<FavoriteBandDto> FavoriteBands);
