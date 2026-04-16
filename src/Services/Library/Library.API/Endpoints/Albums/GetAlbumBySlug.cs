namespace Library.API.Endpoints.Albums;

public record GetAlbumBySlugResponse(AlbumDetailDto Album);

public class GetAlbumBySlug : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums/slug/{slug}", async (string slug, ISender sender, int similarPageNumber = 1, int similarPageSize = 4) =>
            {
                GetAlbumBySlugQuery query = new GetAlbumBySlugQuery(slug, SimilarPageNumber: similarPageNumber, SimilarPageSize: similarPageSize);
                GetAlbumBySlugResult result = await sender.Send(query);
                GetAlbumBySlugResponse response = result.Adapt<GetAlbumBySlugResponse>();
                return Results.Ok(response);
            })
            .WithName("GetAlbumBySlug")
            .Produces<GetAlbumBySlugResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Album by Slug")
            .WithDescription("Get Album by Slug")
            .WithTags("Albums");
    }
}
