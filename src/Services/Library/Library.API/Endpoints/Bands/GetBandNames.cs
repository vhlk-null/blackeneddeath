namespace Library.API.Endpoints.Bands;

public record GetBandNamesResponse(List<NameIdDto> Bands);

public class GetBandNames : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/bands/names", async (ISender sender) =>
            {
                GetBandNamesResult result = await sender.Send(new GetBandNamesQuery());
                return Results.Ok(result.Adapt<GetBandNamesResponse>());
            })
            .WithName("GetBandNames")
            .Produces<GetBandNamesResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Band Names")
            .WithDescription("Returns all bands with only their Id and Name")
            .WithTags("Bands");
    }
}
