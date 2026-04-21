namespace Library.Application.Extensions;

public static class AlbumExtensions
{
    private static string? ComputeTotalDuration(IEnumerable<string?> durations)
    {
        TimeSpan total = TimeSpan.Zero;
        bool hasAny = false;

        foreach (string? raw in durations)
        {
            if (string.IsNullOrWhiteSpace(raw)) continue;

            string[] parts = raw.Trim().Split(':');
            TimeSpan? parsed = parts.Length switch
            {
                2 when int.TryParse(parts[0], out int m) && int.TryParse(parts[1], out int s)
                    => (TimeSpan?)TimeSpan.FromSeconds(m * 60 + s),
                3 when int.TryParse(parts[0], out int h) && int.TryParse(parts[1], out int m) && int.TryParse(parts[2], out int s)
                    => TimeSpan.FromSeconds(h * 3600 + m * 60 + s),
                _ => null
            };

            if (parsed is null) continue;
            total += parsed.Value;
            hasAny = true;
        }

        if (!hasAny) return null;

        return total.TotalHours >= 1
            ? $"{(int)total.TotalHours}:{total.Minutes:D2}:{total.Seconds:D2}"
            : $"{(int)total.TotalMinutes}:{total.Seconds:D2}";
    }

    private static AlbumSummaryDto ToAlbumSummaryDto(
        Album a,
        Band primaryBand,
        IReadOnlyDictionary<GenreId, Genre> genres,
        IReadOnlyDictionary<CountryId, Country> countries,
        IStorageUrlResolver urlResolver) => new(
            a.Id.Value,
            a.Title,
            a.Slug ?? string.Empty,
            a.AlbumRelease.ReleaseYear,
            urlResolver.Resolve(a.CoverUrl),
            a.Type,
            a.AlbumRelease.Format,
            a.AlbumGenres
                .Where(ag => genres.ContainsKey(ag.GenreId))
                .Select(ag => new GenreDto(genres[ag.GenreId].Id.Value, genres[ag.GenreId].Name, genres[ag.GenreId].Slug, ag.IsPrimary))
                .ToList(),
            a.AlbumCountries
                .Where(ac => countries.ContainsKey(ac.CountryId))
                .Select(ac => new CountryDto(countries[ac.CountryId].Id.Value, countries[ac.CountryId].Name, countries[ac.CountryId].Code))
                .ToList(),
            primaryBand.Id.Value,
            primaryBand.Name,
            a.IsExplicit);

    // Builds a deduplicated discography group list for all bands of an album combined.
    // Each album appears exactly once — either under a solo band group or a collab group.
    private static List<DiscographyGroupDto> BuildDiscographyGroups(
        List<BandId> rootBandIds,
        AlbumId excludeAlbumId,
        ILookup<BandId, Album> discographyByBand,
        IReadOnlyDictionary<BandId, Band> bands,
        IReadOnlyDictionary<GenreId, Genre> genres,
        IReadOnlyDictionary<CountryId, Country> countries,
        IStorageUrlResolver urlResolver)
    {
        // Collect all discography albums across all root bands, deduplicated
        List<Album> allAlbums = rootBandIds
            .SelectMany(id => discographyByBand[id])
            .DistinctBy(a => a.Id)
            .Where(a => a.Id != excludeAlbumId)
            .OrderByDescending(a => a.AlbumRelease.ReleaseYear)
            .ToList();

        if (allAlbums.Count == 0)
            return [];

        // Group by unique participant combination (sorted bandIds joined by "|")
        Dictionary<string, List<Album>> byCombo = new();

        foreach (Album album in allAlbums)
        {
            string key = string.Join("|",
                album.AlbumBands.Select(ab => ab.BandId.Value).OrderBy(g => g));

            if (!byCombo.TryGetValue(key, out List<Album>? bucket))
            {
                bucket = [];
                byCombo[key] = bucket;
            }
            bucket.Add(album);
        }

        // Index of each root band by its position in the album's band order
        Dictionary<BandId, int> rootBandOrder = rootBandIds
            .Select((id, i) => (id, i))
            .ToDictionary(x => x.id, x => x.i);

        List<DiscographyGroupDto> soloGroups = [];
        List<DiscographyGroupDto> collabGroups = [];

        foreach ((string key, List<Album> groupAlbums) in byCombo.OrderByDescending(kv =>
                     kv.Value.Max(a => a.AlbumRelease.ReleaseYear)))
        {
            List<BandId> participantIds = key.Split('|')
                .Select(s => BandId.Of(Guid.Parse(s)))
                .ToList();

            bool isSolo = participantIds.Count == 1;

            if (isSolo)
            {
                BandId soloId = participantIds[0];
                if (!bands.TryGetValue(soloId, out Band? soloBand)) continue;

                soloGroups.Add(new DiscographyGroupDto(
                    soloBand.Id.Value,
                    soloBand.Name,
                    soloBand.Slug,
                    soloBand.Name,
                    groupAlbums
                        .Select(a => ToAlbumSummaryDto(a, soloBand, genres, countries, urlResolver))
                        .ToList()));
            }
            else
            {
                List<Band> participants = participantIds
                    .Where(bands.ContainsKey)
                    .Select(id => bands[id])
                    .ToList();

                string label = string.Join(" & ", participants.Select(b => b.Name));

                Band primaryBand = participantIds
                    .Where(rootBandIds.Contains)
                    .Where(bands.ContainsKey)
                    .Select(id => bands[id])
                    .FirstOrDefault() ?? participants[0];

                collabGroups.Add(new DiscographyGroupDto(
                    null,
                    null,
                    null,
                    label,
                    groupAlbums
                        .Select(a => ToAlbumSummaryDto(a, primaryBand, genres, countries, urlResolver))
                        .ToList()));
            }
        }

        // Solo groups sorted by band order in the album, collab groups after
        return soloGroups
            .OrderBy(g => g.BandId.HasValue && rootBandOrder.TryGetValue(BandId.Of(g.BandId.Value), out int idx) ? idx : int.MaxValue)
            .Concat(collabGroups)
            .ToList();
    }

    public static AlbumDto ToAlbumDto(
        this Album album,
        IReadOnlyDictionary<BandId, Band> bands,
        IReadOnlyDictionary<GenreId, Genre> genres,
        IReadOnlyDictionary<CountryId, Country> countries,
        IReadOnlyDictionary<TrackId, Track> tracks,
        IStorageUrlResolver urlResolver,
        IReadOnlyDictionary<LabelId, Label> labels,
        IReadOnlyDictionary<TagId, Tag> tags,
        ILookup<BandId, Album> discographyByBand)
    {
        List<BandId> rootBandIds = album.AlbumBands
            .OrderBy(ab => ab.Order)
            .Where(ab => bands.ContainsKey(ab.BandId))
            .Select(ab => ab.BandId)
            .ToList();

        return new AlbumDto(
            album.Id.Value,
            album.Title,
            album.Slug,
            album.AlbumRelease.ReleaseYear,
            urlResolver.Resolve(album.CoverUrl),
            album.Type,
            album.AlbumRelease.Format,
            album.LabelId != null && labels.TryGetValue(album.LabelId, out Label? label) ? label.ToLabelDto() : null,
            rootBandIds
                .Select(id => bands[id])
                .Select(b => new BandSummaryDto(b.Id.Value, b.Name, b.Slug, b.Status))
                .ToList(),
            album.AlbumCountries
                .Where(ac => countries.ContainsKey(ac.CountryId))
                .Select(ac => countries[ac.CountryId])
                .Select(c => new CountryDto(c.Id.Value, c.Name, c.Code))
                .ToList(),
            album.StreamingLinks
                .Select(sl => new StreamingLinkDto(sl.Platform, sl.EmbedCode))
                .ToList(),
            album.AlbumTracks
                .Where(at => tracks.ContainsKey(at.TrackId))
                .OrderBy(at => at.TrackNumber)
                .Select(at => new TrackDto(tracks[at.TrackId].Id.Value, tracks[at.TrackId].Title, at.TrackNumber, tracks[at.TrackId].Duration))
                .ToList(),
            album.AlbumGenres
                .Where(ag => genres.ContainsKey(ag.GenreId))
                .Select(ag => new GenreDto(genres[ag.GenreId].Id.Value, genres[ag.GenreId].Name, genres[ag.GenreId].Slug, ag.IsPrimary))
                .ToList(),
            album.AlbumTags
                .Where(at => tags.ContainsKey(at.TagId))
                .Select(at => new TagDto(tags[at.TagId].Id.Value, tags[at.TagId].Name))
                .ToList(),
            ComputeTotalDuration(
                album.AlbumTracks.Select(at => tracks.TryGetValue(at.TrackId, out Track? t) ? t.Duration : null)),
            BuildDiscographyGroups(rootBandIds, album.Id, discographyByBand, bands, genres, countries, urlResolver),
            album.IsExplicit);
    }
}
