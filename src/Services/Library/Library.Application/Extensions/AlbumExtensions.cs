namespace Library.Application.Extensions;

public static class AlbumExtensions
{
    public static AlbumDto ToAlbumDto(
        this Album album,
        IReadOnlyDictionary<BandId, Band> bands,
        IReadOnlyDictionary<GenreId, Genre> genres,
        IReadOnlyDictionary<CountryId, Country> countries,
        IReadOnlyDictionary<TrackId, Track> tracks,
        IStorageUrlResolver urlResolver,
        IReadOnlyDictionary<LabelId, Label> labels,
        IReadOnlyDictionary<TagId, Tag> tags) => new(
            album.Id.Value,
            album.Title,
            album.Slug,
            album.AlbumRelease.ReleaseYear,
            urlResolver.Resolve(album.CoverUrl),
            album.Type,
            album.AlbumRelease.Format,
            album.LabelId != null && labels.TryGetValue(album.LabelId, out var label) ? label.ToLabelDto() : null,
            album.AlbumBands
                .Select(ab => bands[ab.BandId])
                .Select(b => new BandSummaryDto((Guid?)b.Id.Value, b.Name, b.Slug))
                .ToList(),
            album.AlbumCountries
                .Select(ac => countries[ac.CountryId])
                .Select(c => new CountryDto(c.Id.Value, c.Name, c.Code))
                .ToList(),
            album.StreamingLinks
                .Select(sl => new StreamingLinkDto(sl.Platform, sl.EmbedCode))
                .ToList(),
            album.AlbumTracks
                .OrderBy(at => at.TrackNumber)
                .Select(at => new TrackDto(tracks[at.TrackId].Id.Value, tracks[at.TrackId].Title, at.TrackNumber))
                .ToList(),
            album.AlbumGenres
                .Select(ag => new GenreDto(genres[ag.GenreId].Id.Value, genres[ag.GenreId].Name, genres[ag.GenreId].Slug, ag.IsPrimary))
                .ToList(),
            album.AlbumTags
                .Where(at => tags.ContainsKey(at.TagId))
                .Select(at => new TagDto(tags[at.TagId].Id.Value, tags[at.TagId].Name))
                .ToList()
        );
}
