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

    public static AlbumDto ToAlbumDto(
        this Album album,
        IReadOnlyDictionary<BandId, Band> bands,
        IReadOnlyDictionary<GenreId, Genre> genres,
        IReadOnlyDictionary<CountryId, Country> countries,
        IReadOnlyDictionary<TrackId, Track> tracks,
        IStorageUrlResolver urlResolver,
        IReadOnlyDictionary<LabelId, Label> labels,
        IReadOnlyDictionary<TagId, Tag> tags,
        ILookup<BandId, Album> discographyByBand) => new(
            album.Id.Value,
            album.Title,
            album.Slug,
            album.AlbumRelease.ReleaseYear,
            urlResolver.Resolve(album.CoverUrl),
            album.Type,
            album.AlbumRelease.Format,
            album.LabelId != null && labels.TryGetValue(album.LabelId, out Label? label) ? label.ToLabelDto() : null,
            album.AlbumBands
                .OrderBy(ab => ab.Order)
                .Where(ab => bands.ContainsKey(ab.BandId))
                .Select(ab => bands[ab.BandId])
                .Select(b => new BandSummaryDto(
                    b.Id.Value,
                    b.Name,
                    b.Slug,
                    b.Status,
                    discographyByBand[b.Id]
                        .DistinctBy(a => a.Id)
                        .Where(a => a.Id != album.Id)
                        .OrderByDescending(a => a.AlbumRelease.ReleaseYear)
                        .SelectMany(a => a.AlbumBands
                            .Select(ab => bands.GetValueOrDefault(ab.BandId))
                            .Where(ab => ab is not null)
                            .Select(ab => new AlbumSummaryDto(
                                a.Id.Value, a.Title, a.Slug, a.AlbumRelease.ReleaseYear,
                                urlResolver.Resolve(a.CoverUrl), a.Type, a.AlbumRelease.Format,
                                a.AlbumGenres
                                    .Where(ag => genres.ContainsKey(ag.GenreId))
                                    .Select(ag => new GenreDto(genres[ag.GenreId].Id.Value, genres[ag.GenreId].Name, genres[ag.GenreId].Slug, ag.IsPrimary))
                                    .ToList(),
                                a.AlbumCountries
                                    .Where(ac => countries.ContainsKey(ac.CountryId))
                                    .Select(ac => new CountryDto(countries[ac.CountryId].Id.Value, countries[ac.CountryId].Name, countries[ac.CountryId].Code))
                                    .ToList(),
                                ab!.Id.Value,
                                ab.Name)))
                        .ToList()))
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
                album.AlbumTracks.Select(at => tracks.TryGetValue(at.TrackId, out Track? t) ? t.Duration : null))
        );
}
