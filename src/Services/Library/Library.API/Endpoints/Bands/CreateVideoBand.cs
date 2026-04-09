namespace Library.API.Endpoints.Bands;

public record CreateVideoBandRequest(CreateVideoBandDto VideoBand);
public record CreateVideoBandResponse(Guid Id);

public class CreateVideoBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/bands/{bandId:guid}/videos",
                async ([FromRoute] Guid bandId, [FromBody] CreateVideoBandDto body, ISender sender) =>
                {
                    CreateVideoBandDto dto = body with { BandId = bandId };
                    CreateVideoBandCommand command = new CreateVideoBandCommand(dto);
                    CreateVideoBandResult result = await sender.Send(command);
                    return Results.Created($"/bands/{bandId}/videos/{result.Id}", result.Adapt<CreateVideoBandResponse>());
                })
            .WithName("CreateVideoBand")
            .Produces<CreateVideoBandResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Create Video Band")
            .WithDescription("Create Video Band")
            .WithTags("Bands")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization();
    }
}
