using Microsoft.AspNetCore.Mvc;
using UserContent.Application.UserContent.FavoriteAlbums.DeleteFavoriteAlbum;

namespace UserContent.API.Endpoints.FavoriteAlbums;

public record DeleteFavoriteAlbumResponse(bool IsSuccess);

public class DeleteUserFavoriteAlbumsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/favoriteAlbums", async ([FromQuery] Guid userId, [FromQuery] Guid albumId, ISender sender) =>
        {
            var command = new DeleteFavoriteAlbumCommand(userId, albumId);

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
