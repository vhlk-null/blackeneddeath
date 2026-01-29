using Microsoft.AspNetCore.Mvc;

namespace UserContent.API.UserContent.FavoriteAlbums.DeleteUserFavoriteAlbums
{
    public record DeleteFavoriteAlbumResponse(bool IsSuccess);

    public class DeleteUserFavoriteAlbumsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/favoriteAlbums/{albumId:guid}", async (Guid albumId, [FromQuery] Guid userId, ISender sender) =>
            {
                var command = new DeleteFavoriteAlbumCommand(albumId, userId);

                var result = await sender.Send(command);

                var response = result.Adapt<DeleteFavoriteAlbumResponse>();

                return Results.Ok(response);
            })
            .WithName("DeleteFavoriteAlbum")
            .Produces<DeleteFavoriteAlbumResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Favorite Album")
            .WithDescription("Remove an album from user's favorite albums");
        }
    }
}
