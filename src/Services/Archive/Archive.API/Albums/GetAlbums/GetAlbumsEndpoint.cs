
namespace Archive.API.Albums.GetAlbums
{
    //public record GetAlbumsRequest();

    public record GetAlbumsResponse(IEnumerable<Album> Albums);

    public class GetAlbumsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/albums", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAlbumsQuery());

                var response = result.Adapt<GetAlbumsResponse>();

                return response;
            })
             .WithName("GetAlbums")
             .Produces<GetAlbumsResponse>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Get Albums")
             .WithDescription("Get Albums");
        }
    }
}
