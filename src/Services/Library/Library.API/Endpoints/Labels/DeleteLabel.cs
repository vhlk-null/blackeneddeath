namespace Library.API.Endpoints.Labels;

public record DeleteLabelResponse(bool IsSuccess);

public class DeleteLabel : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/labels/{id:guid}",
                async (Guid id, ISender sender) =>
                {
                    var command = new DeleteLabelCommand(id);

                    var result = await sender.Send(command);

                    return Results.Ok(result.Adapt<DeleteLabelResponse>());
                })
            .WithName("DeleteLabel")
            .Produces<DeleteLabelResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Label")
            .WithDescription("Delete Label")
            .WithTags("Labels");
    }
}
