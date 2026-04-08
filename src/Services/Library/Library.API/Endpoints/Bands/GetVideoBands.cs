namespace Library.API.Endpoints.Bands;

public record GetVideoBandsResponse(PaginatedResult<VideoBandDto> VideoBands);

public class GetVideoBands : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/videos",
                async ([AsParameters] PaginationRequest paginationRequest, ISender sender) =>
                {
                    GetVideoBandsQuery query = new GetVideoBandsQuery(paginationRequest);
                    GetVideoBandsResult result = await sender.Send(query);
                    return Results.Ok(result.Adapt<GetVideoBandsResponse>());
                })
            .WithName("GetVideoBands")
            .Produces<GetVideoBandsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get All Videos")
            .WithDescription("Get All Videos with pagination")
            .WithTags("Videos");
    }
}
