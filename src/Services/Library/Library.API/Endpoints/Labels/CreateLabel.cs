using Library.Application.Services.Labels.Commands.CreateLabel;

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
                    var command = request.Adapt<CreateLabelCommand>();

                    var result = await sender.Send(command);

                    var response = result.Adapt<CreateLabelResponse>();

                    return Results.Created($"/labels/{response.Id}", response);
                })
            .WithName("CreateLabel")
            .Produces<CreateLabelResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Label")
            .WithDescription("Create Label")
            .WithTags("Labels");
    }
}
