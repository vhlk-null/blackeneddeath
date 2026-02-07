namespace UserContent.API.UserContent.FavoriteBands.AddFavoriteBand
{
    public record AddBandToFavoriteRequest(Guid bandId, Guid userId);
    public record AddBandToFavoriteResponse(Guid userId);

    public class StoreUserFavoriteBandsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/favoriteBands", async (AddBandToFavoriteRequest request, ISender sender) =>
            {
                var command = new AddBandToFavoriteCommand(request.bandId, request.userId);

                var result = await sender.Send(command);

                var response = result.Adapt<AddBandToFavoriteResponse>();

                return Results.Created($"/favoriteBands/{response.userId}", response);

            })
            .WithName("AddBandToFavorite")
            .Produces<AddBandToFavoriteResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Add Band To Favorite")
            .WithDescription("Add Band To Favorite");
        }
    }
}
