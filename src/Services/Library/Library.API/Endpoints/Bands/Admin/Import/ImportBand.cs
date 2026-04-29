using Library.Application.Services.Import.Commands.ImportBand;

namespace Library.API.Endpoints.Bands.Admin.Import;

public record ImportBandRequest(string BandName);
public record ImportBandResponse(Guid BandId, string BandName, int AlbumsImported, int AlbumsSkipped);

public class ImportBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/admin/import/band",
                async (ImportBandRequest request, ISender sender, CancellationToken ct) =>
                {
                    ImportBandResult result = await sender.Send(new ImportBandCommand(request.BandName), ct);
                    return Results.Ok(result.Adapt<ImportBandResponse>());
                })
            .WithName("ImportBandFromMusicBrainz")
            .Produces<ImportBandResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Import band from MusicBrainz")
            .WithDescription("Searches MusicBrainz by band name and imports band + albums + tracks into the database.")
            .WithTags("Admin")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
