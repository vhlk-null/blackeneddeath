namespace Library.Application.Services.Albums.Queries.GetAlbumBySlug;

public class GetAlbumBySlugQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetAlbumBySlugQuery, GetAlbumBySlugResult>
{
    public async ValueTask<GetAlbumBySlugResult> Handle(GetAlbumBySlugQuery query, CancellationToken cancellationToken)
    {
        Album album = await context.Albums
                          .AsNoTracking()
                          .Include(a => a.AlbumBands)
                          .Include(a => a.AlbumGenres)
                          .Include(a => a.AlbumCountries)
                          .Include(a => a.AlbumTracks)
                          .Include(a => a.AlbumTags)
                          .Include(a => a.StreamingLinks)
                          .FirstOrDefaultAsync(a => a.Slug == query.Slug && (!query.ApprovedOnly || a.IsApproved), cancellationToken)
                      ?? throw new AlbumBySlugNotFoundException(query.Slug);

        AlbumId albumId = album.Id;

        List<BandId> bandIds = album.AlbumBands.Select(ab => ab.BandId).ToList();

        Dictionary<BandId, Band> bands = await context.Bands.AsNoTracking()
            .Where(b => bandIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id, cancellationToken);

        List<Album> discographyAlbums = await context.Albums.AsNoTracking()
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Where(a => (!query.ApprovedOnly || a.IsApproved) && a.AlbumBands.Any(ab => bandIds.Contains(ab.BandId)))
            .ToListAsync(cancellationToken);

        ILookup<BandId, Album> discographyByBand = discographyAlbums
            .SelectMany(a => a.AlbumBands.Select(ab => (ab.BandId, a)))
            .ToLookup(x => x.BandId, x => x.a);

        List<BandId> discographyCoArtistIds = discographyAlbums
            .SelectMany(a => a.AlbumBands.Select(ab => ab.BandId))
            .Except(bands.Keys)
            .Distinct()
            .ToList();

        if (discographyCoArtistIds.Count > 0)
        {
            Dictionary<BandId, Band> discographyCoArtists = await context.Bands.AsNoTracking()
                .Where(b => discographyCoArtistIds.Contains(b.Id))
                .ToDictionaryAsync(b => b.Id, cancellationToken);
            foreach (KeyValuePair<BandId, Band> kv in discographyCoArtists)
                bands[kv.Key] = kv.Value;
        }

        List<GenreId> genreIds = album.AlbumGenres.Select(ag => ag.GenreId).ToList();
        List<TagId> tagIds = album.AlbumTags.Select(at => at.TagId).ToList();

        List<GenreId> allGenreIds = genreIds
            .Concat(discographyAlbums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId)))
            .Distinct().ToList();

        Dictionary<GenreId, Genre> genres = (await context.GetAllGenresAsync(cancellationToken))
            .Where(g => allGenreIds.Contains(g.Id))
            .ToDictionary(g => g.Id);

        List<CountryId> allCountryIds = album.AlbumCountries.Select(ac => ac.CountryId)
            .Concat(discographyAlbums.SelectMany(a => a.AlbumCountries.Select(ac => ac.CountryId)))
            .Distinct().ToList();

        Dictionary<CountryId, Country> countries = (await context.GetAllCountriesAsync(cancellationToken))
            .Where(c => allCountryIds.Contains(c.Id))
            .ToDictionary(c => c.Id);

        Dictionary<TrackId, Track> tracks = await context.Tracks.AsNoTracking()
            .Where(t => album.AlbumTracks.Select(at => at.TrackId).Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);

        Dictionary<LabelId, Label> labels = album.LabelId != null
            ? (await context.GetAllLabelsAsync(cancellationToken))
                .Where(l => l.Id == album.LabelId)
                .ToDictionary(l => l.Id)
            : new Dictionary<LabelId, Label>();

        Dictionary<TagId, Tag> tags = tagIds.Count > 0
            ? (await context.GetAllTagsAsync(cancellationToken))
                .Where(t => tagIds.Contains(t.Id))
                .ToDictionary(t => t.Id)
            : new Dictionary<TagId, Tag>();

        AlbumDto albumDto = album.ToAlbumDto(bands, genres, countries, tracks, urlResolver, labels, tags, discographyByBand);

        List<VideoBandDto> videos = await context.VideoBands.AsNoTracking()
            .Where(vb => bandIds.Contains(vb.BandId))
            .Join(context.Bands, vb => vb.BandId, b => b.Id, (vb, b) => new { vb, BandName = b.Name })
            .OrderByDescending(x => x.vb.Year)
            .Select(x => new VideoBandDto(
                x.vb.Id.Value, x.vb.BandId.Value, x.BandName, x.vb.Name, x.vb.Year,
                x.vb.CountryId != null ? x.vb.CountryId.Value : null,
                x.vb.VideoType, x.vb.YoutubeLink, x.vb.Info))
            .ToListAsync(cancellationToken);

        return new GetAlbumBySlugResult(new AlbumDetailDto(
            albumDto.Id, albumDto.Title, albumDto.Slug, albumDto.ReleaseDate,
            albumDto.ReleaseMonth, albumDto.ReleaseDay,
            albumDto.CoverUrl, albumDto.Type, albumDto.Format, albumDto.Label,
            albumDto.Bands, albumDto.Countries, albumDto.StreamingLinks,
            albumDto.Tracks, albumDto.Genres, albumDto.Tags, albumDto.TotalDuration,
            albumDto.DiscographyGroups, albumDto.IsExplicit, videos));
    }
}
