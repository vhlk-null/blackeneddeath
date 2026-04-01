namespace Library.Application.Extensions;

public static class BandExtensions
{
    public static BandDto ToBandDto(
        this Band band,
        IReadOnlyDictionary<CountryId, Country> countries,
        IReadOnlyDictionary<GenreId, Genre> genres,
        ILookup<BandId, Album> albumsByBand,
        IStorageUrlResolver urlResolver) => new(
            band.Id.Value,
            band.Name,
            band.Slug,
            band.Bio,
            urlResolver.Resolve(band.LogoUrl),
            band.Activity.FormedYear,
            band.Activity.DisbandedYear,
            band.Status,
            band.BandCountries
                .Where(bc => countries.ContainsKey(bc.CountryId))
                .Select(bc => countries[bc.CountryId])
                .Select(c => new CountryDto(c.Id.Value, c.Name, c.Code))
                .ToList(),
            albumsByBand[band.Id]
                .DistinctBy(a => a.Id)
                .Select(a => new AlbumSummaryDto(a.Id.Value, a.Title, a.Slug, a.AlbumRelease.ReleaseYear, urlResolver.Resolve(a.CoverUrl), a.Type, a.AlbumRelease.Format,
                    a.AlbumGenres.Where(ag => genres.ContainsKey(ag.GenreId))
                        .Select(ag => new GenreDto(genres[ag.GenreId].Id.Value, genres[ag.GenreId].Name, genres[ag.GenreId].Slug, ag.IsPrimary))
                        .ToList(),
                    a.AlbumCountries.Where(ac => countries.ContainsKey(ac.CountryId))
                        .Select(ac => new CountryDto(countries[ac.CountryId].Id.Value, countries[ac.CountryId].Name, countries[ac.CountryId].Code))
                        .ToList()))
                .ToList(),
            band.BandGenres
                .Where(bg => genres.ContainsKey(bg.GenreId))
                .OrderByDescending(bg => bg.IsPrimary)
                .Select(bg => new GenreDto(genres[bg.GenreId].Id.Value, genres[bg.GenreId].Name, genres[bg.GenreId].Slug, bg.IsPrimary))
                .ToList(),
            band.Facebook,
            band.Youtube,
            band.Instagram,
            band.Twitter,
            band.Website
        );
}
