namespace Library.API.Endpoints.Albums;

public record GetAlbumsResult(PaginatedResult<AlbumCardDto> Albums);
public class GetAlbums : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums", async (
                [AsParameters] PaginationRequest paginationRequest,
                ISender sender,
                HttpContext httpContext,
                AlbumSortBy sortBy = AlbumSortBy.ReleaseDate) =>
            {
                SortDir sortDir = Enum.TryParse<SortDir>(httpContext.Request.Query["sortDir"], ignoreCase: true, out SortDir sd) ? sd : SortDir.Desc;

                Application.Services.Albums.Queries.GetAlbums.GetAlbumsResult result = await sender.Send(new GetAlbumsQuery(paginationRequest, sortBy, sortDir));
                return Results.Ok(result.Adapt<GetAlbumsResult>());
            })
            .WithName("GetAlbums")
            .Produces<GetAlbumsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Albums")
            .WithDescription("Get Albums")
            .WithTags("Albums");
    }
}
