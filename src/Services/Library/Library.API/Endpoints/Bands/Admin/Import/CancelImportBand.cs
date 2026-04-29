using Library.Application.Services.Import;

namespace Library.API.Endpoints.Bands.Admin.Import;

public class CancelImportBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/admin/import/band",
                (ImportStatusService importStatus) =>
                {
                    importStatus.Cancel();
                    return Results.Ok();
                })
            .WithName("CancelImportBand")
            .WithTags("Admin")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly")
            .WithSummary("Cancel an in-progress band import");
    }
}
