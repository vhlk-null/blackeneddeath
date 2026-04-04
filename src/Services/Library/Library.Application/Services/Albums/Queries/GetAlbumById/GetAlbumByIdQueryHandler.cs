namespace Library.Application.Services.Albums.Queries.GetAlbumById;

public class GetAlbumByIdQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver) : BuildingBlocks.CQRS.IQueryHandler<GetAlbumByIdQuery, GetAlbumByIdResult>
{
    public async ValueTask<GetAlbumByIdResult> Handle(GetAlbumByIdQuery query, CancellationToken cancellationToken)
    {
        var albumId = AlbumId.Of(query.Id);

        var album = await context.Albums
            .AsNoTracking()
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Include(a => a.AlbumTracks)
            .Include(a => a.AlbumTags)
            .Include(a => a.StreamingLinks)
            .FirstOrDefaultAsync(a => a.Id == albumId, cancellationToken)
            ?? throw new AlbumNotFoundException(query.Id);

        var bandIds = album.AlbumBands.Select(ab => ab.BandId).ToList();

        var bands = await context.Bands.AsNoTracking()
            .Where(b => bandIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id, cancellationToken);

        var discographyAlbums = await context.Albums.AsNoTracking()
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Where(a => a.AlbumBands.Any(ab => bandIds.Contains(ab.BandId)))
            .ToListAsync(cancellationToken);

        var discographyByBand = discographyAlbums
            .SelectMany(a => a.AlbumBands.Select(ab => (ab.BandId, a)))
            .ToLookup(x => x.BandId, x => x.a);

        var genreIds = album.AlbumGenres.Select(ag => ag.GenreId).ToList();

        // Similar albums: same genre, exclude current + discography, random 4
        var excludeAlbumIds = discographyAlbums.Select(a => a.Id).Append(albumId).Distinct().ToHashSet();

        var similarAlbums = await context.Albums.AsNoTracking()
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Where(a => !excludeAlbumIds.Contains(a.Id))
            .Where(a => a.AlbumGenres.Any(ag => genreIds.Contains(ag.GenreId)))
            .OrderBy(_ => EF.Functions.Random())
            .Take(4)
            .ToListAsync(cancellationToken);

        var allGenreIds = genreIds
            .Concat(discographyAlbums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId)))
            .Concat(similarAlbums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId)))
            .Distinct().ToList();

        var genres = await context.Genres.AsNoTracking()
            .Where(g => allGenreIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        var allCountryIds = album.AlbumCountries.Select(ac => ac.CountryId)
            .Concat(discographyAlbums.SelectMany(a => a.AlbumCountries.Select(ac => ac.CountryId)))
            .Concat(similarAlbums.SelectMany(a => a.AlbumCountries.Select(ac => ac.CountryId)))
            .Distinct().ToList();

        var countries = await context.Countries.AsNoTracking()
            .Where(c => allCountryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken);

        var tracks = await context.Tracks.AsNoTracking()
            .Where(t => album.AlbumTracks.Select(at => at.TrackId).Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);

        var labels = album.LabelId != null
            ? await context.Labels.AsNoTracking()
                .Where(l => l.Id == album.LabelId)
                .ToDictionaryAsync(l => l.Id, cancellationToken)
            : new Dictionary<LabelId, Label>();

        var tagIds = album.AlbumTags.Select(at => at.TagId).ToList();
        var tags = tagIds.Count > 0
            ? await context.Tags.AsNoTracking()
                .Where(t => tagIds.Contains(t.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken)
            : new Dictionary<TagId, Tag>();

        var albumDto = album.ToAlbumDto(bands, genres, countries, tracks, urlResolver, labels, tags, discographyByBand);

        var similarAlbumDtos = similarAlbums.Select(a => new AlbumSummaryDto(
            a.Id.Value, a.Title, a.Slug, a.AlbumRelease.ReleaseYear,
            urlResolver.Resolve(a.CoverUrl), a.Type, a.AlbumRelease.Format,
            a.AlbumGenres
                .Where(ag => genres.ContainsKey(ag.GenreId))
                .Select(ag => new GenreDto(genres[ag.GenreId].Id.Value, genres[ag.GenreId].Name, genres[ag.GenreId].Slug, ag.IsPrimary))
                .ToList(),
            a.AlbumCountries
                .Where(ac => countries.ContainsKey(ac.CountryId))
                .Select(ac => new CountryDto(countries[ac.CountryId].Id.Value, countries[ac.CountryId].Name, countries[ac.CountryId].Code))
                .ToList())).ToList();

        var videos = await context.VideoBands.AsNoTracking()
            .Where(vb => bandIds.Contains(vb.BandId))
            .OrderByDescending(vb => vb.Year)
            .Select(vb => new VideoBandDto(
                vb.Id.Value, vb.BandId.Value, vb.Name, vb.Year,
                vb.CountryId != null ? vb.CountryId.Value : null,
                vb.VideoType, vb.YoutubeLink, vb.Info))
            .ToListAsync(cancellationToken);

        return new GetAlbumByIdResult(new AlbumDetailDto(
            albumDto.Id, albumDto.Title, albumDto.Slug, albumDto.ReleaseDate,
            albumDto.CoverUrl, albumDto.Type, albumDto.Format, albumDto.Label,
            albumDto.Bands, albumDto.Countries, albumDto.StreamingLinks,
            albumDto.Tracks, albumDto.Genres, albumDto.Tags, albumDto.TotalDuration,
            videos, similarAlbumDtos));
    }
}
