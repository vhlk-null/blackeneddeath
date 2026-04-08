namespace Library.Application.Services.Albums.Queries.GetAlbumByYear;

public class GetAlbumsByYearQueryHandler(ILibraryDbContext context, IStorageUrlResolver urlResolver)
    : BuildingBlocks.CQRS.IQueryHandler<GetAlbumsByYearQuery, GetAlbumsByYearResult>
{
    public async ValueTask<GetAlbumsByYearResult> Handle(GetAlbumsByYearQuery query, CancellationToken cancellationToken)
    {
        List<Album> albums = await context.Albums
            .AsNoTracking()
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Include(a => a.AlbumTracks)
            .Include(a => a.AlbumTags)
            .Include(a => a.StreamingLinks)
            .Where(a => a.AlbumRelease.ReleaseYear == query.ReleaseDate)
            .ToListAsync(cancellationToken);

        List<BandId> bandIds = albums.SelectMany(a => a.AlbumBands.Select(ab => ab.BandId)).Distinct().ToList();
        List<CountryId> countryIds = albums.SelectMany(a => a.AlbumCountries.Select(ac => ac.CountryId)).Distinct().ToList();
        List<TrackId> trackIds = albums.SelectMany(a => a.AlbumTracks.Select(at => at.TrackId)).Distinct().ToList();
        List<LabelId> labelIds = albums.Where(a => a.LabelId != null).Select(a => a.LabelId!).Distinct().ToList();
        List<TagId> tagIds = albums.SelectMany(a => a.AlbumTags.Select(at => at.TagId)).Distinct().ToList();

        Dictionary<BandId, Band> bands = await context.Bands.AsNoTracking()
            .Where(b => bandIds.Contains(b.Id))
            .ToDictionaryAsync(b => b.Id, cancellationToken);

        List<Album> discographyAlbums = await context.Albums.AsNoTracking()
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Where(a => a.AlbumBands.Any(ab => bandIds.Contains(ab.BandId)))
            .ToListAsync(cancellationToken);

        ILookup<BandId, Album> discographyByBand = discographyAlbums
            .SelectMany(a => a.AlbumBands.Select(ab => (ab.BandId, a)))
            .ToLookup(x => x.BandId, x => x.a);

        List<GenreId> genreIds = albums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId))
            .Concat(discographyAlbums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId)))
            .Distinct().ToList();

        Dictionary<GenreId, Genre> genres = await context.Genres.AsNoTracking()
            .Where(g => genreIds.Contains(g.Id))
            .ToDictionaryAsync(g => g.Id, cancellationToken);

        List<CountryId> allCountryIds = countryIds
            .Concat(discographyAlbums.SelectMany(a => a.AlbumCountries.Select(ac => ac.CountryId)))
            .Distinct().ToList();

        Dictionary<CountryId, Country> countries = await context.Countries.AsNoTracking()
            .Where(c => allCountryIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, cancellationToken);

        Dictionary<TrackId, Track> tracks = await context.Tracks.AsNoTracking()
            .Where(t => trackIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, cancellationToken);

        Dictionary<LabelId, Label> labels = await context.Labels.AsNoTracking()
            .Where(l => labelIds.Contains(l.Id))
            .ToDictionaryAsync(l => l.Id, cancellationToken);

        Dictionary<TagId, Tag> tags = tagIds.Count > 0
            ? await context.Tags.AsNoTracking()
                .Where(t => tagIds.Contains(t.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken)
            : new Dictionary<TagId, Tag>();

        List<AlbumDto> albumDtos = albums
            .Select(a => a.ToAlbumDto(bands, genres, countries, tracks, urlResolver, labels, tags, discographyByBand))
            .ToList();

        return new GetAlbumsByYearResult(albumDtos);
    }
}
