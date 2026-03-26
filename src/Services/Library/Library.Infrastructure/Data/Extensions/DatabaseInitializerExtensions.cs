using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Library.Infrastructure.Data.Extensions;

public static class DatabaseInitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
        var logger  = scope.ServiceProvider.GetRequiredService<ILogger<LibraryContext>>();

        await ApplyMigrationsAsync(context, logger);
        await SeedAsync(context, logger);
    }

    private static async Task ApplyMigrationsAsync(LibraryContext context, ILogger logger)
    {
        try
        {
            logger.LogInformation("Applying database migrations...");
            await context.Database.MigrateAsync();
            logger.LogInformation("Migrations applied successfully.");
        }
        catch (PostgresException ex) when (ex.SqlState == "42P07")
        {
            // 42P07 = "relation already exists". This happens when the schema was
            // created (e.g. by a previous run that deadlocked before recording the
            // migration history), so the tables exist but __EFMigrationsHistory has
            // no entry.  Recover by marking all migrations as applied.
            logger.LogWarning(
                "Database schema already exists without recorded migration history. " +
                "Inserting missing migration records and continuing...");

            await EnsureMigrationHistoryAsync(context);
        }
    }

    private static async Task EnsureMigrationHistoryAsync(LibraryContext context)
    {
        const string productVersion = "10.0.3";

        await context.Database.ExecuteSqlRawAsync("""
            CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
                "MigrationId"   character varying(150) NOT NULL,
                "ProductVersion" character varying(32)  NOT NULL,
                CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
            )
            """);

        foreach (var migrationId in context.Database.GetMigrations())
        {
            await context.Database.ExecuteSqlAsync(
                $"""
                INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
                VALUES ({migrationId}, {productVersion})
                ON CONFLICT DO NOTHING
                """);
        }
    }

    private static async Task SeedAsync(LibraryContext context, ILogger logger)
    {
        await SeedLabelsAsync(context, logger);
        await SeedCountriesAsync(context, logger);
        await SeedGenresAsync(context, logger);
        await SeedTracksAsync(context, logger);
        await SeedBandsAsync(context, logger);
        await SeedAlbumsAsync(context, logger);
        await SeedAlbumCoverUrlsAsync(context, logger);
        await SeedStreamingLinksAsync(context, logger);
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
        if (await context.Countries.AnyAsync()) return;

        logger.LogInformation("Seeding countries...");
        await context.Countries.AddRangeAsync(InitialData.Countries);
        await context.SaveChangesAsync();
    }

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
        var seedLookup = InitialData.Albums
            .Where(a => a.CoverUrl != null)
            .ToDictionary(a => a.Id, a => a.CoverUrl!);

        if (!await context.Albums.AnyAsync(a => a.CoverUrl == null && seedLookup.Keys.Contains(a.Id)))
            return;

        logger.LogInformation("Backfilling album cover URLs...");

        foreach (var (id, coverUrl) in seedLookup)
        {
            await context.Albums
                .Where(a => a.Id == id && a.CoverUrl == null)
                .ExecuteUpdateAsync(s => s.SetProperty(a => a.CoverUrl, coverUrl));
        }
    }

    private static async Task SeedStreamingLinksAsync(LibraryContext context, ILogger logger)
    {
        if (await context.StreamingLinks.AnyAsync()) return;

        logger.LogInformation("Backfilling streaming links...");

        var seedLookup = InitialData.Albums.ToDictionary(a => a.Id, a => a.StreamingLinks);

        var dbAlbums = await context.Albums
            .Include(a => a.StreamingLinks)
            .ToListAsync();

        foreach (var album in dbAlbums)
        {
            if (!seedLookup.TryGetValue(album.Id, out var links)) continue;

            foreach (var link in links)
                album.AddStreamingLink(link.Platform, link.EmbedCode);
        }

        await context.SaveChangesAsync();
    }
}
