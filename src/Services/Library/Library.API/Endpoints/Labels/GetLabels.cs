using Library.Application.Services.Labels.Queries.GetLabels;

namespace Library.API.Endpoints.Labels;

public record GetLabelsResult(IReadOnlyList<LabelDto> Labels);

public class GetLabels : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/labels", async (ISender sender) =>
            {
                var result = await sender.Send(new GetLabelsQuery());
                var response = result.Adapt<GetLabelsResult>();
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
