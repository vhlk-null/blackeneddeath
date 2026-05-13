namespace Library.API.Endpoints.Albums;

public record RateAlbumRequest(Guid UserId, int Rating);
public record RateAlbumResponse(Guid AlbumId, int UserRating, double? AverageRating, int RatingsCount);
public record GetAlbumRatingResponse(Guid AlbumId, int? UserRating, double? AverageRating, int RatingsCount);

public class RateAlbum : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums/{id:guid}/rating", async (Guid id, Guid userId, ISender sender) =>
            {
                GetAlbumRatingQuery query = new GetAlbumRatingQuery(userId, id);
                GetAlbumRatingResult result = await sender.Send(query);
                GetAlbumRatingResponse response = result.Adapt<GetAlbumRatingResponse>();
                return Results.Ok(response);
            })
            .WithName("GetAlbumRating")
            .Produces<GetAlbumRatingResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Album Rating")
            .WithDescription("Get the current user's rating and aggregate stats for an album")
            .WithTags("Albums");

        app.MapPost("/albums/{id:guid}/rating", async (Guid id, RateAlbumRequest request, ISender sender) =>
            {
                RateAlbumCommand command = new RateAlbumCommand(request.UserId, id, request.Rating);
                RateAlbumResult result = await sender.Send(command);
                RateAlbumResponse response = result.Adapt<RateAlbumResponse>();
                return Results.Ok(response);
            })
            .WithName("RateAlbum")
            .Produces<RateAlbumResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Rate an Album")
            .WithDescription("Submit or update a user rating for an album")
            .WithTags("Albums");
    }
}
