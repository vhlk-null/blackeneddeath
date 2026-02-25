using Library.Domain.Enums;
using Library.Domain.Models;

namespace Library.API.Albums.UpdateAlbum;

public record UpdateAlbumRequest(Guid Id,
    string Title,
    int ReleaseDate,
    AlbumType Type,
    AlbumFormat Format,
    string? Label,
    Guid? CountryId,
    List<Guid> BandIds,
    List<Guid> GenreIds,
    List<Guid> TagIds);

public record UpdateAlbumResponse(bool IsSuccess);

public class UpdateAlbumEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/albums", async (UpdateAlbumRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateAlbumCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateAlbumResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateAlbums")
            .Produces<UpdateAlbumResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Albums")
            .WithDescription("Update Albums");
    }
}