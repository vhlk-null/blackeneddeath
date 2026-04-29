namespace Library.Application.Services.Import;

public interface IMusicBrainzImportService
{
    Task<MusicBrainzImportResult> ImportByNameAsync(
        string bandName,
        IProgress<ImportProgressEvent>? progress = null,
        bool includeAlbums = true,
        CancellationToken ct = default);
}

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
