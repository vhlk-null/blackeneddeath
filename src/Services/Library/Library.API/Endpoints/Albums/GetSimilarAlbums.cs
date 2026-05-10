namespace Library.API.Endpoints.Albums;

public record GetSimilarAlbumsResponse(PaginatedResult<AlbumSummaryDto> SimilarAlbums);

public class GetSimilarAlbums : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums/slug/{slug}/similar", async (string slug, ISender sender, int pageNumber = 1, int pageSize = 4) =>
            {
                GetSimilarAlbumsQuery query = new(slug, PageNumber: pageNumber, PageSize: pageSize);
                GetSimilarAlbumsResult result = await sender.Send(query);
                return Results.Ok(new GetSimilarAlbumsResponse(result.SimilarAlbums));
            })
            .WithName("GetSimilarAlbums")
            .Produces<GetSimilarAlbumsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags("Albums");
    }
}
