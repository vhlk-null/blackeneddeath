namespace Library.API.Endpoints.Bands.Admin;

public record GetBandByIdAdminResponse(BandDetailDto Band);

public class GetBandByIdAdmin : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/admin/bands/{id:guid}", async (Guid id, ISender sender) =>
            {
                GetBandByIdResult result = await sender.Send(new GetBandByIdQuery(id, ApprovedOnly: false));
                return Results.Ok(result.Adapt<GetBandByIdAdminResponse>());
            })
            .WithName("AdminGetBandById")
            .Produces<GetBandByIdAdminResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Admin: Get Band by Id")
            .WithDescription("Get band by Id including unapproved")
            .WithTags("Admin")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
