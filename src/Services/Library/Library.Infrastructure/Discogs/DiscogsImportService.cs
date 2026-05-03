using System.Net.Http.Json;
using System.Text.Json;
using Library.Application.Services.Import;
using Library.Domain.Enums;
using Library.Infrastructure.Resolvers;
using Microsoft.Extensions.Logging;
using AppImportResult  = Library.Application.Services.Import.MusicBrainzImportResult;
using AppAlbumTypeHint = Library.Application.Services.Import.AlbumTypeHint;

namespace Library.Infrastructure.Discogs;

public class DiscogsImportService(HttpClient http, ILogger<DiscogsImportService> logger, IOdesliService odesli, YouTubeResolver? youtube = null)
    : IBandImportService
{
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    public async Task<List<BandSearchCandidate>> SearchCandidatesAsync(string bandName, CancellationToken ct = default)
    {
        string term = Uri.EscapeDataString(bandName);
        var response = await http.GetFromJsonAsync<DiscogsSearchResponse>(
            $"database/search?q={term}&type=artist&per_page=10", JsonOpts, ct);

        var results = (response?.Results ?? [])
            .Where(r => CleanArtistName(r.Title).Equals(bandName.Trim(), StringComparison.OrdinalIgnoreCase))
            .ToList();

        // Fall back to starts-with if no exact matches
        if (results.Count == 0)
            results = (response?.Results ?? [])
                .Where(r => CleanArtistName(r.Title).StartsWith(bandName.Trim(), StringComparison.OrdinalIgnoreCase))
                .ToList();

        var candidates = new List<BandSearchCandidate>();

        var enrichTasks = results.Select(async r =>
        {
            var yearTask   = FetchEarliestReleaseYearAsync(r.Id.ToString(), ct);
            var artistTask = GetArtistAsync(r.Id.ToString(), ct);
            await Task.WhenAll(yearTask, artistTask);
            return (r, formedYear: yearTask.Result, artist: artistTask.Result);
        });

        var enriched = await Task.WhenAll(enrichTasks);

        foreach (var (r, formedYear, artist) in enriched)
        {

            // Try to extract country from profile text e.g. "...band from Poland..."
            string? country = ExtractCountryFromProfile(artist?.Profile);

            var genres = r.Style is { Count: > 0 } ? r.Style : r.Genre;
            var parts = new List<string>();
            if (genres is { Count: > 0 }) parts.Add(string.Join(", ", genres.Take(3)));
            if (!string.IsNullOrWhiteSpace(r.Country)) parts.Add(r.Country);
            string? disambiguation = parts.Count > 0 ? string.Join(" · ", parts) : null;

            string? profileUrl = r.Uri is not null
                ? $"https://www.discogs.com{r.Uri}"
                : $"https://www.discogs.com/artist/{r.Id}";

            candidates.Add(new BandSearchCandidate(
                MbId: r.Id.ToString(),
                Name: CleanArtistName(r.Title),
                Disambiguation: disambiguation,
                Country: country,
                FormedYear: formedYear,
                ProfileUrl: profileUrl));
        }

        return candidates;
    }

    public async Task<BandPreviewResult> PreviewByIdAsync(string id, IReadOnlySet<string> existingSlugs, CancellationToken ct = default)
    {
        try
        {
            var artist = await GetArtistAsync(id, ct);
            if (artist is null)
                return FailPreview("Could not load artist details.");

            var releases = await GetMainReleasesAsync(id, ct);

            var albums = releases
                .Select(r =>
                {
                    int? year = r.Year;
                    string slug = $"{SlugHelper.Generate(r.Title)}-{year}";
                    return new BandPreviewAlbum(
                        Title: r.Title,
                        Year: year,
                        Type: MapFormatToType(r.Format),
                        Slug: slug,
                        MbUrl: r.Type == "master"
                            ? $"https://www.discogs.com/master/{r.Id}"
                            : $"https://www.discogs.com/release/{r.Id}",
                        ExistsInDb: existingSlugs.Contains(slug),
                        Format: r.Format,
                        SourceId: r.Id.ToString());
                })
                .ToList();

            return new BandPreviewResult(
                Found: true,
                ErrorMessage: null,
                MbId: id,
                Name: CleanArtistName(artist.Name),
                Country: null,
                FormedYear: null,
                DisbandedYear: null,
                IsActive: true,
                Tags: [],
                ReleaseGroupCount: albums.Count,
                Albums: albums);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Discogs preview failed for id '{Id}'", id);
            return FailPreview(ex.Message);
        }
    }

    public async Task<AppImportResult> ImportByIdAsync(
        string id,
        string bandName,
        IReadOnlySet<string>? selectedAlbumIds = null,
        IProgress<ImportProgressEvent>? progress = null,
        CancellationToken ct = default)
    {
        try
        {
            progress?.Report(new ImportProgressEvent(ImportProgressStage.Searching, $"Loading '{bandName}'..."));

            var artist = await GetArtistAsync(id, ct);
            if (artist is null)
            {
                progress?.Report(new ImportProgressEvent(ImportProgressStage.Failed, "Could not load artist details."));
                return Fail("Could not load artist details.");
            }

            progress?.Report(new ImportProgressEvent(ImportProgressStage.BandFound, $"Found: {CleanArtistName(artist.Name)}"));

            var bandData = new BandImportData
            {
                Name    = CleanArtistName(artist.Name),
                IsActive = true,
            };

            var releases = await GetMainReleasesAsync(id, ct);
            logger.LogInformation("Discogs: found {Total} main releases for '{Name}'", releases.Count, bandData.Name);

            // UI sends IDs as "release-{id}" or "master-{id}" (taken from MbUrl)
            var normalizedSelected = selectedAlbumIds?
                .Select(s => s.Replace("release-", "").Replace("master-", ""))
                .ToHashSet();

            var allowed = releases
                .Where(r => normalizedSelected is not { Count: > 0 } || normalizedSelected.Contains(r.Id.ToString()))
                .ToList();

            int total  = allowed.Count;
            var albums = new List<AlbumImportData>();

            for (int i = 0; i < allowed.Count; i++)
            {
                var rel = allowed[i];

                logger.LogInformation("  [{Current}/{Total}] Fetching: '{Title}' (id={Id}, type={Type})",
                    i + 1, total, rel.Title, rel.Id, rel.Type);

                progress?.Report(new ImportProgressEvent(
                    ImportProgressStage.FetchingAlbum,
                    $"Fetching: {rel.Title}",
                    Current: i + 1,
                    Total: total));

                var detail = await FetchReleaseDetailAsync(rel, bandData.Name, ct);
                if (detail is null)
                {
                    logger.LogWarning("  [{Current}/{Total}] Skipped '{Title}' — could not fetch detail", i + 1, total, rel.Title);
                    continue;
                }

                logger.LogInformation("  [{Current}/{Total}] ✓ '{Title}' ({Year}) — {Tracks} tracks, label='{Label}', genres=[{Genres}], cover={HasCover}",
                    i + 1, total, detail.Title, detail.ReleaseYear, detail.Tracks.Count,
                    detail.LabelName ?? "none",
                    string.Join(", ", detail.Genres),
                    detail.CoverUrl is not null);

                progress?.Report(new ImportProgressEvent(
                    ImportProgressStage.AlbumFetched,
                    $"{detail.Title} ({detail.ReleaseYear}) — {detail.Tracks.Count} tracks",
                    Current: i + 1,
                    Total: total));

                albums.Add(detail);
            }

            progress?.Report(new ImportProgressEvent(ImportProgressStage.Saving, "Saving to database..."));

            return new AppImportResult { Success = true, Band = bandData, Albums = albums };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Discogs import failed for id '{Id}'", id);
            progress?.Report(new ImportProgressEvent(ImportProgressStage.Failed, ex.Message));
            return Fail(ex.Message);
        }
    }

    // ── private helpers ──────────────────────────────────────────────────────

    private async Task<int?> FetchEarliestReleaseYearAsync(string artistId, CancellationToken ct)
    {
        try
        {
            await RateLimit(ct);
            var response = await http.GetFromJsonAsync<DiscogsArtistReleasesResponse>(
                $"artists/{artistId}/releases?sort=year&sort_order=asc&per_page=5", JsonOpts, ct);

            return response?.Releases
                .Where(r => r.Year is > 1900)
                .Select(r => r.Year)
                .FirstOrDefault();
        }
        catch
        {
            return null;
        }
    }

    private async Task<DiscogsArtist?> GetArtistAsync(string id, CancellationToken ct)
    {
        await RateLimit(ct);
        return await http.GetFromJsonAsync<DiscogsArtist>($"artists/{id}", JsonOpts, ct);
    }

    private async Task<List<DiscogsArtistRelease>> GetMainReleasesAsync(string artistId, CancellationToken ct)
    {
        var all = new List<DiscogsArtistRelease>();
        int page = 1;
        int totalPages = 1;
        do
        {
            await RateLimit(ct);
            var response = await http.GetFromJsonAsync<DiscogsArtistReleasesResponse>(
                $"artists/{artistId}/releases?sort=year&sort_order=asc&per_page=100&page={page}", JsonOpts, ct);
            all.AddRange(response?.Releases ?? []);
            totalPages = response?.Pagination?.Pages ?? 1;
            page++;
        }
        while (page <= totalPages);

        logger.LogInformation("Discogs: artist {Id} — {All} total releases fetched across {Pages} page(s)",
            artistId, all.Count, totalPages);

        // Log all Main role entries so we can see raw type/format values
        foreach (var r in all.Where(r => r.Role == "Main"))
            logger.LogDebug("  Main release: id={Id} type={Type} format={Format} year={Year} title={Title}",
                r.Id, r.Type, r.Format, r.Year, r.Title);

        var masters = all.Where(r => r.Role == "Main" && r.Type == "master").ToList();

        var masterKeys = masters
            .Select(m => (m.Title.ToLowerInvariant(), m.Year))
            .ToHashSet();

        var standaloneReleases = all
            .Where(r => r.Role == "Main" && r.Type == "release"
                && !masterKeys.Contains((r.Title.ToLowerInvariant(), r.Year)))
            .ToList();

        var main = masters.Concat(standaloneReleases)
            .OrderBy(r => r.Year)
            .ToList();

        logger.LogInformation("Discogs: {Masters} masters + {Standalone} standalone releases → {Main} combined",
            masters.Count, standaloneReleases.Count, main.Count);

        return main;
    }

    private async Task<AlbumImportData?> FetchReleaseDetailAsync(DiscogsArtistRelease rel, string bandName, CancellationToken ct)
    {
        try
        {
            await RateLimit(ct);

            AlbumImportData? data;

            // Prefer master release for genre/styles/tracklist; fall back to release
            if (rel.Type == "master")
            {
                DiscogsMasterRelease? master = null;
                try
                {
                    master = await http.GetFromJsonAsync<DiscogsMasterRelease>(
                        $"masters/{rel.Id}", JsonOpts, ct);
                }
                catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    logger.LogWarning("  → '{Title}' master/{Id} not found, falling back to release/{Id}", rel.Title, rel.Id);
                }

                if (master is null)
                {
                    await RateLimit(ct);
                    var fallback = await http.GetFromJsonAsync<DiscogsRelease>($"releases/{rel.Id}", JsonOpts, ct);
                    if (fallback is null) return null;

                    data = new AlbumImportData
                    {
                        Title          = fallback.Title,
                        ReleaseYear    = fallback.Year ?? rel.Year ?? 0,
                        TypeHint       = MapFormatToTypeHint(rel.Format),
                        LabelName      = CleanLabelName(fallback.Labels?.FirstOrDefault()?.Name),
                        Genres         = BuildGenres(fallback.Styles, fallback.Genres),
                        CoverUrl       = fallback.Images?.FirstOrDefault(i => i.Type == "primary")?.Uri
                                      ?? fallback.Images?.FirstOrDefault()?.Uri,
                        Tracks         = MapTracks(fallback.Tracklist),
                        StreamingLinks = ExtractStreamingLinks(fallback.Videos),
                    };
                }
                else
                {
                    string? labelName = null;
                    List<StreamingLinkImportData> streamingLinks = [];
                    if (master.MainRelease != 0)
                    {
                        await RateLimit(ct);
                        var mainRelease = await http.GetFromJsonAsync<DiscogsRelease>(
                            $"releases/{master.MainRelease}", JsonOpts, ct);
                        labelName      = CleanLabelName(mainRelease?.Labels?.FirstOrDefault()?.Name);
                        streamingLinks = ExtractStreamingLinks(mainRelease?.Videos);
                        logger.LogInformation("  → '{Title}' main release {Id}: label='{Label}', links={LinkCount}",
                            rel.Title, master.MainRelease, labelName ?? "none", streamingLinks.Count);
                    }

                    foreach (var link in ExtractStreamingLinks(master.Videos))
                        if (!streamingLinks.Any(l => l.Platform == link.Platform))
                            streamingLinks.Add(link);

                    data = new AlbumImportData
                    {
                        Title          = master.Title,
                        ReleaseYear    = master.Year ?? rel.Year ?? 0,
                        TypeHint       = MapFormatToTypeHint(rel.Format),
                        LabelName      = labelName,
                        Genres         = BuildGenres(master.Styles, master.Genres),
                        CoverUrl       = master.Images?.FirstOrDefault(i => i.Type == "primary")?.Uri
                                      ?? master.Images?.FirstOrDefault()?.Uri,
                        Tracks         = MapTracks(master.Tracklist),
                        StreamingLinks = streamingLinks,
                    };
                }
            }
            else
            {
                var release = await http.GetFromJsonAsync<DiscogsRelease>($"releases/{rel.Id}", JsonOpts, ct);
                if (release is null) return null;

                data = new AlbumImportData
                {
                    Title          = release.Title,
                    ReleaseYear    = release.Year ?? rel.Year ?? 0,
                    TypeHint       = MapFormatToTypeHint(rel.Format),
                    LabelName      = CleanLabelName(release.Labels?.FirstOrDefault()?.Name),
                    Genres         = BuildGenres(release.Styles, release.Genres),
                    CoverUrl       = release.Images?.FirstOrDefault(i => i.Type == "primary")?.Uri
                                  ?? release.Images?.FirstOrDefault()?.Uri,
                    Tracks         = MapTracks(release.Tracklist),
                    StreamingLinks = ExtractStreamingLinks(release.Videos),
                };
            }

            // Enrich with Odesli if we have a YouTube seed link
            var youtubeLink = data.StreamingLinks.FirstOrDefault(l => l.Platform == StreamingPlatform.YouTube);

            // If Discogs had no YouTube video, search for one via YouTube API
            if (youtubeLink is null && youtube is not null)
            {
                logger.LogInformation("  → No YouTube link from Discogs for '{Title}', searching YouTube", data.Title);
                string? ytUrl = await youtube.ResolveAsync(bandName, data.Title, ct);
                if (ytUrl is not null)
                {
                    youtubeLink = new StreamingLinkImportData(StreamingPlatform.YouTube, ytUrl);
                    data.StreamingLinks.Add(youtubeLink);
                    logger.LogInformation("  → YouTube search found: {Url}", ytUrl);
                }
                else
                {
                    logger.LogInformation("  → YouTube search found nothing for '{Title}'", data.Title);
                }
            }

            if (youtubeLink is not null)
            {
                logger.LogInformation("  → Enriching '{Title}' via Odesli from YouTube seed", data.Title);
                var odesliLinks = await odesli.GetLinksAsync(youtubeLink.Url, ct);
                foreach (var link in odesliLinks)
                    if (!data.StreamingLinks.Any(l => l.Platform == link.Platform))
                        data.StreamingLinks.Add(link);
                logger.LogInformation("  → Odesli returned {Count} platform link(s) for '{Title}'",
                    odesliLinks.Count, data.Title);
            }
            else
            {
                logger.LogInformation("  → No YouTube link available for '{Title}', skipping Odesli", data.Title);
            }

            return data;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to fetch Discogs release detail for '{Title}' (id={Id})", rel.Title, rel.Id);
            return null;
        }
    }

    private static List<StreamingLinkImportData> ExtractStreamingLinks(List<DiscogsVideo>? videos)
    {
        if (videos is not { Count: > 0 }) return [];

        var youtubeUrl = videos
            .Select(v => v.Uri)
            .FirstOrDefault(u => u is not null &&
                (u.Contains("youtube.com") || u.Contains("youtu.be")));

        return youtubeUrl is not null
            ? [new StreamingLinkImportData(StreamingPlatform.YouTube, youtubeUrl)]
            : [];
    }

    private static List<TrackImportData> MapTracks(List<DiscogsTrack>? tracklist)
    {
        if (tracklist is null) return [];

        return tracklist
            .Where(t => t.Type != "heading" && !string.IsNullOrWhiteSpace(t.Title))
            .Select((t, i) => new TrackImportData
            {
                Title       = t.Title,
                TrackNumber = i + 1,
                Duration    = NormalizeDuration(t.Duration),
            })
            .ToList();
    }

    private static string? NormalizeDuration(string? duration)
    {
        if (string.IsNullOrWhiteSpace(duration)) return null;
        var parts = duration.Split(':');
        if (parts.Length != 2) return duration;
        if (int.TryParse(parts[0], out int min) && int.TryParse(parts[1], out int sec))
            return $"{min:D2}:{sec:D2}";
        return duration;
    }

    private static List<string> BuildGenres(List<string>? styles, List<string>? genres)
    {
        if (styles is { Count: > 0 }) return styles;
        if (genres is { Count: > 0 }) return genres;
        return [];
    }

    private static string? ExtractCountryFromProfile(string? profile)
    {
        if (string.IsNullOrWhiteSpace(profile)) return null;

        // "from Warsaw, Poland" or "from Poland"
        var m = System.Text.RegularExpressions.Regex.Match(profile,
            @"from\s+(?:[A-Z][a-zA-Z\s]+,\s*)?([A-Z][a-zA-Z\s]{2,20})(?:\b)",
            System.Text.RegularExpressions.RegexOptions.None);
        if (m.Success) return m.Groups[1].Value.Trim();

        // "Polish metal band", "Ukrainian black metal"
        m = System.Text.RegularExpressions.Regex.Match(profile,
            @"\b(Polish|Ukrainian|German|Swedish|Norwegian|Finnish|French|American|British|Dutch|Italian|Greek|Czech|Slovak|Hungarian|Romanian|Bulgarian|Russian|Brazilian|Canadian|Australian)\b",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (m.Success) return m.Groups[1].Value.Trim();

        return null;
    }

    private static string CleanArtistName(string name)
    {
        // Discogs appends " (N)" to disambiguate duplicate names e.g. "Behemoth (3)"
        int paren = name.LastIndexOf(" (", StringComparison.Ordinal);
        if (paren > 0 && name.EndsWith(')'))
            return name[..paren].Trim();
        return name.Trim();
    }

    private static string? CleanLabelName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;
        if (name.Equals("Not On Label", StringComparison.OrdinalIgnoreCase)) return null;
        return name.Trim();
    }

    private static string MapFormatToType(string? format) => format?.ToLowerInvariant() switch
    {
        var f when f?.Contains("ep") == true   => "EP",
        var f when f?.Contains("single") == true => "Single",
        var f when f?.Contains("comp") == true => "Compilation",
        _ => "Album"
    };

    private static AppAlbumTypeHint MapFormatToTypeHint(string? format) => format?.ToLowerInvariant() switch
    {
        var f when f?.Contains("ep") == true      => AppAlbumTypeHint.EP,
        var f when f?.Contains("single") == true  => AppAlbumTypeHint.Single,
        var f when f?.Contains("comp") == true    => AppAlbumTypeHint.Compilation,
        var f when f?.Contains("live") == true    => AppAlbumTypeHint.LiveAlbum,
        var f when f?.Contains("demo") == true    => AppAlbumTypeHint.Demo,
        _ => AppAlbumTypeHint.FullLength
    };

    private static Task RateLimit(CancellationToken ct) =>
        Task.Delay(TimeSpan.FromMilliseconds(500), ct);

    private static AppImportResult Fail(string msg) =>
        new() { Success = false, ErrorMessage = msg };

    private static BandPreviewResult FailPreview(string msg) =>
        new(false, msg, null, null, null, null, null, false, [], 0, []);
}

