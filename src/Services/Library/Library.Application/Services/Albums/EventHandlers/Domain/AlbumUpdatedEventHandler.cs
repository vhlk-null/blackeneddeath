namespace Library.Application.Services.Albums.EventHandlers.Domain;

public sealed class AlbumUpdatedEventHandler(
    ILogger<AlbumUpdatedEventHandler> logger,
    IPublishEndpoint publishEndpoint,
    ILibraryDbContext context,
    IStorageUrlResolver urlResolver)
    : INotificationHandler<AlbumUpdatedEvent>
{
    public async ValueTask Handle(AlbumUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        Album? album = await context.Albums
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .FirstOrDefaultAsync(a => a.Id == domainEvent.Album.Id, cancellationToken);

        if (album is null) return;

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

        AlbumGenre? primaryAlbumGenre = album.AlbumGenres.FirstOrDefault(ag => ag.IsPrimary);
        Genre? primaryGenre = primaryAlbumGenre is not null
            ? genres.FirstOrDefault(g => g.Id == primaryAlbumGenre.GenreId)
            : null;

        AlbumUpdatedIntegrationEvent integrationEvent = new AlbumUpdatedIntegrationEvent
        {
            AlbumId = album.Id.Value,
            Title = album.Title,
            Slug = album.Slug,
            CoverUrl = urlResolver.Resolve(album.CoverUrl),
            ReleaseDate = album.AlbumRelease.ReleaseYear,
            Format = (int)album.AlbumRelease.Format,
            Type = (int)album.Type,
            Bands = bands.Select(b => new AlbumBandInfo(b.Id.Value, b.Name, b.Slug)).ToList(),
            PrimaryGenre = primaryGenre is not null
                ? new AlbumGenreInfo(primaryGenre.Id.Value, primaryGenre.Name, primaryGenre.Slug)
                : null,
            Countries = countries.Select(c => new AlbumCountryInfo(c.Id.Value, c.Name, c.Code)).ToList(),
            IsExplicit = album.IsExplicit
        };

        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
