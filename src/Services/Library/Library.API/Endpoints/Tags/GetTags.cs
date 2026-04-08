namespace Library.API.Endpoints.Tags;

public record GetTagsResponse(IReadOnlyList<TagDto> Tags);

public class GetTags : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tags", async (ISender sender) =>
            {
                GetTagsResult result = await sender.Send(new GetTagsQuery());
                GetTagsResponse response = result.Adapt<GetTagsResponse>();
                return Results.Ok(response);
            })
            .WithName("GetTags")
            .Produces<GetTagsResponse>(StatusCodes.Status200OK)
            .WithSummary("Get Tags")
            .WithDescription("Get Tags")
            .WithTags("Tags");
    }
}
