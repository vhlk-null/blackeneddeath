using Library.Application.Services.GenreCards.Queries.GetAlbumsByGenreCard;

namespace Library.API.Endpoints.GenreCards;

public class GetAlbumsByGenreCard : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/genre-cards/{id:guid}/albums",
                async (Guid id, [AsParameters] PaginationRequest pagination, ISender sender) =>
                {
                    var result = await sender.Send(new GetAlbumsByGenreCardQuery(id, pagination));

                    return Results.Ok(result.Albums);
                })
            .WithName("GetAlbumsByGenreCard")
            .Produces<PaginatedResult<AlbumCardDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Albums by GenreCard")
            .WithDescription("Returns albums matching any genre or tag from the card (OR logic)")
            .WithTags("GenreCards");
    }
}
