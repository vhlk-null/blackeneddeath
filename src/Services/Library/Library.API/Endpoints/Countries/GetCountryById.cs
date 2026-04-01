namespace Library.API.Endpoints.Countries;

public record GetCountryByIdResponse(CountryDto Country);

public class GetCountryById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/countries/{id:guid}", async (Guid id, ISender sender) =>
            {
                var query = new GetCountryByIdQuery(id);
                var result = await sender.Send(query);
                var response = result.Adapt<GetCountryByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("GetCountryById")
            .Produces<GetCountryByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Country By Id")
            .WithDescription("Get Country By Id")
            .WithTags("Countries");
    }
}
