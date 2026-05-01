namespace Library.Application.Services.Bands.EventHandlers.Domain;

public sealed class BandUpdatedEventHandler(ILogger<BandUpdatedEventHandler> logger, IPublishEndpoint publishEndpoint, ILibraryDbContext context, IStorageUrlResolver urlResolver, ISearchService searchService)
    : INotificationHandler<BandUpdatedEvent>
{
    public async ValueTask Handle(BandUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        Band? band = await context.Bands
            .Include(b => b.BandGenres)
            .Include(b => b.BandCountries)
            .FirstOrDefaultAsync(b => b.Id == domainEvent.Band.Id, cancellationToken);

        if (band is null) return;

        if (!band.IsApproved) return;

        List<GenreId> genreIds = band.BandGenres.Select(bg => bg.GenreId).ToList();
        List<Genre> genres = genreIds.Count > 0
            ? await context.Genres.Where(g => genreIds.Contains(g.Id)).ToListAsync(cancellationToken)
            : [];

        List<CountryId> countryIds = band.BandCountries.Select(bc => bc.CountryId).ToList();
        List<Country> countries = countryIds.Count > 0
            ? await context.Countries.Where(c => countryIds.Contains(c.Id)).ToListAsync(cancellationToken)
            : [];

        BandGenre? primaryBandGenre = band.BandGenres.FirstOrDefault(bg => bg.IsPrimary);
        Genre? primaryGenre = primaryBandGenre is not null
            ? genres.FirstOrDefault(g => g.Id == primaryBandGenre.GenreId)
            : null;

        BandUpdatedIntegrationEvent integrationEvent = new BandUpdatedIntegrationEvent
        {
            BandId = band.Id.Value,
            Name = band.Name,
            Slug = band.Slug,
            Bio = band.Bio,
            LogoUrl = urlResolver.Resolve(band.LogoUrl),
            FormedYear = band.Activity.FormedYear,
            DisbandedYear = band.Activity.DisbandedYear,
            Status = (int)band.Status,
            PrimaryGenre = primaryGenre is not null
                ? new AlbumGenreInfo(primaryGenre.Id.Value, primaryGenre.Name, primaryGenre.Slug)
                : null,
            Countries = countries.Select(c => new AlbumCountryInfo(c.Id.Value, c.Name, c.Code)).ToList()
        };

        await publishEndpoint.Publish(integrationEvent, cancellationToken);

        await searchService.IndexBandAsync(new BandSearchDocument(
            band.Id.Value.ToString(),
            band.Name,
            band.Slug,
            urlResolver.Resolve(band.LogoUrl),
            band.Activity.FormedYear,
            band.Activity.DisbandedYear,
            band.Status.ToString(),
            genres.Select(g => g.Name).ToList(),
            countries.Select(c => c.Name).ToList(),
            band.CreatedAt.HasValue ? new DateTimeOffset(band.CreatedAt.Value).ToUnixTimeSeconds() : 0,
            band.AverageRating,
            band.RatingsCount
        ), cancellationToken);
    }
}
