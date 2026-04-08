namespace Library.API.Endpoints.Bands;

public record GetVideoBandsByBandIdResponse(PaginatedResult<VideoBandDto> VideoBands);

public class GetVideoBandsByBandId : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/bands/{bandId:guid}/videos",
                async ([FromRoute] Guid bandId, [AsParameters] PaginationRequest paginationRequest, ISender sender) =>
                {
                    GetVideoBandsByBandIdQuery query = new GetVideoBandsByBandIdQuery(bandId, paginationRequest);
                    GetVideoBandsByBandIdResult result = await sender.Send(query);
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
