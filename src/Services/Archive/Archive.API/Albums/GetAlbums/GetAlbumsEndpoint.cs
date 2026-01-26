namespace Archive.API.Albums.GetAlbums
{
    public record GetAlbumsRequest(int? PageNumber, int? PageSize = 10);

    public record GetAlbumsResult(
        List<Album> Items,
        int PageNumber,
        int PageSize,
        int TotalCount,
        int TotalPages,
        bool HasPreviousPage,
        bool HasNextPage
    );

    public class GetAlbumsEndpoint : ICarterModule
    {       
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/albums", async([AsParameters] GetAlbumsRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetAlbumsQuery>();

                var result = await sender.Send(query);

                var response = result.Adapt<GetAlbumsResult>();

                return response;
            })
             .WithName("GetAlbums")
             .Produces<GetAlbumsResult>(StatusCodes.Status200OK)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .WithSummary("Get Albums")
             .WithDescription("Get Albums");
        }
    }
}
