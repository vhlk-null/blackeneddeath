namespace Library.API.Endpoints.Bands.Admin;

public record GetAllVideoBandsResponse(PaginatedResult<VideoBandDto> VideoBands);

public class GetAllVideoBands : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/admin/videos", async (
                [AsParameters] PaginationRequest paginationRequest,
                ISender sender) =>
            {
                GetVideoBandsResult result = await sender.Send(new GetVideoBandsQuery(paginationRequest, ApprovedOnly: false));
                return Results.Ok(result.Adapt<GetAllVideoBandsResponse>());
            })
            .WithName("AdminGetVideoBands")
            .Produces<GetAllVideoBandsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Admin: Get All Videos")
            .WithDescription("Get all videos including unapproved")
            .WithTags("Admin")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
