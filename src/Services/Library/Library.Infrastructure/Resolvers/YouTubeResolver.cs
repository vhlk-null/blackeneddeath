using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Library.Application.Services.Import;
using Library.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Resolvers;

public class YouTubeResolver(HttpClient http, ILogger<YouTubeResolver> logger, string apiKey)
    : IStreamingLinkResolver
{
    public StreamingPlatform Platform => StreamingPlatform.YouTube;

    public async Task<string?> ResolveAsync(string artistName, string albumTitle, CancellationToken ct = default)
    {
        try
        {
            string term = Uri.EscapeDataString($"{artistName} {albumTitle} full album");
            string url = $"search?part=snippet&q={term}&type=video&maxResults=1&key={apiKey}";

            var response = await http.GetFromJsonAsync<YouTubeSearchResponse>(url, ct);
            string? videoId = response?.Items?.FirstOrDefault()?.Id?.VideoId;

            return videoId is null ? null : $"https://www.youtube.com/watch?v={videoId}";
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "YouTube lookup failed for '{Artist} - {Album}'", artistName, albumTitle);
            return null;
        }
    }

    private sealed class YouTubeSearchResponse
    {
        [JsonPropertyName("items")]
        public List<YouTubeSearchItem>? Items { get; init; }
    }

    private sealed class YouTubeSearchItem
    {
        [JsonPropertyName("id")]
        public YouTubeVideoId? Id { get; init; }
    }

    private sealed class YouTubeVideoId
    {
        [JsonPropertyName("videoId")]
        public string? VideoId { get; init; }
    }
}
