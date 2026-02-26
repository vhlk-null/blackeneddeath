using Library.Domain.Enums;

namespace Library.API.Bands.CreateBand;

public record CreateBandRequest(
    string Name,
    string? Bio,
    Guid? CountryId,
    int? FormedYear,
    int? DisbandedYear,
    BandStatus Status,
    List<Guid> GenreIds);

public record CreateBandResponse(Guid Id);

public class CreateBandEndpoint : ICarterModule
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