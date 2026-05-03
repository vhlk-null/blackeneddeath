using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Library.Application.Services.Import;
using Library.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Resolvers;

public class AppleMusicResolver(HttpClient http, ILogger<AppleMusicResolver> logger)
    : IStreamingLinkResolver
{
    public StreamingPlatform Platform => StreamingPlatform.AppleMusic;

    public async Task<string?> ResolveAsync(string artistName, string albumTitle, CancellationToken ct = default)
    {
        try
        {
            string term = Uri.EscapeDataString($"{artistName} {albumTitle}");
            var response = await http.GetFromJsonAsync<ItunesSearchResponse>(
                $"search?term={term}&entity=album&limit=5", ct);

            int resultCount = response?.Results?.Count ?? 0;
            logger.LogInformation("  → Apple Music search '{Artist} - {Album}': {Count} result(s)", artistName, albumTitle, resultCount);

            var match = response?.Results?.FirstOrDefault(r =>
                r.WrapperType == "collection" &&
                r.CollectionType == "Album" &&
                string.Equals(r.ArtistName, artistName, StringComparison.OrdinalIgnoreCase));

            match ??= response?.Results?.FirstOrDefault(r => r.WrapperType == "collection");

            return match?.CollectionViewUrl;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Apple Music lookup failed for '{Artist} - {Album}'", artistName, albumTitle);
            return null;
        }
    }

    private sealed class ItunesSearchResponse
    {
        [JsonPropertyName("results")]
        public List<ItunesAlbumResult>? Results { get; init; }
    }

    private sealed class ItunesAlbumResult
    {
        [JsonPropertyName("wrapperType")]
        public string? WrapperType { get; init; }

        [JsonPropertyName("collectionType")]
        public string? CollectionType { get; init; }

        [JsonPropertyName("artistName")]
        public string? ArtistName { get; init; }

        [JsonPropertyName("collectionViewUrl")]
        public string? CollectionViewUrl { get; init; }
    }
}
