namespace Library.API.Endpoints.Bands;

public record RateBandRequest(Guid UserId, int Rating);
public record RateBandResponse(Guid BandId, int UserRating, double? AverageRating, int RatingsCount);
public record GetBandRatingResponse(Guid BandId, int? UserRating, double? AverageRating, int RatingsCount);

public class BandRatings : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/bands/{bandId:guid}/rating", async (
                Guid bandId,
                [FromQuery] Guid? userId,
                ISender sender,
                CancellationToken ct) =>
            {
                GetBandRatingResult result = await sender.Send(new GetBandRatingQuery(bandId, userId), ct);
                return Results.Ok(result.Adapt<GetBandRatingResponse>());
            })
            .WithName("GetBandRating")
            .Produces<GetBandRatingResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Band Rating")
            .WithTags("Band Ratings");

        app.MapPost("/bands/{bandId:guid}/rating", async (
                Guid bandId,
                RateBandRequest request,
                ISender sender,
                CancellationToken ct) =>
            {
                RateBandResult result = await sender.Send(new RateBandCommand(request.UserId, bandId, request.Rating), ct);
                return Results.Ok(result.Adapt<RateBandResponse>());
            })
            .WithName("RateBand")
            .Produces<RateBandResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Rate Band")
            .WithTags("Band Ratings");
    }
}
