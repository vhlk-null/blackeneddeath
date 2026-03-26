namespace Library.API.Endpoints.Albums;

//public record GetAlbumsByYearRequest(int ReleaseDate) : IRequest<GetAlbumsByYearResult>;

public record GetAlbumsByYearResponse(IEnumerable<Album> Albums);

public class GetAlbumsByYear : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums/releaseyear/{releaseDate:int}", async (int releaseDate, ISender sender) =>
        {
            var query = new GetAlbumsByYearQuery(releaseDate);
            var result = await sender.Send(query);
            var response = result.Adapt<GetAlbumsByYearResponse>();
            return Results.Ok(response);
        })
        .WithName("GetAlbumByReleaseYear")
        .Produces<GetAlbumsByYearResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Album by Release year")
        .WithDescription("Get Album by Release year")
        .WithTags("Albums");
    }
}