namespace Library.Application.Services.Albums.EventHandlers.Domain;

public sealed class AlbumCreatedEventHandler(
    ILogger<AlbumCreatedEventHandler> logger,
    IPublishEndpoint publishEndpoint,
    ILibraryDbContext context,
    IStorageUrlResolver urlResolver,
    ISearchService searchService)
    : INotificationHandler<AlbumCreatedEvent>
{
    public async ValueTask Handle(AlbumCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        Album? album = await context.Albums
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Include(a => a.AlbumTracks)
            .FirstOrDefaultAsync(a => a.Id == domainEvent.Album.Id, cancellationToken);

        if (album is null)
        {
            logger.LogWarning("AlbumCreatedEventHandler: Album {AlbumId} not found in DB — integration event will NOT be published", domainEvent.Album.Id.Value);
            return;
        }

        if (!album.IsApproved)
        {
            logger.LogInformation("AlbumCreatedEventHandler: Album {AlbumId} is not approved — skipping indexing and integration event", domainEvent.Album.Id.Value);
            return;
        }

        List<BandId> bandIds = album.AlbumBands.Select(ab => ab.BandId).ToList();
        List<Band> bands = bandIds.Count > 0
            ? await context.Bands.Where(b => bandIds.Contains(b.Id)).ToListAsync(cancellationToken)
            : [];

        List<GenreId> genreIds = album.AlbumGenres.Select(ag => ag.GenreId).ToList();
        List<Genre> genres = genreIds.Count > 0
            ? await context.Genres.Where(g => genreIds.Contains(g.Id)).ToListAsync(cancellationToken)
            : [];

        List<CountryId> countryIds = album.AlbumCountries.Select(ac => ac.CountryId).ToList();
        List<Country> countries = countryIds.Count > 0
            ? await context.Countries.Where(c => countryIds.Contains(c.Id)).ToListAsync(cancellationToken)
            : [];

        List<TrackId> trackIds = album.AlbumTracks.Select(at => at.TrackId).ToList();
        List<Track> tracks = trackIds.Count > 0
            ? await context.Tracks.Where(t => trackIds.Contains(t.Id)).ToListAsync(cancellationToken)
            : [];

        AlbumGenre? primaryAlbumGenre = album.AlbumGenres.FirstOrDefault(ag => ag.IsPrimary);
        Genre? primaryGenre = primaryAlbumGenre is not null
            ? genres.FirstOrDefault(g => g.Id == primaryAlbumGenre.GenreId)
            : null;

        AlbumCreatedIntegrationEvent integrationEvent = new AlbumCreatedIntegrationEvent
        {
            AlbumId = album.Id.Value,
            Title = album.Title,
            Slug = album.Slug,
            CoverUrl = urlResolver.Resolve(album.CoverUrl),
            ReleaseYear = album.AlbumRelease.ReleaseYear,
            Format = (int)album.AlbumRelease.Format,
            Type = (int)album.Type,
            Label = null,
            Bands = bands.Select(b => new AlbumBandInfo(b.Id.Value, b.Name, b.Slug)).ToList(),
            PrimaryGenre = primaryGenre is not null
                ? new AlbumGenreInfo(primaryGenre.Id.Value, primaryGenre.Name, primaryGenre.Slug)
                : null,
            Countries = countries.Select(c => new AlbumCountryInfo(c.Id.Value, c.Name, c.Code)).ToList(),
            IsExplicit = album.IsExplicit
        };

        logger.LogInformation("AlbumCreatedEventHandler: Publishing integration event for AlbumId: {AlbumId}", album.Id.Value);
        await publishEndpoint.Publish(integrationEvent, cancellationToken);

        await searchService.IndexAlbumAsync(new AlbumSearchDocument(
            album.Id.Value.ToString(),
            album.Title,
            album.Slug,
            urlResolver.Resolve(album.CoverUrl),
            album.AlbumRelease.ReleaseYear,
            album.Type.ToString(),
            album.AlbumRelease.Format.ToString(),
            bands.Select(b => new AlbumBandRef(b.Id.Value, b.Name, b.Slug)).ToList(),
            genres.Select(g => g.Name).ToList(),
            [],
            countries.Select(c => new AlbumCountryRef(c.Name, c.Code)).ToList(),
            tracks.Select(t => t.Title).ToList(),
            album.CreatedAt.HasValue ? new DateTimeOffset(album.CreatedAt.Value).ToUnixTimeSeconds() : 0,
            album.AverageRating,
            album.RatingsCount
        ), cancellationToken);
    }
}
