using Library.API.Albums.GetAlbums;

namespace Library.API.Albums.GetAlbumsBy.GetAlbumById;

//public record GetAlbumByIdRequest(Guid Id) : IRequest<GetAlbumByIdResult>;
public record GetAlbumByIdResponse(AlbumDto Album);

public class GetAlbumByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums/{id:guid}", async (Guid id, ISender sender) =>
            {
                var query = new GetAlbumByIdQuery(id);
                var result = await sender.Send(query);
                var response = result.Adapt<GetAlbumByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("GetAlbumById")
            .Produces<GetAlbumByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Album by Id")
            .WithDescription("Get Album by Id");
    }
}