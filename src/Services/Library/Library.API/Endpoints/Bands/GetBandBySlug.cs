namespace Library.API.Endpoints.Bands;

public record GetBandBySlugResponse(BandDetailDto Band);

public class GetBandBySlug : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/bands/slug/{slug}", async (string slug, ISender sender) =>
        {
            GetBandBySlugQuery query = new GetBandBySlugQuery(slug);
            GetBandBySlugResult result = await sender.Send(query);
            GetBandBySlugResponse response = result.Adapt<GetBandBySlugResponse>();
            return Results.Ok(response);
        })
        .WithName("GetBandBySlug")
        .Produces<GetBandBySlugResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Band by Slug")
        .WithDescription("Get Band by Slug")
        .WithTags("Bands");
    }
}
