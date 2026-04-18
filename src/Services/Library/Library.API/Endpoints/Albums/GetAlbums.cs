using BuildingBlocks.Specifications;

namespace Library.API.Endpoints.Albums;

public record GetAlbumsResult(PaginatedResult<AlbumCardDto> Albums);
public class GetAlbums : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums", async (
                [AsParameters] PaginationRequest paginationRequest,
                ISender sender,
                ILibraryDbContext db,
                CancellationToken ct,
                AlbumSortBy sortBy = AlbumSortBy.Newest,
                Guid? genreId = null,
                Guid? labelId = null,
                Guid? countryId = null,
                AlbumType? type = null,
                int? yearFrom = null,
                int? yearTo = null,
                string? name = null,
                string? genreName = null,
                string? labelName = null,
                string? countryName = null) =>
            {
                ISpecification<Album>? filter;
                if (genreName != null || labelName != null || countryName != null)
                    filter = await AlbumFilterBuilder.BuildByNameAsync(db, genreName, labelName, countryName, type, yearFrom, yearTo, name, ct);
                else
                    filter = AlbumFilterBuilder.Build(genreId, labelId, countryId, type, yearFrom, yearTo, name);

                Application.Services.Albums.Queries.GetAlbums.GetAlbumsResult result = await sender.Send(new GetAlbumsQuery(paginationRequest, sortBy, filter));
                return Results.Ok(result.Adapt<GetAlbumsResult>());
            })
            .WithName("GetAlbums")
            .Produces<GetAlbumsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Albums")
            .WithDescription("Get Albums with optional filters: genreId, labelId, countryId, type, yearFrom, yearTo, genreName, labelName, countryName")
            .WithTags("Albums");
    }
}
