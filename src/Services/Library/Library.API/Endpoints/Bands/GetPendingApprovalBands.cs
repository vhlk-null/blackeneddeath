namespace Library.API.Endpoints.Bands;

public record GetPendingApprovalBandsResponse(List<PendingApprovalDto> Bands);

public class GetPendingApprovalBands : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/bands/pending-approval", async (ISender sender) =>
            {
                GetPendingApprovalBandsResult result = await sender.Send(new GetPendingApprovalBandsQuery());
                return Results.Ok(result.Adapt<GetPendingApprovalBandsResponse>());
            })
            .WithName("GetPendingApprovalBands")
            .Produces<GetPendingApprovalBandsResponse>(StatusCodes.Status200OK)
            .WithSummary("Get Pending Approval Bands")
            .WithDescription("Returns bands that have not yet been approved by admin")
            .WithTags("Bands")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
