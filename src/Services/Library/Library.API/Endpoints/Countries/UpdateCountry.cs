namespace Library.API.Endpoints.Countries;

public record UpdateCountryRequest(string Name, string Code);

public record UpdateCountryResponse(bool IsSuccess);

public class UpdateCountry : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/countries/{id:guid}",
                async (Guid id, UpdateCountryRequest request, ISender sender) =>
                {
                    var command = request.Adapt<UpdateCountryCommand>() with { Id = id };

                    var result = await sender.Send(command);

                    return Results.Ok(result.Adapt<UpdateCountryResponse>());
                })
            .WithName("UpdateCountry")
            .Produces<UpdateCountryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Country")
            .WithDescription("Update Country")
            .WithTags("Countries");
    }
}
