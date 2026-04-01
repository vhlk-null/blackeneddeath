namespace Library.API.Endpoints.Countries;

public record GetCountriesResult(IReadOnlyList<CountryDto> Countries);

public class GetCountries : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/countries", async (ISender sender) =>
            {
                var result = await sender.Send(new GetCountriesQuery());
                var response = result.Adapt<GetCountriesResult>();
                return Results.Ok(response);
            })
            .WithName("GetCountries")
            .Produces<GetCountriesResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Countries")
            .WithDescription("Get Countries")
            .WithTags("Countries");
    }
}
