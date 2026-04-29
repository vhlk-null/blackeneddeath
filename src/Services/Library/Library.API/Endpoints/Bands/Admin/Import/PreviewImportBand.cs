using Library.Application.Services.Import;

namespace Library.API.Endpoints.Bands.Admin.Import;

public record BandSearchCandidateResponse(string MbId, string Name, string? Disambiguation, string? Country, int? FormedYear);
public record BandPreviewAlbumResponse(string Title, int? Year, string Type);
public record BandPreviewResponse(
    bool Found,
    string? ErrorMessage,
    string? MbId,
    string? Name,
    string? Country,
    int? FormedYear,
    int? DisbandedYear,
    bool IsActive,
    List<string> Tags,
    int ReleaseGroupCount,
    List<BandPreviewAlbumResponse> Albums);

public class PreviewImportBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/admin/import/band/search",
                async (string bandName, IMusicBrainzImportService musicBrainz, CancellationToken ct) =>
                {
                    List<BandSearchCandidate> candidates = await musicBrainz.SearchCandidatesAsync(bandName, ct);
                    return Results.Ok(candidates.Adapt<List<BandSearchCandidateResponse>>());
                })
            .WithName("SearchImportBandCandidates")
            .Produces<List<BandSearchCandidateResponse>>()
            .WithTags("Admin")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly")
            .WithSummary("Search MusicBrainz for band candidates by name");

        app.MapGet("/admin/import/band/preview",
                async (string mbId, IMusicBrainzImportService musicBrainz, CancellationToken ct) =>
                {
                    BandPreviewResult result = await musicBrainz.PreviewByMbIdAsync(mbId, ct);
                    return Results.Ok(result.Adapt<BandPreviewResponse>());
                })
            .WithName("PreviewImportBand")
            .Produces<BandPreviewResponse>()
            .WithTags("Admin")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly")
            .WithSummary("Preview band info from MusicBrainz before importing");
    }
}
