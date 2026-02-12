namespace UserContent.API.UserContent.UserProfile.GetUserProfile
{
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

    public record GetUserProfileResponse(UserProfileDto UserProfile);

    public class GetUserProfileEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/profile/{userId:guid}",
                    async (Guid userId, ISender sender) =>
                    {
                        var result = await sender.Send(new GetUserProfileQuery(userId));

                        return Results.Ok(result);
                    })
                .WithName("GetUserProfile")
                .Produces<GetUserProfileResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Get UserProfile")
                .WithDescription("Get UserProfile");
        }
    }
}