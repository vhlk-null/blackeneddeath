using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Library.Application.Services.Import;
using Library.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Resolvers;

public class DeezerResolver(HttpClient http, ILogger<DeezerResolver> logger)
    : IStreamingLinkResolver
{
    public StreamingPlatform Platform => StreamingPlatform.Deezer;

    public async Task<string?> ResolveAsync(string artistName, string albumTitle, CancellationToken ct = default)
    {
        try
        {
            string query = Uri.EscapeDataString($"artist:\"{artistName}\" album:\"{albumTitle}\"");
            var response = await http.GetFromJsonAsync<DeezerSearchResponse>(
                $"search/album?q={query}&limit=5", ct);

            var match = response?.Data?.FirstOrDefault(r =>
                string.Equals(r.Artist?.Name, artistName, StringComparison.OrdinalIgnoreCase));

            match ??= response?.Data?.FirstOrDefault();

            if (match is null) return null;

            return $"https://www.deezer.com/album/{match.Id}";
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Deezer lookup failed for '{Artist} - {Album}'", artistName, albumTitle);
            return null;
        }
    }

    private sealed class DeezerSearchResponse
    {
        [JsonPropertyName("data")]
        public List<DeezerAlbum>? Data { get; init; }
    }

    private sealed class DeezerAlbum
    {
        [JsonPropertyName("id")]
        public long Id { get; init; }

        [JsonPropertyName("artist")]
        public DeezerArtist? Artist { get; init; }
    }

    private sealed class DeezerArtist
    {
        [JsonPropertyName("name")]
        public string? Name { get; init; }
    }
}
