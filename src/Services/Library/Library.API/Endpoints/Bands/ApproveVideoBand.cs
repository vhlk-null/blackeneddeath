namespace Library.API.Endpoints.Bands;

public record ApproveVideoBandResponse(bool IsSuccess);

public class ApproveVideoBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/video-bands/{id:guid}/approve", async (Guid id, ISender sender) =>
            {
                ApproveVideoBandCommand command = new ApproveVideoBandCommand(id);
                ApproveVideoBandResult result = await sender.Send(command);
                ApproveVideoBandResponse response = result.Adapt<ApproveVideoBandResponse>();
                return Results.Ok(response);
            })
            .WithName("ApproveVideoBand")
            .Produces<ApproveVideoBandResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Approve Video Band")
            .WithDescription("Approve Video Band")
            .WithTags("Bands")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
