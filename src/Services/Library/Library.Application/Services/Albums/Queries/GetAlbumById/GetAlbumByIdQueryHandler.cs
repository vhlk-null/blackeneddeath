namespace Library.Application.Services.Albums.Queries.GetAlbumById;

public class GetAlbumByIdQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver) : BuildingBlocks.CQRS.IQueryHandler<GetAlbumByIdQuery, GetAlbumByIdResult>
{
    public async ValueTask<GetAlbumByIdResult> Handle(GetAlbumByIdQuery query, CancellationToken cancellationToken)
    {
        AlbumId albumId = AlbumId.Of(query.Id);

        Album album = await context.Albums
                          .AsNoTracking()
                          .Include(a => a.AlbumBands)
                          .Include(a => a.AlbumGenres)
                          .Include(a => a.AlbumCountries)
                          .Include(a => a.AlbumTracks)
                          .Include(a => a.AlbumTags)
                          .Include(a => a.StreamingLinks)
                          .FirstOrDefaultAsync(a => a.Id == albumId && (!query.ApprovedOnly || a.IsApproved), cancellationToken)
                      ?? throw new AlbumNotFoundException(query.Id);

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

        HashSet<AlbumId> excludeAlbumIds = discographyAlbums.Select(a => a.Id).Append(albumId).Distinct().ToHashSet();

        IQueryable<Album> similarQuery = context.Albums.AsNoTracking()
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Include(a => a.AlbumBands)
            .Where(a => (!query.ApprovedOnly || a.IsApproved) && !excludeAlbumIds.Contains(a.Id))
            .Where(a => a.AlbumGenres.Any(ag => genreIds.Contains(ag.GenreId))
                     || (tagIds.Count > 0 && a.AlbumTags.Any(at => tagIds.Contains(at.TagId))))
            .OrderBy(_ => EF.Functions.Random());

        int similarTotalCount = await similarQuery.CountAsync(cancellationToken);
        List<Album> similarAlbums = await similarQuery
            .Skip((query.SimilarPageNumber - 1) * query.SimilarPageSize)
            .Take(query.SimilarPageSize)
            .ToListAsync(cancellationToken);

        List<BandId> similarBandIds = similarAlbums
            .SelectMany(a => a.AlbumBands.Select(ab => ab.BandId))
            .Except(bands.Keys)
            .Distinct()
            .ToList();

        if (similarBandIds.Count > 0)
        {
            Dictionary<BandId, Band> similarBands = await context.Bands.AsNoTracking()
                .Where(b => similarBandIds.Contains(b.Id))
                .ToDictionaryAsync(b => b.Id, cancellationToken);
            foreach (KeyValuePair<BandId, Band> kv in similarBands)
                bands[kv.Key] = kv.Value;
        }

        List<GenreId> allGenreIds = genreIds
            .Concat(discographyAlbums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId)))
            .Concat(similarAlbums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId)))
            .Distinct().ToList();

        Dictionary<GenreId, Genre> genres = await context.Genres.AsNoTracking()
            .Where(g => allGenreIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        List<CountryId> allCountryIds = album.AlbumCountries.Select(ac => ac.CountryId)
            .Concat(discographyAlbums.SelectMany(a => a.AlbumCountries.Select(ac => ac.CountryId)))
            .Concat(similarAlbums.SelectMany(a => a.AlbumCountries.Select(ac => ac.CountryId)))
            .Distinct().ToList();

        Dictionary<CountryId, Country> countries = await context.Countries.AsNoTracking()
            .Where(c => allCountryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken);

        Dictionary<TrackId, Track> tracks = await context.Tracks.AsNoTracking()
            .Where(t => album.AlbumTracks.Select(at => at.TrackId).Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);

        Dictionary<LabelId, Label> labels = album.LabelId != null
            ? await context.Labels.AsNoTracking()
                .Where(l => l.Id == album.LabelId)
                .ToDictionaryAsync(l => l.Id, cancellationToken)
            : new Dictionary<LabelId, Label>();

        Dictionary<TagId, Tag> tags = tagIds.Count > 0
            ? await context.Tags.AsNoTracking()
                .Where(t => tagIds.Contains(t.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken)
            : new Dictionary<TagId, Tag>();

        AlbumDto albumDto = album.ToAlbumDto(bands, genres, countries, tracks, urlResolver, labels, tags, discographyByBand);

        List<AlbumSummaryDto> similarAlbumItems = similarAlbums.Select(a =>
        {
            Band? primaryBand = a.AlbumBands
                .Select(ab => bands.GetValueOrDefault(ab.BandId))
                .FirstOrDefault(b => b is not null);
            return new AlbumSummaryDto(
                a.Id.Value, a.Title, a.Slug, a.AlbumRelease.ReleaseYear,
                a.AlbumRelease.ReleaseMonth, a.AlbumRelease.ReleaseDay,
                urlResolver.Resolve(a.CoverUrl), a.Type, a.AlbumRelease.Format,
                a.AlbumGenres
                    .Where(ag => genres.ContainsKey(ag.GenreId))
                    .Select(ag => new GenreDto(genres[ag.GenreId].Id.Value, genres[ag.GenreId].Name, genres[ag.GenreId].Slug, ag.IsPrimary))
                    .ToList(),
                a.AlbumCountries
                    .Where(ac => countries.ContainsKey(ac.CountryId))
                    .Select(ac => new CountryDto(countries[ac.CountryId].Id.Value, countries[ac.CountryId].Name, countries[ac.CountryId].Code))
                    .ToList(),
                primaryBand?.Id.Value ?? Guid.Empty,
                primaryBand?.Name ?? string.Empty,
                a.IsExplicit);
        }).ToList();

        PaginatedResult<AlbumSummaryDto> similarAlbumDtos = new(query.SimilarPageNumber, query.SimilarPageSize, similarTotalCount, similarAlbumItems);

        List<VideoBandDto> videos = await context.VideoBands.AsNoTracking()
            .Where(vb => bandIds.Contains(vb.BandId))
            .Join(context.Bands, vb => vb.BandId, b => b.Id, (vb, b) => new { vb, BandName = b.Name })
            .OrderByDescending(x => x.vb.Year)
            .Select(x => new VideoBandDto(
                x.vb.Id.Value, x.vb.BandId.Value, x.BandName, x.vb.Name, x.vb.Year,
                x.vb.CountryId != null ? x.vb.CountryId.Value : null,
                x.vb.VideoType, x.vb.YoutubeLink, x.vb.Info))
            .ToListAsync(cancellationToken);

        return new GetAlbumByIdResult(new AlbumDetailDto(
            albumDto.Id, albumDto.Title, albumDto.Slug, albumDto.ReleaseDate,
            albumDto.ReleaseMonth, albumDto.ReleaseDay,
            albumDto.CoverUrl, albumDto.Type, albumDto.Format, albumDto.Label,
            albumDto.Bands, albumDto.Countries, albumDto.StreamingLinks,
            albumDto.Tracks, albumDto.Genres, albumDto.Tags, albumDto.TotalDuration,
            albumDto.DiscographyGroups, albumDto.IsExplicit, videos, similarAlbumDtos));
    }
}
