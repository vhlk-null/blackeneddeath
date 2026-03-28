namespace Library.API.Endpoints.Bands;

public record GetBandSummariesResponse(List<BandSummaryDto> Bands);

public class GetBandSummaries : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/bands/summaries", async (ISender sender) =>
            {
                var result = await sender.Send(new GetBandSummariesQuery());
                return Results.Ok(result.Adapt<GetBandSummariesResponse>());
            })
            .WithName("GetBandSummaries")
            .Produces<GetBandSummariesResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Band Summaries")
            .WithDescription("Returns all bands with only their Id, Name and Slug")
            .WithTags("Bands");
    }
}
