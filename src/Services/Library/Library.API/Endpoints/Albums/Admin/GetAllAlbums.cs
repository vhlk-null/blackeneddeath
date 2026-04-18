using BuildingBlocks.Specifications;

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
                AlbumSortBy sortBy = AlbumSortBy.Newest,
                AlbumType? type = null,
                int? yearFrom = null,
                int? yearTo = null,
                string? name = null) =>
            {
                List<Guid> genreIds   = httpContext.Request.Query["genreId"].Select(s => Guid.TryParse(s, out Guid g) ? (Guid?)g : null).Where(g => g.HasValue).Select(g => g!.Value).ToList();
                List<Guid> labelIds   = httpContext.Request.Query["labelId"].Select(s => Guid.TryParse(s, out Guid g) ? (Guid?)g : null).Where(g => g.HasValue).Select(g => g!.Value).ToList();
                List<Guid> countryIds = httpContext.Request.Query["countryId"].Select(s => Guid.TryParse(s, out Guid g) ? (Guid?)g : null).Where(g => g.HasValue).Select(g => g!.Value).ToList();
                ISpecification<Album>? filter = AlbumFilterBuilder.Build(genreIds, labelIds, countryIds, type, yearFrom, yearTo, name);
                Application.Services.Albums.Queries.GetAlbums.GetAlbumsResult result = await sender.Send(new GetAlbumsQuery(paginationRequest, sortBy, filter, ApprovedOnly: false));
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
