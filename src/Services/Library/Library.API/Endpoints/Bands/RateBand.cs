namespace Library.API.Endpoints.Bands;

public record RateBandRequest(Guid UserId, int Rating);
public record RateBandResponse(Guid BandId, int UserRating, double? AverageRating, int RatingsCount);
public record GetBandRatingResponse(Guid BandId, int? UserRating, double? AverageRating, int RatingsCount);

public class RateBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/bands/{id:guid}/rating", async (Guid id, Guid userId, ISender sender) =>
            {
                GetBandRatingQuery query = new GetBandRatingQuery(userId, id);
                GetBandRatingResult result = await sender.Send(query);
                GetBandRatingResponse response = result.Adapt<GetBandRatingResponse>();
                return Results.Ok(response);
            })
            .WithName("GetBandRating")
            .Produces<GetBandRatingResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Band Rating")
            .WithDescription("Get the current user's rating and aggregate stats for a band")
            .WithTags("Bands");

        app.MapPost("/bands/{id:guid}/rating", async (Guid id, RateBandRequest request, ISender sender) =>
            {
                RateBandCommand command = new RateBandCommand(request.UserId, id, request.Rating);
                RateBandResult result = await sender.Send(command);
                RateBandResponse response = result.Adapt<RateBandResponse>();
                return Results.Ok(response);
            })
            .WithName("RateBand")
            .Produces<RateBandResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Rate a Band")
            .WithDescription("Submit or update a user rating for a band")
            .WithTags("Bands");
    }
}
