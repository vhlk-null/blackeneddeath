namespace Library.API.Endpoints.Albums.Admin;

public record GetAllAlbumsResult(PaginatedResult<AlbumCardDto> Albums);

public class GetAllAlbums : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/admin/albums", async (
                [AsParameters] PaginationRequest paginationRequest,
                ISender sender,
                HttpContext httpContext,
                AlbumSortBy sortBy = AlbumSortBy.ReleaseDate) =>
            {
                SortDir sortDir = Enum.TryParse<SortDir>(httpContext.Request.Query["sortDir"], ignoreCase: true, out SortDir sd) ? sd : SortDir.Desc;

                Application.Services.Albums.Queries.GetAlbums.GetAlbumsResult result = await sender.Send(new GetAlbumsQuery(paginationRequest, sortBy, sortDir, ApprovedOnly: false));
                return Results.Ok(result.Adapt<GetAllAlbumsResult>());
            })
            .WithName("AdminGetAlbums")
            .Produces<GetAllAlbumsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Admin: Get All Albums")
            .WithDescription("Get all albums including unapproved")
            .WithTags("Admin")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
