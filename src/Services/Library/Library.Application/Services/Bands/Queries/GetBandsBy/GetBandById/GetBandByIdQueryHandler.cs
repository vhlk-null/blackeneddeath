namespace Library.Application.Services.Bands.Queries.GetBandsBy.GetBandById;

public class GetBandByIdQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetBandByIdQuery, GetBandByIdResult>
{
    public async ValueTask<GetBandByIdResult> Handle(GetBandByIdQuery query, CancellationToken cancellationToken)
    {
        var bandId = BandId.Of(query.Id);

        var band = await context.Bands
            .AsNoTracking()
            .Include(b => b.BandCountries)
            .Include(b => b.BandGenres)
            .Include(b => b.VideoBands)
            .FirstOrDefaultAsync(b => b.Id == bandId, cancellationToken)
            ?? throw new BandNotFoundException(query.Id);

        var albumIds = await context.AlbumBands.AsNoTracking()
            .Where(ab => ab.BandId == band.Id)
            .Select(ab => ab.AlbumId)
            .ToListAsync(cancellationToken);

        var countryIds = band.BandCountries.Select(bc => bc.CountryId).ToList();
        var genreIds = band.BandGenres.Select(bg => bg.GenreId).ToList();

        var albums = await context.Albums.AsNoTracking()
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Where(a => albumIds.Contains(a.Id))
            .ToListAsync(cancellationToken);

        // Similar bands: same genre, not this band, random 3
        var similarBands = await context.Bands.AsNoTracking()
            .Include(b => b.BandGenres)
            .Include(b => b.BandCountries)
            .Where(b => b.Id != bandId)
            .Where(b => b.BandGenres.Any(bg => genreIds.Contains(bg.GenreId)))
            .OrderBy(_ => EF.Functions.Random())
            .Take(3)
            .ToListAsync(cancellationToken);

        var albumGenreIds = albums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId));
        var similarBandGenreIds = similarBands.SelectMany(b => b.BandGenres.Select(bg => bg.GenreId));

        var allGenreIds = genreIds
            .Concat(albumGenreIds)
            .Concat(similarBandGenreIds)
            .Distinct().ToList();

        var allCountryIds = countryIds
            .Concat(albums.SelectMany(a => a.AlbumCountries.Select(ac => ac.CountryId)))
            .Distinct().ToList();

        var countries = await context.Countries.AsNoTracking()
            .Where(c => allCountryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken);

        var genres = await context.Genres.AsNoTracking()
            .Where(g => allGenreIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        var albumsByBand = albums.ToLookup(_ => band.Id);

        var bandDto = band.ToBandDto(countries, genres, albumsByBand, urlResolver);

        var similarBandCountryIds = similarBands
            .SelectMany(b => b.BandCountries.Select(bc => bc.CountryId))
            .Except(allCountryIds)
            .Distinct().ToList();

        foreach (var entry in await context.Countries.AsNoTracking()
            .Where(c => similarBandCountryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken))
        {
            countries[entry.Key] = entry.Value;
        }

        var similarBandDtos = similarBands.Select(b => new BandCardDto(
            b.Id.Value,
            b.Name,
            b.Slug,
            urlResolver.Resolve(b.LogoUrl),
            b.Status,
            b.Activity.FormedYear,
            b.Activity.DisbandedYear,
            b.BandGenres.Where(bg => bg.IsPrimary && genres.ContainsKey(bg.GenreId))
                .Select(bg => new GenreDto(genres[bg.GenreId].Id.Value, genres[bg.GenreId].Name, genres[bg.GenreId].Slug, bg.IsPrimary))
                .FirstOrDefault(),
            b.BandCountries
                .Where(bc => countries.ContainsKey(bc.CountryId))
                .Select(bc => new CountryDto(countries[bc.CountryId].Id.Value, countries[bc.CountryId].Name, countries[bc.CountryId].Code))
                .ToList())).ToList();

        var videos = band.VideoBands
            .OrderByDescending(vb => vb.Year)
            .Select(vb => new VideoBandDto(
                vb.Id.Value, vb.BandId.Value, band.Name, vb.Name, vb.Year,
                vb.CountryId != null ? vb.CountryId.Value : null,
                vb.VideoType, vb.YoutubeLink, vb.Info))
            .ToList();

        return new GetBandByIdResult(new BandDetailDto(
            bandDto.Id, bandDto.Name, bandDto.Slug, bandDto.Bio,
            bandDto.LogoUrl, bandDto.FormedYear, bandDto.DisbandedYear,
            bandDto.Status, bandDto.Countries, bandDto.Albums, bandDto.Genres,
            bandDto.Facebook, bandDto.Youtube, bandDto.Instagram,
            bandDto.Twitter, bandDto.Website,
            videos, similarBandDtos));
    }
}
