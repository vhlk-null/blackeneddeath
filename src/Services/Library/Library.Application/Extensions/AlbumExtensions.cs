namespace Library.Application.Extensions;

public static class AlbumExtensions
{
    public static AlbumDto ToAlbumDto(
        this Album album,
        IReadOnlyDictionary<BandId, Band> bands,
        IReadOnlyDictionary<GenreId, Genre> genres,
        IReadOnlyDictionary<CountryId, Country> countries,
        IReadOnlyDictionary<TrackId, Track> tracks) => new(
            album.Id.Value,
            album.Title,
            album.AlbumRelease.ReleaseYear,
            album.CoverUrl,
            album.Type,
            album.AlbumRelease.Format,
            album.LabelInfo?.Name,
            album.AlbumBands
                .Select(ab => bands[ab.BandId])
                .Select(b => new BandSummaryDto((Guid?)b.Id.Value, b.Name))
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
                .Select(ag => new GenreDto(genres[ag.GenreId].Id.Value, genres[ag.GenreId].Name, ag.IsPrimary))
                .ToList()
        );
}
