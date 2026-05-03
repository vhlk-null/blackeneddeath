namespace Library.Application.Services.Import;

public interface IBandImportService
{
    Task<List<BandSearchCandidate>> SearchCandidatesAsync(string bandName, CancellationToken ct = default);

    Task<BandPreviewResult> PreviewByIdAsync(string id, IReadOnlySet<string> existingSlugs, CancellationToken ct = default);

    Task<MusicBrainzImportResult> ImportByIdAsync(
        string id,
        string bandName,
        IReadOnlySet<string>? selectedAlbumIds = null,
        IProgress<ImportProgressEvent>? progress = null,
        CancellationToken ct = default);
}
