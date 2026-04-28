namespace Library.API.Endpoints.Albums;

public record RateAlbumRequest(Guid UserId, int Rating);
public record RateAlbumResponse(Guid AlbumId, int UserRating, double? AverageRating, int RatingsCount);
public record GetAlbumRatingResponse(Guid AlbumId, int? UserRating, double? AverageRating, int RatingsCount);

public class AlbumRatings : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums/{albumId:guid}/rating", async (
                Guid albumId,
                [FromQuery] Guid? userId,
                ISender sender,
                CancellationToken ct) =>
            {
                GetAlbumRatingResult result = await sender.Send(new GetAlbumRatingQuery(albumId, userId), ct);
                return Results.Ok(result.Adapt<GetAlbumRatingResponse>());
            })
            .WithName("GetAlbumRating")
            .Produces<GetAlbumRatingResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Album Rating")
            .WithTags("Album Ratings");

        app.MapPost("/albums/{albumId:guid}/rating", async (
                Guid albumId,
                RateAlbumRequest request,
                ISender sender,
                CancellationToken ct) =>
            {
                RateAlbumResult result = await sender.Send(new RateAlbumCommand(request.UserId, albumId, request.Rating), ct);
                return Results.Ok(result.Adapt<RateAlbumResponse>());
            })
            .WithName("RateAlbum")
            .Produces<RateAlbumResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Rate Album")
            .WithTags("Album Ratings");
    }
}
