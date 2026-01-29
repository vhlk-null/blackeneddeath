using UserContent.API.UserContent.UserProfile.GetUserProfile;

namespace UserContent.API.UserContent.FavoriteAlbums.AddFavoriteAlbum
{
    public record AddAlbumToFavoriteRequest(Guid albumId, Guid userId);
    public record AddAlbumToFavoriteResponse(Guid userId);
    public class StoreUserFavoriteAlbumsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/favoriteAlbums", async (AddAlbumToFavoriteRequest request, ISender sender) =>
            {
                var command = request.Adapt<AddAlbumToFavoriteCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<AddAlbumToFavoriteResponse>();

                return Results.Created($"/favoriteAlbums/{response.userId}", response);

            })
            .WithName("AddAlbumToFavorite")
            .Produces<GetUserProfileResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Add Album To Favorite")
            .WithDescription("Add Album To Favorite");
        }
    }
}
