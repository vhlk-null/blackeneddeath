namespace Library.API.Endpoints.Albums;

public record GetAlbumNamesResponse(List<NameIdDto> Albums);

public class GetAlbumNames : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums/names", async (ISender sender) =>
            {
                GetAlbumNamesResult result = await sender.Send(new GetAlbumNamesQuery());
                return Results.Ok(result.Adapt<GetAlbumNamesResponse>());
            })
            .WithName("GetAlbumNames")
            .Produces<GetAlbumNamesResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Album Names")
            .WithDescription("Returns all albums with only their Id and Name")
            .WithTags("Albums");
    }
}
