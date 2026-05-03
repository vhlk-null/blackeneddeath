using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Library.Application.Services.Import;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Discogs;

public class DiscogsService(HttpClient http, ILogger<DiscogsService> logger)
    : IDiscogsService
{
    public async Task<DiscogsReleaseData?> GetReleaseAsync(string releaseId, CancellationToken ct = default)
    {
        try
        {
            var release = await http.GetFromJsonAsync<DiscogsRelease>($"releases/{releaseId}", ct);
            if (release is null) return null;

            string? labelName = release.Labels?.FirstOrDefault()?.Name;

            var genres = new List<string>();
            if (release.Styles is { Count: > 0 })
                genres.AddRange(release.Styles);
            else if (release.Genres is { Count: > 0 })
                genres.AddRange(release.Genres);

            logger.LogInformation("  → Discogs release {Id}: label='{Label}', genres=[{Genres}]",
                releaseId, labelName, string.Join(", ", genres));

            return new DiscogsReleaseData(labelName, genres);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Discogs lookup failed for release '{Id}'", releaseId);
            return null;
        }
    }

    private sealed class DiscogsRelease
    {
        [JsonPropertyName("labels")]
        public List<DiscogsLabel>? Labels { get; init; }

        [JsonPropertyName("genres")]
        public List<string>? Genres { get; init; }

        [JsonPropertyName("styles")]
        public List<string>? Styles { get; init; }
    }

    private sealed class DiscogsLabel
    {
        [JsonPropertyName("name")]
        public string? Name { get; init; }
    }
}
