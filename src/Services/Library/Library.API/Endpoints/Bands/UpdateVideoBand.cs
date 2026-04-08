namespace Library.API.Endpoints.Bands;

public record UpdateVideoBandResponse(bool IsSuccess);

public class UpdateVideoBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/bands/{bandId:guid}/videos/{id:guid}",
                async ([FromRoute] Guid bandId, [FromRoute] Guid id, [FromBody] UpdateVideoBandDto body, ISender sender) =>
                {
                    UpdateVideoBandDto dto = body with { Id = id, BandId = bandId };
                    UpdateVideoBandCommand command = new UpdateVideoBandCommand(dto);
                    UpdateVideoBandResult result = await sender.Send(command);
                    return Results.Ok(result.Adapt<UpdateVideoBandResponse>());
                })
            .WithName("UpdateVideoBand")
            .Produces<UpdateVideoBandResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Video Band")
            .WithDescription("Update Video Band")
            .WithTags("Bands");
    }
}
