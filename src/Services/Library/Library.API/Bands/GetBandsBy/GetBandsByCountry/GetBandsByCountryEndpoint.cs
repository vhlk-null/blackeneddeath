using Library.API.Bands.GetBands;

namespace Library.API.Bands.GetBandsBy.GetBandsByCountry
{
    public record GetBandsByCountryResponse(IEnumerable<BandDto> Bands);

    public class GetBandsByCountryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/bands/country/{countryId}", async (Guid countryId, ISender sender) =>
            {
                var query = new GetBandsByCountryQuery(countryId);
                var result = await sender.Send(query);
                return Results.Ok(result.Adapt<GetBandsByCountryResponse>());
            })
            .WithName("GetBandsByCountry")
            .Produces<GetBandsByCountryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Bands By Country")
            .WithDescription("Get Bands By Country");
        }
    }
}
