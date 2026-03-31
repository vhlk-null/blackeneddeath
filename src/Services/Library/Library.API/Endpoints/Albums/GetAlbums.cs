using BuildingBlocks.Specifications;
using Library.Application.Services.Albums.Specifications;

namespace Library.API.Endpoints.Albums;

public record GetAlbumsResult(PaginatedResult<AlbumDto> Albums);

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
                ISpecification<Album>? filter = null;

                if (genreId.HasValue)   filter = Combine(filter, new AlbumByGenreSpec(genreId.Value));
                if (labelId.HasValue)   filter = Combine(filter, new AlbumByLabelSpec(labelId.Value));
                if (countryId.HasValue) filter = Combine(filter, new AlbumByCountrySpec(countryId.Value));
                if (type.HasValue)      filter = Combine(filter, new AlbumByTypeSpec(type.Value));
                if (year.HasValue)      filter = Combine(filter, new AlbumByYearSpec(year.Value));

                var result = await sender.Send(new GetAlbumsQuery(paginationRequest, sortBy, filter));
                return Results.Ok(result.Adapt<GetAlbumsResult>());
            })
            .WithName("GetAlbums")
            .Produces<GetAlbumsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Albums")
            .WithDescription("Get Albums with optional filters: genreId, labelId, countryId, type, year")
            .WithTags("Albums");
    }

    private static ISpecification<Album> Combine(ISpecification<Album>? current, ISpecification<Album> next) =>
        current is null ? next : current.And(next);
}
