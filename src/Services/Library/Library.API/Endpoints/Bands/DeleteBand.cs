using Library.Application.Bands.Commands.DeleteBand;

namespace Library.API.Endpoints.Bands;

public record DeleteBandResponse(bool IsSuccess);

public class DeleteBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/bands/{id}",
                async (Guid id, ISender sender) =>
                {
                    var command = new DeleteBandCommand(id);

                    var result = await sender.Send(command);

                    return Results.Ok(result.Adapt<DeleteBandResponse>());
                })
            .WithName("DeleteBand")
            .Produces<DeleteBandResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Band")
            .WithDescription("Delete Band");
    }
}