using Library.Application.Services.Labels.Commands.UpdateLabel;

namespace Library.API.Endpoints.Labels;

public record UpdateLabelRequest(string Name);

public record UpdateLabelResponse(bool IsSuccess);

public class UpdateLabel : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/labels/{id:guid}",
                async (Guid id, UpdateLabelRequest request, ISender sender) =>
                {
                    var command = request.Adapt<UpdateLabelCommand>() with { Id = id };

                    var result = await sender.Send(command);

                    return Results.Ok(result.Adapt<UpdateLabelResponse>());
                })
            .WithName("UpdateLabel")
            .Produces<UpdateLabelResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Label")
            .WithDescription("Update Label")
            .WithTags("Labels");
    }
}
