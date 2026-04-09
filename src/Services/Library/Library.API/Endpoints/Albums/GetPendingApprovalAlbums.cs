namespace Library.API.Endpoints.Albums;

public record GetPendingApprovalAlbumsResponse(List<PendingApprovalDto> Albums);

public class GetPendingApprovalAlbums : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums/pending-approval", async (ISender sender) =>
            {
                GetPendingApprovalAlbumsResult result = await sender.Send(new GetPendingApprovalAlbumsQuery());
                return Results.Ok(result.Adapt<GetPendingApprovalAlbumsResponse>());
            })
            .WithName("GetPendingApprovalAlbums")
            .Produces<GetPendingApprovalAlbumsResponse>(StatusCodes.Status200OK)
            .WithSummary("Get Pending Approval Albums")
            .WithDescription("Returns albums that have not yet been approved by admin")
            .WithTags("Albums")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
