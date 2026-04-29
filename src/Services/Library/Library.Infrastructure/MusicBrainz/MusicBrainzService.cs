using System.Net.Http.Json;
using System.Text.Json;
using Library.Application.Services.Import;
using Microsoft.Extensions.Logging;
using AppImportResult  = Library.Application.Services.Import.MusicBrainzImportResult;
using AppBandData      = Library.Application.Services.Import.BandImportData;
using AppAlbumData     = Library.Application.Services.Import.AlbumImportData;
using AppTrackData     = Library.Application.Services.Import.TrackImportData;
using AppAlbumTypeHint = Library.Application.Services.Import.AlbumTypeHint;

namespace Library.Infrastructure.MusicBrainz;

public class MusicBrainzService(HttpClient http, ILogger<MusicBrainzService> logger)
    : IMusicBrainzImportService
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new FlexibleBoolConverter() }
    };

    public async Task<List<BandSearchCandidate>> SearchCandidatesAsync(string bandName, CancellationToken ct = default)
    {
        var url = $"artist/?query=artist:\"{Uri.EscapeDataString(bandName)}\"+AND+type:group&limit=5&fmt=json";
        logger.LogInformation("MusicBrainz search URL: {Url}", url);

        var response = await http.GetFromJsonAsync<MbArtistSearchResponse>(url, JsonOpts, ct);

        logger.LogInformation("MusicBrainz search returned {Count} artists", response?.Artists?.Count ?? 0);

        return (response?.Artists ?? [])
            .Select(a => new BandSearchCandidate(
                MbId: a.Id,
                Name: a.Name,
                Disambiguation: a.Disambiguation,
                Country: a.Country,
                FormedYear: ParseYear(a.LifeSpan?.Begin)))
            .ToList();
    }

    public async Task<BandPreviewResult> PreviewByMbIdAsync(string mbId, IReadOnlySet<string> existingSlugs, CancellationToken ct = default)
    {
        try
        {
            var detail = await GetArtistDetailAsync(mbId, ct);
            if (detail is null)
                return new BandPreviewResult(false, "Could not load artist details.", null, null, null, null, null, false, [], 0, []);

            var band = MapBand(detail);

            List<BandPreviewAlbum> albums = (detail.ReleaseGroups ?? [])
                .Where(rg => IsAllowedType(rg.PrimaryType, rg.SecondaryTypes))
                .OrderBy(rg => rg.FirstReleaseDate)
                .Select(rg =>
                {
                    int? year = ParseYear(rg.FirstReleaseDate);
                    string slug = $"{SlugHelper.Generate(rg.Title)}-{year}";
                    return new BandPreviewAlbum(
                        Title: rg.Title,
                        Year: year,
                        Type: rg.PrimaryType ?? "Unknown",
                        Slug: slug,
                        MbUrl: $"https://musicbrainz.org/release-group/{rg.Id}",
                        ExistsInDb: existingSlugs.Contains(slug));
                })
                .ToList();

            return new BandPreviewResult(
                Found: true,
                ErrorMessage: null,
                MbId: mbId,
                Name: band.Name,
                Country: band.Country,
                FormedYear: band.FormedYear,
                DisbandedYear: band.DisbandedYear,
                IsActive: band.IsActive,
                Tags: band.Tags,
                ReleaseGroupCount: albums.Count,
                Albums: albums);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "MusicBrainz preview failed for mbId '{MbId}'", mbId);
            return new BandPreviewResult(false, ex.Message, null, null, null, null, null, false, [], 0, []);
        }
    }

    public async Task<AppImportResult> ImportByMbIdAsync(
        string mbId,
        string bandName,
        IReadOnlySet<string>? selectedAlbumMbIds = null,
        IProgress<ImportProgressEvent>? progress = null,
        CancellationToken ct = default)
    {
        try
        {
            progress?.Report(new ImportProgressEvent(ImportProgressStage.Searching, $"Loading '{bandName}'..."));

            var detail = await GetArtistDetailAsync(mbId, ct);
            if (detail is null)
            {
                progress?.Report(new ImportProgressEvent(ImportProgressStage.Failed, "Could not load artist details."));
                return Fail("Could not load artist details.");
            }

            progress?.Report(new ImportProgressEvent(ImportProgressStage.BandFound, $"Found: {detail.Name}"));

            var bandData = MapBand(detail);
            var albums = await FetchAlbumsAsync(detail, selectedAlbumMbIds, progress, ct);

            progress?.Report(new ImportProgressEvent(ImportProgressStage.Saving, "Saving to database..."));

            return new AppImportResult { Success = true, Band = bandData, Albums = albums };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "MusicBrainz import failed for mbId '{MbId}'", mbId);
            progress?.Report(new ImportProgressEvent(ImportProgressStage.Failed, ex.Message));
            return Fail(ex.Message);
        }
    }

    private async Task<MbArtist?> SearchArtistAsync(string name, CancellationToken ct)
    {
        var url = $"artist/?query=artist:\"{Uri.EscapeDataString(name)}\"+AND+type:group&limit=5&fmt=json";
        var response = await http.GetFromJsonAsync<MbArtistSearchResponse>(url, JsonOpts, ct);
        return response?.Artists.FirstOrDefault();
    }

    private async Task<MbArtistDetail?> GetArtistDetailAsync(string mbId, CancellationToken ct)
    {
        var url = $"artist/{mbId}?inc=release-groups+tags&fmt=json";
        return await http.GetFromJsonAsync<MbArtistDetail>(url, JsonOpts, ct);
    }

    private async Task<List<MbRelease>> BrowseReleasesAsync(string releaseGroupId, CancellationToken ct)
    {
        await RateLimit(ct);
        var url = $"release?release-group={releaseGroupId}&status=official&fmt=json";
        var result = await http.GetFromJsonAsync<MbReleaseBrowseResult>(url, JsonOpts, ct);
        return result?.Releases ?? [];
    }

    private async Task<MbRelease?> GetReleaseDetailAsync(string releaseId, CancellationToken ct)
    {
        await RateLimit(ct);
        var url = $"release/{releaseId}?inc=recordings+labels&fmt=json";
        return await http.GetFromJsonAsync<MbRelease>(url, JsonOpts, ct);
    }

    private async Task<string?> FetchCoverUrlAsync(string releaseId, CancellationToken ct)
    {
        try
        {
            await RateLimit(ct);
            var url = $"https://coverartarchive.org/release/{releaseId}";
            var caa = await http.GetFromJsonAsync<CoverArtResponse>(url, JsonOpts, ct);
            var front = caa?.Images.FirstOrDefault(i => i.Front);
            return front?.Thumbnails.S500 ?? front?.Thumbnails.Large ?? front?.Image;
        }
        catch
        {
            return null;
        }
    }

    private static AppBandData MapBand(MbArtistDetail d) => new()
    {
        Name          = d.Name,
        Country       = d.Country,
        FormedYear    = ParseYear(d.LifeSpan?.Begin),
        DisbandedYear = ParseYear(d.LifeSpan?.End),
        IsActive      = !(d.LifeSpan?.Ended ?? false),
        Tags          = d.Tags?
                         .OrderByDescending(t => t.Count)
                         .Select(t => t.Name)
                         .Take(10)
                         .ToList() ?? []
    };

    private async Task<List<AppAlbumData>> FetchAlbumsAsync(
        MbArtistDetail detail,
        IReadOnlySet<string>? selectedAlbumMbIds,
        IProgress<ImportProgressEvent>? progress,
        CancellationToken ct)
    {
        var allowedGroups = (detail.ReleaseGroups ?? [])
            .Where(rg => IsAllowedType(rg.PrimaryType, rg.SecondaryTypes))
            .Where(rg => selectedAlbumMbIds is null or { Count: 0 } || selectedAlbumMbIds.Contains(rg.Id))
            .ToList();

        logger.LogInformation("Release groups after filter: {Count}/{Total}",
            allowedGroups.Count, detail.ReleaseGroups?.Count ?? 0);

        int total = allowedGroups.Count;
        var albums = new List<AppAlbumData>();

        for (int i = 0; i < allowedGroups.Count; i++)
        {
            var rg = allowedGroups[i];

            progress?.Report(new ImportProgressEvent(
                ImportProgressStage.FetchingAlbum,
                $"Fetching: {rg.Title}",
                Current: i + 1,
                Total: total));

            // Get official releases in this group
            var releases = await BrowseReleasesAsync(rg.Id, ct);
            var earliestRelease = releases.OrderBy(r => r.Date).FirstOrDefault();

            var releaseDate = earliestRelease?.Date ?? rg.FirstReleaseDate;
            var (year, month, day) = ParseDate(releaseDate);

            if (year is null)
            {
                logger.LogWarning("Skipping '{Title}' — no release year", rg.Title);
                continue;
            }

            var releaseDetail = earliestRelease is not null ? await GetReleaseDetailAsync(earliestRelease.Id, ct) : null;
            var coverUrl = earliestRelease is not null ? await FetchCoverUrlAsync(earliestRelease.Id, ct) : null;
            var tracks = MapTracks(releaseDetail?.Media);

            string? labelName = releaseDetail?.LabelInfo?
                .FirstOrDefault(li => li.Label is not null)?.Label?.Name;

            albums.Add(new AppAlbumData
            {
                Title        = rg.Title,
                ReleaseYear  = year.Value,
                ReleaseMonth = month,
                ReleaseDay   = day,
                TypeHint     = MapAlbumTypeHint(rg.PrimaryType),
                CoverUrl     = coverUrl,
                LabelName    = labelName,
                Tracks       = tracks
            });

            progress?.Report(new ImportProgressEvent(
                ImportProgressStage.AlbumFetched,
                $"{rg.Title} ({year}) — {tracks.Count} tracks",
                Current: i + 1,
                Total: total));

            logger.LogInformation("  ✓ {Title} ({Year}), {TrackCount} tracks, cover: {HasCover}",
                rg.Title, year, tracks.Count, coverUrl is not null);
        }

        return albums;
    }

    private static List<AppTrackData> MapTracks(List<MbMedia>? media)
    {
        if (media is null) return [];

        var result = new List<AppTrackData>();
        int globalIndex = 1;

        foreach (var disc in media)
            foreach (var track in disc.Tracks ?? [])
                result.Add(new AppTrackData
                {
                    Title       = track.Title,
                    TrackNumber = globalIndex++,
                    Duration    = FormatDuration(track.LengthMs)
                });

        return result;
    }

    private static readonly HashSet<string> AllowedPrimaryTypes =
        ["Album", "EP", "Single"];

    private static readonly HashSet<string> BlockedSecondaryTypes =
        ["Live", "Compilation", "Remix", "Soundtrack", "Demo", "DJ-mix", "Mixtape/Street"];

    private static bool IsAllowedType(string? primaryType, List<string>? secondaryTypes)
    {
        if (primaryType is null) return false;
        if (!AllowedPrimaryTypes.Contains(primaryType)) return false;
        if (secondaryTypes is { Count: > 0 } && secondaryTypes.Any(s => BlockedSecondaryTypes.Contains(s))) return false;
        return true;
    }

    private static AppAlbumTypeHint MapAlbumTypeHint(string? mbType) => mbType?.ToLowerInvariant() switch
    {
        "album"       => AppAlbumTypeHint.FullLength,
        "ep"          => AppAlbumTypeHint.EP,
        "single"      => AppAlbumTypeHint.Single,
        "live"        => AppAlbumTypeHint.LiveAlbum,
        "compilation" => AppAlbumTypeHint.Compilation,
        "demo"        => AppAlbumTypeHint.Demo,
        _             => AppAlbumTypeHint.Unknown
    };

    private static int? ParseYear(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        return int.TryParse(value.Split('-')[0], out var y) ? y : null;
    }

    private static (int? year, int? month, int? day) ParseDate(string? date)
    {
        if (string.IsNullOrWhiteSpace(date)) return (null, null, null);
        var parts = date.Split('-');
        int? y = parts.Length > 0 && int.TryParse(parts[0], out var yy) ? yy : null;
        int? m = parts.Length > 1 && int.TryParse(parts[1], out var mm) ? mm : null;
        int? d = parts.Length > 2 && int.TryParse(parts[2], out var dd) ? dd : null;
        return (y, m, d);
    }

    private static string? FormatDuration(int? ms)
    {
        if (ms is null) return null;
        var span = TimeSpan.FromMilliseconds(ms.Value);
        return span.Hours > 0 ? span.ToString(@"h\:mm\:ss") : span.ToString(@"m\:ss");
    }

    private static Task RateLimit(CancellationToken ct) =>
        Task.Delay(TimeSpan.FromSeconds(1), ct);

    private static AppImportResult Fail(string msg) =>
        new() { Success = false, ErrorMessage = msg };
}
