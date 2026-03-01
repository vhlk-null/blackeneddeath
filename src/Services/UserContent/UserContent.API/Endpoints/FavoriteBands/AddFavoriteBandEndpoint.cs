using UserContent.Application.UserContent.FavoriteBands.AddFavoriteBand;

namespace UserContent.API.Endpoints.FavoriteBands;

public record AddBandToFavoriteRequest(Guid BandId, Guid UserId);
public record AddBandToFavoriteResponse(Guid UserId);

public class StoreUserFavoriteBandsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/favoriteBands", async (AddBandToFavoriteRequest request, ISender sender) =>
        {
            var command = new AddBandToFavoriteCommand(request.BandId, request.UserId);

            var result = await sender.Send(command);

            var response = result.Adapt<AddBandToFavoriteResponse>();

            return Results.Created($"/favoriteBands/{response.UserId}", response);
        })
        .WithName("AddBandToFavorite")
        .Produces<AddBandToFavoriteResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Add Band To Favorite")
        .WithDescription("Add Band To Favorite");
    }
}
