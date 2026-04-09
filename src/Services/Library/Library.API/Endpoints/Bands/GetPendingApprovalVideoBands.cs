namespace Library.API.Endpoints.Bands;

public record GetPendingApprovalVideoBandsResponse(List<PendingApprovalDto> VideoBands);

public class GetPendingApprovalVideoBands : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/video-bands/pending-approval", async (ISender sender) =>
            {
                GetPendingApprovalVideoBandsResult result = await sender.Send(new GetPendingApprovalVideoBandsQuery());
                return Results.Ok(result.Adapt<GetPendingApprovalVideoBandsResponse>());
            })
            .WithName("GetPendingApprovalVideoBands")
            .Produces<GetPendingApprovalVideoBandsResponse>(StatusCodes.Status200OK)
            .WithSummary("Get Pending Approval Video Bands")
            .WithDescription("Returns video bands that have not yet been approved by admin")
            .WithTags("Bands")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
