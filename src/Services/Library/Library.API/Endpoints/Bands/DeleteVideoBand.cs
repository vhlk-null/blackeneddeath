namespace Library.API.Endpoints.Bands;

public record DeleteVideoBandResponse(bool IsSuccess);

public class DeleteVideoBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/bands/{bandId:guid}/videos/{id:guid}",
                async ([FromRoute] Guid bandId, [FromRoute] Guid id, ISender sender) =>
                {
                    DeleteVideoBandCommand command = new DeleteVideoBandCommand(id);
                    DeleteVideoBandResult result = await sender.Send(command);
                    return Results.Ok(result.Adapt<DeleteVideoBandResponse>());
                })
            .WithName("DeleteVideoBand")
            .Produces<DeleteVideoBandResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Video Band")
            .WithDescription("Delete Video Band")
            .WithTags("Bands");
    }
}
