using Library.Application.Bands.Commands.CreateBand;

namespace Library.API.Endpoints.Bands;

public record CreateBandRequest(BandDto Band);

public record CreateBandResponse(Guid Id);

public class CreateBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/bands",
                async (CreateBandRequest request, ISender sender) =>
                {
                    var command = request.Adapt<CreateBandCommand>();

                    var result = await sender.Send(command);

                    var response = result.Adapt<CreateBandResponse>();

                    return Results.Created($"/bands/{response.Id}", response);
                })
            .WithName("CreateBand")
            .Produces<CreateBandResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Band")
            .WithDescription("Create Band");
    }
}