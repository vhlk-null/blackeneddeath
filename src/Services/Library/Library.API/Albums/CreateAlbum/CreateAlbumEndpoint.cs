using Library.Domain.Enums;
using Library.Domain.Models;

namespace Library.API.Albums.CreateAlbum;

public record CreateAlbumRequest(
    string Title,
    int ReleaseDate,
    AlbumType Type,
    AlbumFormat Format,
    string? Label,
    Guid? CountryId,
    List<Guid> BandIds,
    List<Guid> GenreIds,
    List<Guid> TagIds);

public record CreateAlbumResponse(Guid Id);

public class CreateAlbumEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/albums",
                async (CreateAlbumRequest request, ISender sender) =>
                {
                    var command = request.Adapt<CreateAlbumCommand>();

                    var result = await sender.Send(command);

                    var response = result.Adapt<CreateAlbumResponse>();

                    return Results.Created($"/albums/{response.Id}", response);
                })
            .WithName("CreatedAlbum")
            .Produces<CreateAlbumResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Album")
            .WithDescription("Create Album");
    }
}