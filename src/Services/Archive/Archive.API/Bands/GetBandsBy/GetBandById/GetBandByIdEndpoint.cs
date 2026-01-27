using Archive.API.Bands.GetBands;

namespace Archive.API.Bands.GetBandsBy.GetBandById;

public record GetBandByIdResponse(BandDto Band);

public class GetBandByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/bands/{id}", async (Guid id, ISender sender) =>
        {
            var query = new GetBandByIdQuery(id);
            var result = await sender.Send(query);
            var response = result.Adapt<GetBandByIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetBandById")
        .Produces<GetBandByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Band By Id")
        .WithDescription("Get Band By Id");
    }
}
