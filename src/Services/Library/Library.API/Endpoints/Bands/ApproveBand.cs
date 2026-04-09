namespace Library.API.Endpoints.Bands;

public record ApproveBandResponse(bool IsSuccess);

public class ApproveBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/bands/{id:guid}/approve", async (Guid id, ISender sender) =>
            {
                ApproveBandCommand command = new ApproveBandCommand(id);
                ApproveBandResult result = await sender.Send(command);
                ApproveBandResponse response = result.Adapt<ApproveBandResponse>();
                return Results.Ok(response);
            })
            .WithName("ApproveBand")
            .Produces<ApproveBandResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Approve Band")
            .WithDescription("Approve Band")
            .WithTags("Bands")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
