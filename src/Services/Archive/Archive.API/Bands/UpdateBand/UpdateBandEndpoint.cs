using BuildingBlocks.Models;

namespace Archive.API.Bands.UpdateBand
{
    public record UpdateBandRequest(
        string Name,
        string? Bio,
        Guid? CountryId,
        int? FormedYear,
        int? DisbandedYear,
        BandStatus Status,
        List<Guid> GenreIds);

    public record UpdateBandResponse(bool IsSuccess);

    public class UpdateBandEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/bands/{id}",
                async (Guid id, UpdateBandRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateBandCommand>() with { Id = id };

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
}
