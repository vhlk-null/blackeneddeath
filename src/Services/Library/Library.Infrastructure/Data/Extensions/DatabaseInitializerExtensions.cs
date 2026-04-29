using BuildingBlocks.Storage;
using Library.Application.Data;
using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Index = Meilisearch.Index;

namespace Library.Infrastructure.Data.Extensions;

public static class DatabaseInitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        LibraryContext context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
        ILogger<LibraryContext> logger  = scope.ServiceProvider.GetRequiredService<ILogger<LibraryContext>>();

        await ApplyMigrationsAsync(context, logger);
        await SeedCountriesAsync(context, logger);

        //if (app.Environment.IsDevelopment())
        //    await SeedAsync(context, logger);
    }

    public static async Task InitializeMeilisearchAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        MeilisearchClient client = scope.ServiceProvider.GetRequiredService<MeilisearchClient>();
        LibraryContext context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
        IStorageUrlResolver urlResolver = scope.ServiceProvider.GetRequiredService<IStorageUrlResolver>();
        ILogger<LibraryContext> logger = scope.ServiceProvider.GetRequiredService<ILogger<LibraryContext>>();

        try
        {
            logger.LogInformation("Initializing Meilisearch indexes...");

            var albumsTask = await client.CreateIndexAsync(SearchIndexes.Albums, "id");
            var bandsTask = await client.CreateIndexAsync(SearchIndexes.Bands, "id");
            await Task.WhenAll(
                client.WaitForTaskAsync(albumsTask.TaskUid),
                client.WaitForTaskAsync(bandsTask.TaskUid));

            var typoTolerance = new TypoTolerance
            {
                Enabled = true,
                MinWordSizeForTypos = new TypoTolerance.TypoSize { OneTypo = 3, TwoTypos = 5 }
            };

            var albumsIndex = client.Index(SearchIndexes.Albums);
            await albumsIndex.UpdateFilterableAttributesAsync(SearchIndexes.AlbumAttributes.Filterable);
            await albumsIndex.UpdateSortableAttributesAsync(SearchIndexes.AlbumAttributes.Sortable);
            await albumsIndex.UpdateSearchableAttributesAsync(SearchIndexes.AlbumAttributes.Searchable);
            await albumsIndex.UpdateTypoToleranceAsync(typoTolerance);

            var bandsIndex = client.Index(SearchIndexes.Bands);
            await bandsIndex.UpdateFilterableAttributesAsync(SearchIndexes.BandAttributes.Filterable);
            await bandsIndex.UpdateSortableAttributesAsync(SearchIndexes.BandAttributes.Sortable);
            await bandsIndex.UpdateSearchableAttributesAsync(SearchIndexes.BandAttributes.Searchable);
            await bandsIndex.UpdateTypoToleranceAsync(typoTolerance);

            logger.LogInformation("Meilisearch indexes initialized successfully.");

            await ReindexAlbumsAsync(context, albumsIndex, urlResolver, logger);
            await ReindexBandsAsync(context, bandsIndex, urlResolver, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to initialize Meilisearch indexes.");
        }
    }

    private static async Task ReindexAlbumsAsync(
        LibraryContext context,
        Index index,
        IStorageUrlResolver urlResolver,
        ILogger logger)
    {
        logger.LogInformation("Reindexing albums into Meilisearch...");

        List<Album> albums = await context.Albums
            .Include(a => a.AlbumBands)
            .Include(a => a.AlbumGenres)
            .Include(a => a.AlbumCountries)
            .Include(a => a.AlbumTags)
            .Include(a => a.AlbumTracks)
            .AsNoTracking()
            .ToListAsync();

        if (albums.Count == 0)
        {
            logger.LogInformation("No albums to reindex.");
            return;
        }

        List<BandId> allBandIds = albums.SelectMany(a => a.AlbumBands.Select(ab => ab.BandId)).Distinct().ToList();
        List<GenreId> allGenreIds = albums.SelectMany(a => a.AlbumGenres.Select(ag => ag.GenreId)).Distinct().ToList();
        List<CountryId> allCountryIds = albums.SelectMany(a => a.AlbumCountries.Select(ac => ac.CountryId)).Distinct().ToList();
        List<TrackId> allTrackIds = albums.SelectMany(a => a.AlbumTracks.Select(at => at.TrackId)).Distinct().ToList();

        Dictionary<BandId, AlbumBandRef> bandNames = allBandIds.Count > 0
            ? await context.Bands.Where(b => allBandIds.Contains(b.Id)).AsNoTracking()
                .ToDictionaryAsync(b => b.Id, b => new AlbumBandRef(b.Id.Value, b.Name, b.Slug))
            : [];

        Dictionary<GenreId, string> genreNames = allGenreIds.Count > 0
            ? await context.Genres.Where(g => allGenreIds.Contains(g.Id)).AsNoTracking()
                .ToDictionaryAsync(g => g.Id, g => g.Name)
            : [];

        Dictionary<CountryId, AlbumCountryRef> countryNames = allCountryIds.Count > 0
            ? await context.Countries.Where(c => allCountryIds.Contains(c.Id)).AsNoTracking()
                .ToDictionaryAsync(c => c.Id, c => new AlbumCountryRef(c.Name, c.Code))
            : [];

        Dictionary<TrackId, string> trackTitles = allTrackIds.Count > 0
            ? await context.Tracks.Where(t => allTrackIds.Contains(t.Id)).AsNoTracking()
                .ToDictionaryAsync(t => t.Id, t => t.Title)
            : [];

        List<AlbumSearchDocument> documents = albums.Select(a => new AlbumSearchDocument(
            a.Id.Value.ToString(),
            a.Title,
            a.Slug,
            urlResolver.Resolve(a.CoverUrl),
            a.AlbumRelease.ReleaseYear,
            a.Type.ToString(),
            a.AlbumRelease.Format.ToString(),
            a.AlbumBands.Where(ab => bandNames.ContainsKey(ab.BandId)).Select(ab => bandNames[ab.BandId]).ToList(),
            a.AlbumGenres.Select(ag => genreNames.TryGetValue(ag.GenreId, out string? n) ? n : "").Where(n => n != "").ToList(),
            [],
            a.AlbumCountries.Where(ac => countryNames.ContainsKey(ac.CountryId)).Select(ac => countryNames[ac.CountryId]).ToList(),
            a.AlbumTracks.Select(at => trackTitles.TryGetValue(at.TrackId, out string? n) ? n : "").Where(n => n != "").ToList(),
            a.CreatedAt.HasValue ? new DateTimeOffset(a.CreatedAt.Value).ToUnixTimeSeconds() : 0,
            a.AverageRating,
            a.RatingsCount
        )).ToList();

        await index.AddDocumentsAsync(documents);
        logger.LogInformation("Reindexed {Count} albums into Meilisearch.", documents.Count);
    }

    private static async Task ReindexBandsAsync(
        LibraryContext context,
        Index index,
        IStorageUrlResolver urlResolver,
        ILogger logger)
    {
        logger.LogInformation("Reindexing bands into Meilisearch...");

        List<Band> bands = await context.Bands
            .Include(b => b.BandGenres)
            .Include(b => b.BandCountries)
            .AsNoTracking()
            .ToListAsync();

        if (bands.Count == 0)
        {
            logger.LogInformation("No bands to reindex.");
            return;
        }

        List<GenreId> allGenreIds = bands.SelectMany(b => b.BandGenres.Select(bg => bg.GenreId)).Distinct().ToList();
        List<CountryId> allCountryIds = bands.SelectMany(b => b.BandCountries.Select(bc => bc.CountryId)).Distinct().ToList();

        Dictionary<GenreId, string> genreNames = allGenreIds.Count > 0
            ? await context.Genres.Where(g => allGenreIds.Contains(g.Id)).AsNoTracking()
                .ToDictionaryAsync(g => g.Id, g => g.Name)
            : [];

        Dictionary<CountryId, string> countryNames = allCountryIds.Count > 0
            ? await context.Countries.Where(c => allCountryIds.Contains(c.Id)).AsNoTracking()
                .ToDictionaryAsync(c => c.Id, c => c.Name)
            : [];

        List<BandSearchDocument> documents = bands.Select(b => new BandSearchDocument(
            b.Id.Value.ToString(),
            b.Name,
            b.Slug,
            urlResolver.Resolve(b.LogoUrl),
            b.Activity.FormedYear,
            b.Activity.DisbandedYear,
            b.Status.ToString(),
            b.BandGenres.Select(bg => genreNames.TryGetValue(bg.GenreId, out string? n) ? n : "").Where(n => n != "").ToList(),
            b.BandCountries.Select(bc => countryNames.TryGetValue(bc.CountryId, out string? n) ? n : "").Where(n => n != "").ToList(),
            b.CreatedAt.HasValue ? new DateTimeOffset(b.CreatedAt.Value).ToUnixTimeSeconds() : 0,
            b.AverageRating,
            b.RatingsCount
        )).ToList();

        await index.AddDocumentsAsync(documents);
        logger.LogInformation("Reindexed {Count} bands into Meilisearch.", documents.Count);
    }

    private static async Task ApplyMigrationsAsync(LibraryContext context, ILogger logger)
    {
        logger.LogInformation("Applying database migrations...");
        await context.Database.MigrateAsync();
        logger.LogInformation("Migrations applied successfully.");
    }

    private static async Task SeedAsync(LibraryContext context, ILogger logger)
    {
        await SeedTagsAsync(context, logger);
        await SeedLabelsAsync(context, logger);
        await SeedCountriesAsync(context, logger);
        await SeedGenresAsync(context, logger);
        await SeedTracksAsync(context, logger);
        await SeedBandsAsync(context, logger);
        await SeedAlbumsAsync(context, logger);
        await SeedAlbumCoverUrlsAsync(context, logger);
        await SeedStreamingLinksAsync(context, logger);
        await SeedGenreCardsAsync(context, logger);
    }

    private static async Task SeedTagsAsync(LibraryContext context, ILogger logger)
    {
        if (await context.Tags.AnyAsync()) return;

        logger.LogInformation("Seeding tags...");
        await context.Tags.AddRangeAsync(InitialData.Tags);
        await context.SaveChangesAsync();
    }

    private static async Task SeedLabelsAsync(LibraryContext context, ILogger logger)
    {
        if (await context.Labels.AnyAsync()) return;

        logger.LogInformation("Seeding labels...");
        await context.Labels.AddRangeAsync(InitialData.Labels);
        await context.SaveChangesAsync();
    }

    private static async Task SeedCountriesAsync(LibraryContext context, ILogger logger)
    {

        logger.LogInformation("Seeding countries from embedded resource...");

        using System.IO.Stream stream = typeof(DatabaseInitializerExtensions).Assembly
            .GetManifestResourceStream("Library.Infrastructure.Data.all-countries.json")!;

        List<RestCountryDto>? restCountries = await System.Text.Json.JsonSerializer.DeserializeAsync<List<RestCountryDto>>(stream);

        if (restCountries is null || restCountries.Count == 0)
        {
            logger.LogWarning("No countries in embedded resource, skipping.");
            return;
        }

        HashSet<string> existingNames = (await context.Countries
            .Select(c => c.Name)
            .ToListAsync())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        List<Country> newCountries = restCountries
            .Where(c => !string.IsNullOrWhiteSpace(c.Name?.Common) && !existingNames.Contains(c.Name!.Common!))
            .OrderBy(c => c.Name?.Common)
            .Select(c => Country.Create(CountryId.Of(Guid.NewGuid()), c.Name!.Common!, c.Cca2))
            .ToList();

        if (newCountries.Count == 0)
        {
            logger.LogInformation("All countries already in DB, nothing to seed.");
            return;
        }

        await context.Countries.AddRangeAsync(newCountries);
        await context.SaveChangesAsync();
        logger.LogInformation("Seeded {Count} new countries.", newCountries.Count);
    }

    private sealed record RestCountryDto(
        [property: System.Text.Json.Serialization.JsonPropertyName("name")] RestCountryName? Name,
        [property: System.Text.Json.Serialization.JsonPropertyName("cca2")] string? Cca2);

    private sealed record RestCountryName(
        [property: System.Text.Json.Serialization.JsonPropertyName("common")] string? Common);

    private static async Task SeedGenresAsync(LibraryContext context, ILogger logger)
    {
        if (await context.Genres.AnyAsync()) return;

        logger.LogInformation("Seeding genres...");
        await context.Genres.AddRangeAsync(InitialData.Genres);
        await context.SaveChangesAsync();
    }

    private static async Task SeedTracksAsync(LibraryContext context, ILogger logger)
    {
        if (await context.Tracks.AnyAsync()) return;

        logger.LogInformation("Seeding tracks...");
        await context.Tracks.AddRangeAsync(InitialData.Tracks);
        await context.SaveChangesAsync();
    }

    private static async Task SeedBandsAsync(LibraryContext context, ILogger logger)
    {
        if (await context.Bands.AnyAsync()) return;

        logger.LogInformation("Seeding bands...");
        await context.Bands.AddRangeAsync(InitialData.Bands);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAlbumsAsync(LibraryContext context, ILogger logger)
    {
        if (await context.Albums.AnyAsync()) return;

        logger.LogInformation("Seeding albums...");
        await context.Albums.AddRangeAsync(InitialData.Albums);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAlbumCoverUrlsAsync(LibraryContext context, ILogger logger)
    {
        Dictionary<AlbumId, string> seedLookup = InitialData.Albums
            .Where(a => a.CoverUrl != null)
            .ToDictionary(a => a.Id, a => a.CoverUrl!);

        if (!await context.Albums.AnyAsync(a => a.CoverUrl == null && seedLookup.Keys.Contains(a.Id)))
            return;

        logger.LogInformation("Backfilling album cover URLs...");

        foreach ((AlbumId id, string coverUrl) in seedLookup)
        {
            await context.Albums
                .Where(a => a.Id == id && a.CoverUrl == null)
                .ExecuteUpdateAsync(s => s.SetProperty(a => a.CoverUrl, coverUrl));
        }
    }

    private static async Task SeedGenreCardsAsync(LibraryContext context, ILogger logger)
    {
        if (await context.GenreCards.AnyAsync()) return;

        logger.LogInformation("Seeding genre cards...");
        await context.GenreCards.AddRangeAsync(InitialData.GenreCards);
        await context.SaveChangesAsync();
    }

    private static async Task SeedStreamingLinksAsync(LibraryContext context, ILogger logger)
    {
        if (await context.StreamingLinks.AnyAsync()) return;

        logger.LogInformation("Backfilling streaming links...");

        Dictionary<AlbumId, IReadOnlyList<StreamingLink>> seedLookup = InitialData.Albums.ToDictionary(a => a.Id, a => a.StreamingLinks);

        List<Album> dbAlbums = await context.Albums
            .Include(a => a.StreamingLinks)
            .ToListAsync();

        foreach (Album album in dbAlbums)
        {
            if (!seedLookup.TryGetValue(album.Id, out IReadOnlyList<StreamingLink>? links)) continue;

            foreach (StreamingLink link in links)
                album.AddStreamingLink(link.Platform, link.EmbedCode);
        }

        await context.SaveChangesAsync();
    }
}
