namespace Library.Application.Services.Import;

public interface IMusicBrainzImportService
{
    Task<List<BandSearchCandidate>> SearchCandidatesAsync(string bandName, CancellationToken ct = default);

    Task<BandPreviewResult> PreviewByMbIdAsync(string mbId, CancellationToken ct = default);

    Task<MusicBrainzImportResult> ImportByMbIdAsync(
        string mbId,
        string bandName,
        IProgress<ImportProgressEvent>? progress = null,
        CancellationToken ct = default);
}

public record BandSearchCandidate(
    string MbId,
    string Name,
    string? Disambiguation,
    string? Country,
    int? FormedYear);

public record BandPreviewResult(
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
    List<BandPreviewAlbum> Albums);

public record BandPreviewAlbum(string Title, int? Year, string Type);

public record ImportProgressEvent(
    ImportProgressStage Stage,
    string Message,
    int Current = 0,
    int Total = 0);

public enum ImportProgressStage
{
    Searching,
    BandFound,
    FetchingAlbum,
    AlbumFetched,
    Saving,
    Done,
    Failed
}

public class MusicBrainzImportResult
{
    public bool Success { get; init; }
    public string? ErrorMessage { get; init; }
    public BandImportData? Band { get; init; }
    public List<AlbumImportData> Albums { get; init; } = [];
}

public class BandImportData
{
    public string Name { get; init; } = null!;
    public string? Country { get; init; }
    public int? FormedYear { get; init; }
    public int? DisbandedYear { get; init; }
    public bool IsActive { get; init; }
    public List<string> Tags { get; init; } = [];
}

public class AlbumImportData
{
    public string Title { get; init; } = null!;
    public int ReleaseYear { get; init; }
    public int? ReleaseMonth { get; init; }
    public int? ReleaseDay { get; init; }
    public AlbumTypeHint TypeHint { get; init; }
    public string? CoverUrl { get; init; }
    public string? LabelName { get; init; }
    public List<TrackImportData> Tracks { get; init; } = [];
}

public class TrackImportData
{
    public string Title { get; init; } = null!;
    public int TrackNumber { get; init; }
    public string? Duration { get; init; }
}

public enum AlbumTypeHint
{
    FullLength,
    EP,
    Single,
    Demo,
    LiveAlbum,
    Compilation,
    Split,
    Unknown
}
