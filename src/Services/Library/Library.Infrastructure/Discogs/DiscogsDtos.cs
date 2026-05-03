using System.Text.Json.Serialization;

namespace Library.Infrastructure.Discogs;

internal sealed class DiscogsSearchResponse
{
    [JsonPropertyName("results")]
    public List<DiscogsSearchResult> Results { get; init; } = [];
}

internal sealed class DiscogsSearchResult
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("title")]
    public string Title { get; init; } = null!;

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("cover_image")]
    public string? CoverImage { get; init; }

    [JsonPropertyName("genre")]
    public List<string>? Genre { get; init; }

    [JsonPropertyName("style")]
    public List<string>? Style { get; init; }

    [JsonPropertyName("country")]
    public string? Country { get; init; }

    [JsonPropertyName("uri")]
    public string? Uri { get; init; }
}

internal sealed class DiscogsArtist
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;

    [JsonPropertyName("profile")]
    public string? Profile { get; init; }

    [JsonPropertyName("images")]
    public List<DiscogsImage>? Images { get; init; }
}

internal sealed class DiscogsArtistReleasesResponse
{
    [JsonPropertyName("releases")]
    public List<DiscogsArtistRelease> Releases { get; init; } = [];

    [JsonPropertyName("pagination")]
    public DiscogsPagination? Pagination { get; init; }
}

internal sealed class DiscogsPagination
{
    [JsonPropertyName("pages")]
    public int Pages { get; init; }

    [JsonPropertyName("items")]
    public int Items { get; init; }
}

internal sealed class DiscogsArtistRelease
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("title")]
    public string Title { get; init; } = null!;

    [JsonPropertyName("year")]
    public int? Year { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("role")]
    public string? Role { get; init; }

    [JsonPropertyName("format")]
    public string? Format { get; init; }

    [JsonPropertyName("label")]
    public string? Label { get; init; }

    [JsonPropertyName("resource_url")]
    public string? ResourceUrl { get; init; }
}

internal sealed class DiscogsRelease
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("title")]
    public string Title { get; init; } = null!;

    [JsonPropertyName("year")]
    public int? Year { get; init; }

    [JsonPropertyName("released")]
    public string? Released { get; init; }

    [JsonPropertyName("labels")]
    public List<DiscogsLabelRef>? Labels { get; init; }

    [JsonPropertyName("genres")]
    public List<string>? Genres { get; init; }

    [JsonPropertyName("styles")]
    public List<string>? Styles { get; init; }

    [JsonPropertyName("tracklist")]
    public List<DiscogsTrack>? Tracklist { get; init; }

    [JsonPropertyName("images")]
    public List<DiscogsImage>? Images { get; init; }

    [JsonPropertyName("videos")]
    public List<DiscogsVideo>? Videos { get; init; }

    [JsonPropertyName("uri")]
    public string? Uri { get; init; }
}

internal sealed class DiscogsMasterRelease
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("title")]
    public string Title { get; init; } = null!;

    [JsonPropertyName("year")]
    public int? Year { get; init; }

    [JsonPropertyName("genres")]
    public List<string>? Genres { get; init; }

    [JsonPropertyName("styles")]
    public List<string>? Styles { get; init; }

    [JsonPropertyName("tracklist")]
    public List<DiscogsTrack>? Tracklist { get; init; }

    [JsonPropertyName("images")]
    public List<DiscogsImage>? Images { get; init; }

    [JsonPropertyName("main_release")]
    public int MainRelease { get; init; }

    [JsonPropertyName("videos")]
    public List<DiscogsVideo>? Videos { get; init; }

    [JsonPropertyName("uri")]
    public string? Uri { get; init; }
}

internal sealed class DiscogsVideo
{
    [JsonPropertyName("uri")]
    public string? Uri { get; init; }

    [JsonPropertyName("title")]
    public string? Title { get; init; }
}

internal sealed class DiscogsLabelRef
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}

internal sealed class DiscogsTrack
{
    [JsonPropertyName("position")]
    public string? Position { get; init; }

    [JsonPropertyName("type_")]
    public string? Type { get; init; }

    [JsonPropertyName("title")]
    public string Title { get; init; } = null!;

    [JsonPropertyName("duration")]
    public string? Duration { get; init; }
}

internal sealed class DiscogsImage
{
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("uri")]
    public string? Uri { get; init; }

    [JsonPropertyName("uri150")]
    public string? Uri150 { get; init; }
}
