namespace Library.API.Endpoints.Albums;

public record ApproveAlbumResponse(bool IsSuccess);

public class ApproveAlbum : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/albums/{id:guid}/approve", async (Guid id, ISender sender) =>
            {
                ApproveAlbumCommand command = new ApproveAlbumCommand(id);
                ApproveAlbumResult result = await sender.Send(command);
                ApproveAlbumResponse response = result.Adapt<ApproveAlbumResponse>();
                return Results.Ok(response);
            })
            .WithName("ApproveAlbum")
            .Produces<ApproveAlbumResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Approve Album")
            .WithDescription("Approve Album")
            .WithTags("Albums")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
