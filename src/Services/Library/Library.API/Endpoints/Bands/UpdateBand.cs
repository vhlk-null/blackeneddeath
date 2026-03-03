using Library.Application.Bands.Commands.UpdateBand;

namespace Library.API.Endpoints.Bands;

public record UpdateBandRequest(BandDto Band);

public record UpdateBandResponse(bool IsSuccess);

public class UpdateBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/bands/{id}",
                async (Guid id, UpdateBandRequest request, ISender sender) =>
                {
                    var command = request.Adapt<UpdateBandCommand>();

                    var result = await sender.Send(command);

                    return Results.Ok(result.Adapt<UpdateBandResponse>());
                })
            .WithName("UpdateBand")
            .Produces<UpdateBandResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Band")
            .WithDescription("Update Band");
    }
}