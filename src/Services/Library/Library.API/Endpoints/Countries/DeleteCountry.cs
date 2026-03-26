using Library.Application.Services.Countries.Commands.DeleteCountry;

namespace Library.API.Endpoints.Countries;

public record DeleteCountryResponse(bool IsSuccess);

public class DeleteCountry : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/countries/{id:guid}",
                async (Guid id, ISender sender) =>
                {
                    var command = new DeleteCountryCommand(id);

                    var result = await sender.Send(command);

                    return Results.Ok(result.Adapt<DeleteCountryResponse>());
                })
            .WithName("DeleteCountry")
            .Produces<DeleteCountryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Country")
            .WithDescription("Delete Country")
            .WithTags("Countries");
    }
}
