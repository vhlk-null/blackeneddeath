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
                    CreateCountryCommand command = request.Adapt<CreateCountryCommand>();

                    CreateCountryResult result = await sender.Send(command);

                    CreateCountryResponse response = result.Adapt<CreateCountryResponse>();

                    return Results.Created($"/countries/{response.Id}", response);
                })
            .WithName("CreateCountry")
            .Produces<CreateCountryResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Country")
            .WithDescription("Create Country")
            .WithTags("Countries")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly"); ;
    }
}
