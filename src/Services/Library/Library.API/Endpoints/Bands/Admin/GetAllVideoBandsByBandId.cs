namespace Library.API.Endpoints.Bands.Admin;

public record GetAllVideoBandsByBandIdResponse(PaginatedResult<VideoBandDto> VideoBands);

public class GetAllVideoBandsByBandId : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/admin/bands/{bandId:guid}/videos", async (
                [FromRoute] Guid bandId,
                [AsParameters] PaginationRequest paginationRequest,
                ISender sender) =>
            {
                GetVideoBandsByBandIdResult result = await sender.Send(new GetVideoBandsByBandIdQuery(bandId, paginationRequest, ApprovedOnly: false));
                return Results.Ok(result.Adapt<GetAllVideoBandsByBandIdResponse>());
            })
            .WithName("AdminGetVideoBandsByBandId")
            .Produces<GetAllVideoBandsByBandIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Admin: Get Videos by Band Id")
            .WithDescription("Get all videos by band Id including unapproved")
            .WithTags("Admin")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
