namespace Library.API.Endpoints.Countries;

public record CreateCountryRequest(string Name, string Code);

public record CreateCountryResponse(Guid Id);

public class CreateCountry : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/countries",
                async (CreateCountryRequest request, ISender sender) =>
                {
                    var command = request.Adapt<CreateCountryCommand>();

                    var result = await sender.Send(command);

                    var response = result.Adapt<CreateCountryResponse>();

                    return Results.Created($"/countries/{response.Id}", response);
                })
            .WithName("CreateCountry")
            .Produces<CreateCountryResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Country")
            .WithDescription("Create Country")
            .WithTags("Countries");
    }
}
