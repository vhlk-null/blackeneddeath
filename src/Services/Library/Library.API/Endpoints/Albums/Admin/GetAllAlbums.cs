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
                AlbumSortBy sortBy = AlbumSortBy.Newest,
                Guid? genreId = null,
                Guid? labelId = null,
                Guid? countryId = null,
                AlbumType? type = null,
                int? yearFrom = null,
                int? yearTo = null,
                string? name = null) =>
            {
                ISpecification<Album>? filter = AlbumFilterBuilder.Build(genreId, labelId, countryId, type, yearFrom, yearTo, name);
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
