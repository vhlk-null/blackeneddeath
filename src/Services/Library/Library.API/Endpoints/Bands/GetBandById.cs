namespace Library.API.Endpoints.Bands;

public record GetBandByIdResponse(BandDetailDto Band);

public class GetBandById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/bands/{id:guid}", async (Guid id, ISender sender) =>
        {
            GetBandByIdQuery query = new GetBandByIdQuery(id);
            GetBandByIdResult result = await sender.Send(query);
            GetBandByIdResponse response = result.Adapt<GetBandByIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetBandById")
        .Produces<GetBandByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Band By Id")
        .WithDescription("Get Band By Id")
        .WithTags("Bands");
    }
}
