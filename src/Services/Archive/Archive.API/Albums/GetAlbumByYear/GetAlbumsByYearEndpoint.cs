namespace Archive.API.Albums.GetAlbumByYear;

public record GetAlbumsByYearRequest(int ReleaseDate) : IRequest<GetAlbumsByYearResult>;

public record GetAlbumsByYearResponse(IEnumerable<Album> Albums);

public class GetAlbumsByYearEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums/{releaseDate:int}", async (int ReleaseDate, ISender sender) =>
        {
            var query = new GetAlbumsByYearQuery(ReleaseDate);
            var result = await sender.Send(query);
            var response = result.Adapt<GetAlbumsByYearResponse>();
            return Results.Ok(response);
        })
        .WithName("GetAlbumByReleaseYear")
        .Produces<GetAlbumsByYearResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Album by Release year")
        .WithDescription("Get Album by Release year");
    }
}