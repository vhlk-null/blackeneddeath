using Library.Application.Services.Bands.Commands.UpdateBand;

namespace Library.API.Endpoints.Bands;

public record UpdateBandResponse(bool IsSuccess);

public class UpdateBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/bands",
                async (UpdateBandDto request, ISender sender) =>
                {
                    var command = new UpdateBandCommand(request);

                    var result = await sender.Send(command);

                    return Results.Ok(new UpdateBandResponse(result.IsSuccess));
                })
            .WithName("UpdateBand")
            .Produces<UpdateBandResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Band")
            .WithDescription("Update Band")
            .WithTags("Bands");
    }
}
