namespace Library.API.Endpoints.Labels;

public record GetLabelByIdResponse(LabelDto Label);

public class GetLabelById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/labels/{id:guid}", async (Guid id, ISender sender) =>
            {
                GetLabelByIdQuery query = new GetLabelByIdQuery(id);
                GetLabelByIdResult result = await sender.Send(query);
                GetLabelByIdResponse response = result.Adapt<GetLabelByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("GetLabelById")
            .Produces<GetLabelByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Label By Id")
            .WithDescription("Get Label By Id")
            .WithTags("Labels");
    }
}
