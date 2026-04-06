namespace Library.API.Endpoints.Albums;

public record GetAlbumsResult(PaginatedResult<AlbumCardDto> Albums);
public class GetAlbums : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums", async (
                [AsParameters] PaginationRequest paginationRequest,
                ISender sender,
                AlbumSortBy sortBy = AlbumSortBy.Newest,
                Guid? genreId = null,
                Guid? labelId = null,
                Guid? countryId = null,
                AlbumType? type = null,
                int? year = null) =>
            {
                var filter = AlbumFilterBuilder.Build(genreId, labelId, countryId, type, year);
                var result = await sender.Send(new GetAlbumsQuery(paginationRequest, sortBy, filter));
                return Results.Ok(result.Adapt<GetAlbumsResult>());
            })
            .WithName("GetAlbums")
            .Produces<GetAlbumsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Albums")
            .WithDescription("Get Albums with optional filters: genreId, labelId, countryId, type, year")
            .WithTags("Albums")
            //.ProducesProblem(StatusCodes.Status401Unauthorized)
            //.ProducesProblem(StatusCodes.Status403Forbidden)
            //.RequireAuthorization("ClientIdPolicy")
            ;
    }
}
