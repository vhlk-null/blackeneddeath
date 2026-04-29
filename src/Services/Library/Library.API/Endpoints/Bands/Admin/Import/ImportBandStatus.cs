using Library.Application.Services.Import;

namespace Library.API.Endpoints.Bands.Admin.Import;

public class ImportBandStatus : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/admin/import/band/status",
                (ImportStatusService importStatus) =>
                    Results.Ok(importStatus.Current))
            .WithName("GetImportBandStatus")
            .WithTags("Admin")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly")
            .WithSummary("Get current band import status");
    }
}
