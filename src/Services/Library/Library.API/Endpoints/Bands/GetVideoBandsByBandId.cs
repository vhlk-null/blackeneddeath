namespace Library.API.Endpoints.Bands;

public record GetVideoBandsByBandIdResponse(IReadOnlyList<VideoBandDto> VideoBands);

public class GetVideoBandsByBandId : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/bands/{bandId:guid}/videos",
                async ([FromRoute] Guid bandId, ISender sender) =>
                {
                    var query = new GetVideoBandsByBandIdQuery(bandId);
                    var result = await sender.Send(query);
                    return Results.Ok(result.Adapt<GetVideoBandsByBandIdResponse>());
                })
            .WithName("GetVideoBandsByBandId")
            .Produces<GetVideoBandsByBandIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Videos By Band Id")
            .WithDescription("Get Videos By Band Id")
            .WithTags("Bands");
    }
}
