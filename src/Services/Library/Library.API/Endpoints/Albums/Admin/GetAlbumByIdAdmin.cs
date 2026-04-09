namespace Library.API.Endpoints.Albums.Admin;

public record GetAlbumByIdAdminResponse(AlbumDetailDto Album);

public class GetAlbumByIdAdmin : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/admin/albums/{id:guid}", async (Guid id, ISender sender) =>
            {
                GetAlbumByIdResult result = await sender.Send(new GetAlbumByIdQuery(id, ApprovedOnly: false));
                return Results.Ok(result.Adapt<GetAlbumByIdAdminResponse>());
            })
            .WithName("AdminGetAlbumById")
            .Produces<GetAlbumByIdAdminResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Admin: Get Album by Id")
            .WithDescription("Get album by Id including unapproved")
            .WithTags("Admin")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
