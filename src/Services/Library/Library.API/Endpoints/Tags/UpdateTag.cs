namespace Library.API.Endpoints.Tags;

public record UpdateTagRequest(string Name);

public record UpdateTagResponse(bool IsSuccess);

public class UpdateTag : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/tags/{id:guid}", async (Guid id, UpdateTagRequest request, ISender sender) =>
            {
                var command = new UpdateTagCommand(id, request.Name);
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateTagResponse>();
                return Results.Ok(response);
            })
            .WithName("UpdateTag")
            .Produces<UpdateTagResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Tag")
            .WithDescription("Update Tag")
            .WithTags("Tags");
    }
}
