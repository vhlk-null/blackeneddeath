namespace Library.API.Endpoints.Labels;

public record CreateLabelRequest(string Name);

public record CreateLabelResponse(Guid Id);

public class CreateLabel : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/labels",
                async (CreateLabelRequest request, ISender sender) =>
                {
                    CreateLabelCommand command = request.Adapt<CreateLabelCommand>();

                    CreateLabelResult result = await sender.Send(command);

                    CreateLabelResponse response = result.Adapt<CreateLabelResponse>();

                    return Results.Created($"/labels/{response.Id}", response);
                })
            .WithName("CreateLabel")
            .Produces<CreateLabelResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Label")
            .WithDescription("Create Label")
            .WithTags("Labels")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
