namespace Library.API.Endpoints.Tags;

public record DeleteTagResponse(bool IsSuccess);

public class DeleteTag : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/tags/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteTagCommand(id));
                var response = result.Adapt<DeleteTagResponse>();
                return Results.Ok(response);
            })
            .WithName("DeleteTag")
            .Produces<DeleteTagResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Tag")
            .WithDescription("Delete Tag")
            .WithTags("Tags");
    }
}
