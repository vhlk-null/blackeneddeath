namespace Library.API.Endpoints.Tags;

public record GetTagByIdResponse(TagDto Tag);

public class GetTagById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tags/{id:guid}", async (Guid id, ISender sender) =>
            {
                GetTagByIdResult result = await sender.Send(new GetTagByIdQuery(id));
                GetTagByIdResponse response = result.Adapt<GetTagByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("GetTagById")
            .Produces<GetTagByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Tag By Id")
            .WithDescription("Get Tag By Id")
            .WithTags("Tags");
    }
}
