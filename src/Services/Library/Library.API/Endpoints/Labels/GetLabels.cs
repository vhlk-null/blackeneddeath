namespace Library.API.Endpoints.Labels;

public record GetLabelsResult(IReadOnlyList<LabelDto> Labels);

public class GetLabels : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/labels", async (ISender sender) =>
            {
                Application.Services.Labels.Queries.GetLabels.GetLabelsResult result = await sender.Send(new GetLabelsQuery());
                GetLabelsResult response = result.Adapt<GetLabelsResult>();
                return Results.Ok(response);
            })
            .WithName("GetLabels")
            .Produces<GetLabelsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Labels")
            .WithDescription("Get Labels")
            .WithTags("Labels");
    }
}
