using Library.Application.Services.Import.Commands.ImportBand;

namespace Library.API.Endpoints.Bands.Admin.Import;

public record ImportBandRequest(string MbId, string BandName, List<string>? SelectedAlbumMbIds = null);
public record ImportBandResponse(Guid BandId, string BandName, int AlbumsImported, int AlbumsSkipped);

public class ImportBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/admin/import/band",
                async (ImportBandRequest request, ISender sender, CancellationToken ct) =>
                {
                    IReadOnlySet<string>? selectedIds = request.SelectedAlbumMbIds is { Count: > 0 }
                        ? request.SelectedAlbumMbIds.ToHashSet()
                        : null;
                    ImportBandResult result = await sender.Send(new ImportBandCommand(request.MbId, request.BandName, SelectedAlbumMbIds: selectedIds), ct);
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
