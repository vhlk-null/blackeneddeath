using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Library.Application.Services.Import;
using Microsoft.Extensions.Logging;
using AppStreamingLink     = Library.Application.Services.Import.StreamingLinkImportData;
using AppStreamingPlatform = Library.Domain.Enums.StreamingPlatform;

namespace Library.Infrastructure.Odesli;

public class OdesliService(HttpClient http, ILogger<OdesliService> logger)
    : IOdesliService
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static readonly IReadOnlyDictionary<string, AppStreamingPlatform> PlatformKeyMap =
        new Dictionary<string, AppStreamingPlatform>(StringComparer.OrdinalIgnoreCase)
        {
            ["spotify"]      = AppStreamingPlatform.Spotify,
            ["appleMusic"]   = AppStreamingPlatform.AppleMusic,
            ["youtube"]      = AppStreamingPlatform.YouTube,
            ["deezer"]       = AppStreamingPlatform.Deezer,
            ["soundcloud"]   = AppStreamingPlatform.SoundCloud,
            ["tidal"]        = AppStreamingPlatform.Tidal,
            ["amazonMusic"]  = AppStreamingPlatform.AmazonMusic,
            ["bandcamp"]     = AppStreamingPlatform.Bandcamp,
        };

    public async Task<List<AppStreamingLink>> GetLinksAsync(string knownUrl, CancellationToken ct = default)
    {
        try
        {
            string requestUrl = $"links?url={Uri.EscapeDataString(knownUrl)}";
            var response = await http.GetFromJsonAsync<OdesliResponse>(requestUrl, JsonOpts, ct);

            if (response?.LinksByPlatform is null)
                return [];

            var result = new List<AppStreamingLink>();

            foreach (var (key, entry) in response.LinksByPlatform)
            {
                if (!PlatformKeyMap.TryGetValue(key, out var platform)) continue;
                if (string.IsNullOrWhiteSpace(entry.Url)) continue;

                result.Add(new AppStreamingLink(platform, entry.Url));
            }

            return result;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Odesli lookup failed for URL '{Url}'", knownUrl);
            return [];
        }
    }

    private sealed class OdesliResponse
    {
        [JsonPropertyName("linksByPlatform")]
        public Dictionary<string, OdesliPlatformEntry>? LinksByPlatform { get; init; }
    }

    private sealed class OdesliPlatformEntry
    {
        [JsonPropertyName("url")]
        public string? Url { get; init; }
    }
}
