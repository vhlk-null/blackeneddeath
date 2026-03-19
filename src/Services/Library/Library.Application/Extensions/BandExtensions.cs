namespace Library.Application.Extensions;

public static class BandExtensions
{
    public static BandDto ToBandDto(
        this Band band,
        IReadOnlyDictionary<CountryId, Country> countries,
        IReadOnlyDictionary<GenreId, Genre> genres,
        ILookup<BandId, Album> albumsByBand) => new(
            band.Id.Value,
            band.Name,
            band.Slug,
            band.Bio,
            band.LogoUrl,
            band.Activity.FormedYear,
            band.Activity.DisbandedYear,
            band.Status,
            band.BandCountries
                .Select(bc => countries[bc.CountryId])
                .Select(c => new CountryDto(c.Id.Value, c.Name, c.Code))
                .ToList(),
            albumsByBand[band.Id]
                .Select(a => new AlbumSummaryDto(a.Id.Value, a.Title, a.Slug, a.AlbumRelease.ReleaseYear, a.CoverUrl, a.Type, a.AlbumRelease.Format))
                .ToList(),
            band.BandGenres
                .Select(bg => new GenreDto(genres[bg.GenreId].Id.Value, genres[bg.GenreId].Name, genres[bg.GenreId].Slug, bg.IsPrimary))
                .ToList()
        );
}
