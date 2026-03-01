using Microsoft.AspNetCore.Mvc;
using UserContent.Application.UserContent.FavoriteBands.DeleteFavoriteBand;

namespace UserContent.API.Endpoints.FavoriteBands;

public record DeleteFavoriteBandResponse(bool IsSuccess);

public class DeleteUserFavoriteBandsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/favoriteBands", async ([FromQuery] Guid userId, [FromQuery] Guid bandId, ISender sender) =>
        {
            var command = new DeleteFavoriteBandCommand(userId, bandId);

            var result = await sender.Send(command);

            var response = result.Adapt<DeleteFavoriteBandResponse>();

            return Results.Ok(response);
        })
        .WithName("DeleteFavoriteBand")
        .Produces<DeleteFavoriteBandResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Favorite Band")
        .WithDescription("Remove a band from user's favorite bands");
    }
}
