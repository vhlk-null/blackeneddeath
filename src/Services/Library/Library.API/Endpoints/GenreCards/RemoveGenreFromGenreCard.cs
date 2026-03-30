using Library.Application.Services.GenreCards.Commands.RemoveGenreFromGenreCard;

namespace Library.API.Endpoints.GenreCards;

public record RemoveGenreFromGenreCardResponse(bool IsSuccess);

public class RemoveGenreFromGenreCard : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/genre-cards/{id:guid}/genres/{genreId:guid}",
                async (Guid id, Guid genreId, ISender sender) =>
                {
                    var result = await sender.Send(new RemoveGenreFromGenreCardCommand(id, genreId));

                    return Results.Ok(result.Adapt<RemoveGenreFromGenreCardResponse>());
                })
            .WithName("RemoveGenreFromGenreCard")
            .Produces<RemoveGenreFromGenreCardResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Remove Genre from GenreCard")
            .WithDescription("Remove Genre from GenreCard")
            .WithTags("GenreCards");
    }
}
