using Library.Application.Services.Import;

namespace Library.API.Endpoints.Bands.Admin.Import;

public record BandSearchCandidateResponse(string MbId, string Name, string? Disambiguation, string? Country, int? FormedYear, string? ProfileUrl);
public record BandPreviewAlbumResponse(string Title, int? Year, string Type, string Slug, string MbUrl, bool ExistsInDb, string? Format = null, string? SourceId = null);
public record BandPreviewGroupResponse(string GroupType, List<BandPreviewAlbumResponse> Albums);
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
    List<BandPreviewAlbumResponse> Albums,
    List<BandPreviewGroupResponse> Groups);

public class PreviewImportBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/admin/import/band/search",
                async (string bandName, IBandImportService musicBrainz, CancellationToken ct) =>
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
                async (string mbId, ISender sender, CancellationToken ct) =>
                {
                    BandPreviewResult result = await sender.Send(new PreviewImportBandQuery(mbId), ct);
                    var albumResponses = result.Albums.Adapt<List<BandPreviewAlbumResponse>>();
                    var groups = albumResponses
                        .GroupBy(a => a.Type)
                        .OrderBy(g => g.Key)
                        .Select(g => new BandPreviewGroupResponse(g.Key, g.OrderBy(a => a.Year).ToList()))
                        .ToList();
                    var response = new BandPreviewResponse(
                        result.Found, result.ErrorMessage, result.MbId, result.Name,
                        result.Country, result.FormedYear, result.DisbandedYear, result.IsActive,
                        result.Tags, result.ReleaseGroupCount, albumResponses, groups);
                    return Results.Ok(response);
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
