namespace Library.API.Endpoints.Tags;

public record CreateTagRequest(string Name);

public record CreateTagResponse(Guid Id);

public class CreateTag : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/tags", async (CreateTagRequest request, ISender sender) =>
            {
                CreateTagCommand command = request.Adapt<CreateTagCommand>();
                CreateTagResult result = await sender.Send(command);
                CreateTagResponse response = result.Adapt<CreateTagResponse>();
                return Results.Created($"/tags/{response.Id}", response);
            })
            .WithName("CreateTag")
            .Produces<CreateTagResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Tag")
            .WithDescription("Create Tag")
            .WithTags("Tags");
    }
}
