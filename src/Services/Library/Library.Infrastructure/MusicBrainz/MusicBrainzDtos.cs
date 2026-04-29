using System.Text.Json;
using System.Text.Json.Serialization;

namespace Library.Infrastructure.MusicBrainz;

internal sealed class FlexibleBoolConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType switch
        {
            JsonTokenType.True  => true,
            JsonTokenType.False => false,
            JsonTokenType.String => bool.TryParse(reader.GetString(), out bool v) && v,
            JsonTokenType.Number => reader.GetInt32() != 0,
            _ => false
        };

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) =>
        writer.WriteBooleanValue(value);
}

// ── Artist (Band) ────────────────────────────────────────────────────────────

public record MbArtistSearchResponse(
    [property: JsonPropertyName("artists")] List<MbArtist> Artists
);

public record MbArtist(
    [property: JsonPropertyName("id")]            string Id,
    [property: JsonPropertyName("name")]          string Name,
    [property: JsonPropertyName("disambiguation")] string? Disambiguation,
    [property: JsonPropertyName("country")]       string? Country,
    [property: JsonPropertyName("life-span")]     MbLifeSpan? LifeSpan,
    [property: JsonPropertyName("tags")]          List<MbTag>? Tags
);

public record MbLifeSpan(
    [property: JsonPropertyName("begin")] string? Begin,
    [property: JsonPropertyName("end")]   string? End,
    [property: JsonPropertyName("ended")][property: JsonConverter(typeof(FlexibleBoolConverter))] bool Ended
);

public record MbTag(
    [property: JsonPropertyName("name")]  string Name,
    [property: JsonPropertyName("count")] int    Count
);

// ── Artist detail (with releases) ───────────────────────────────────────────

public record MbArtistDetail(
    [property: JsonPropertyName("id")]              string Id,
    [property: JsonPropertyName("name")]            string Name,
    [property: JsonPropertyName("disambiguation")]  string? Disambiguation,
    [property: JsonPropertyName("country")]         string? Country,
    [property: JsonPropertyName("life-span")]       MbLifeSpan? LifeSpan,
    [property: JsonPropertyName("release-groups")]  List<MbReleaseGroup>? ReleaseGroups,
    [property: JsonPropertyName("tags")]            List<MbTag>? Tags
);

public record MbReleaseBrowseResult(
    [property: JsonPropertyName("releases")]      List<MbRelease> Releases,
    [property: JsonPropertyName("release-count")] int Count
);

// ── Release (Album) ──────────────────────────────────────────────────────────

public record MbRelease(
    [property: JsonPropertyName("id")]             string Id,
    [property: JsonPropertyName("title")]          string Title,
    [property: JsonPropertyName("date")]           string? Date,
    [property: JsonPropertyName("status")]         string? Status,
    [property: JsonPropertyName("release-group")] MbReleaseGroup? ReleaseGroup,
    [property: JsonPropertyName("media")]          List<MbMedia>? Media,
    [property: JsonPropertyName("label-info")]     List<MbLabelInfo>? LabelInfo
);

public record MbLabelInfo(
    [property: JsonPropertyName("label")] MbLabel? Label
);

public record MbLabel(
    [property: JsonPropertyName("id")]   string Id,
    [property: JsonPropertyName("name")] string Name
);

public record MbReleaseGroup(
    [property: JsonPropertyName("id")]                  string Id,
    [property: JsonPropertyName("title")]               string Title,
    [property: JsonPropertyName("primary-type")]        string? PrimaryType,
    [property: JsonPropertyName("secondary-types")]     List<string>? SecondaryTypes,
    [property: JsonPropertyName("first-release-date")]  string? FirstReleaseDate
);

// ── Media / Tracks ───────────────────────────────────────────────────────────

public record MbMedia(
    [property: JsonPropertyName("tracks")] List<MbTrack>? Tracks
);

public record MbTrack(
    [property: JsonPropertyName("number")]   string? Number,
    [property: JsonPropertyName("title")]    string Title,
    [property: JsonPropertyName("length")]   int? LengthMs
);

// ── Cover Art Archive ────────────────────────────────────────────────────────

public record CoverArtResponse(
    [property: JsonPropertyName("images")] List<CoverArtImage> Images
);

public record CoverArtImage(
    [property: JsonPropertyName("front")]      bool   Front,
    [property: JsonPropertyName("image")]      string Image,
    [property: JsonPropertyName("thumbnails")] CoverArtThumbnails Thumbnails
);

public record CoverArtThumbnails(
    [property: JsonPropertyName("large")]  string? Large,
    [property: JsonPropertyName("small")]  string? Small,
    [property: JsonPropertyName("250")]    string? S250,
    [property: JsonPropertyName("500")]    string? S500
);
