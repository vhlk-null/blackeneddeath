using Library.Application.Services.Import;

namespace Library.API.Endpoints.Import;

public class ImportBandStatus : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/admin/import/band/status",
                (ImportStatusService importStatus) =>
                    Results.Ok(importStatus.Current))
            .WithName("GetImportBandStatus")
            .WithTags("Admin")
            .AllowAnonymous()
            .WithSummary("Get current band import status");
    }
}
