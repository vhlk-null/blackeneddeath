using Library.API.Data;
using Library.API.Data.Seed;
using Library.Infrastructure.Data;

namespace Library.API.Extensions;

public static class DatabaseSeeder
{
    public static async Task SeedDatabaseAsync(
        IServiceProvider serviceProvider,
        ILogger logger)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();

        try
        {
            logger.LogInformation("Checking if database seeding is required...");

            // Better check: verify key entities exist
            if (await IsAlreadySeededAsync(context))
            {
                logger.LogInformation("Database already seeded. Skipping...");
                return;
            }

            logger.LogInformation("Starting database seeding...");

            // Use transaction for atomicity
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                await SeedBaseEntitiesAsync(context, logger);
                await SeedJunctionTablesAsync(context, logger);

                await transaction.CommitAsync();
                logger.LogInformation("Database seeding completed successfully!");
            }
            catch
            {
                await transaction.RollbackAsync();
                logger.LogError("Database seeding failed. Transaction rolled back.");
                throw;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private static async Task<bool> IsAlreadySeededAsync(LibraryContext context)
    {
        // Check multiple tables for robustness
        var hasCountries = await context.Countries.AnyAsync();
        var hasGenres = await context.Genres.AnyAsync();
        var hasBands = await context.Bands.AnyAsync();
        var hasAlbums = await context.Albums.AnyAsync();
        var hasJunctionData = await context.AlbumBands.AnyAsync();

        return hasCountries && hasGenres && hasBands && hasAlbums && hasJunctionData;
    }

    private static async Task SeedBaseEntitiesAsync(LibraryContext context, ILogger logger)
    {
        // Seed in dependency order: independent entities first

        if (!await context.Countries.AnyAsync())
        {
            logger.LogInformation("Seeding Countries...");
            await context.Countries.AddRangeAsync(CountrySeed.GetCountries());
            await context.SaveChangesAsync();
            logger.LogInformation("Countries seeded successfully!");
        }

        if (!await context.Genres.AnyAsync())
        {
            logger.LogInformation("Seeding Genres...");
            await context.Genres.AddRangeAsync(GenreSeed.GetGenres());
            await context.SaveChangesAsync();
            logger.LogInformation("Genres seeded successfully!");
        }

        if (!await context.Tracks.AnyAsync())
        {
            logger.LogInformation("Seeding Tracks...");
            await context.Tracks.AddRangeAsync(TrackSeed.GetTracks());
            await context.SaveChangesAsync();
            logger.LogInformation("Tracks seeded successfully!");
        }

        if (!await context.Bands.AnyAsync())
        {
            logger.LogInformation("Seeding Bands...");
            await context.Bands.AddRangeAsync(BandSeed.GetBands());
            await context.SaveChangesAsync();
            logger.LogInformation("Bands seeded successfully!");
        }

        if (!await context.Albums.AnyAsync())
        {
            logger.LogInformation("Seeding Albums...");
            await context.Albums.AddRangeAsync(AlbumSeed.GetAlbums());
            await context.SaveChangesAsync();
            logger.LogInformation("Albums seeded successfully!");
        }
    }

    private static async Task SeedJunctionTablesAsync(LibraryContext context, ILogger logger)
    {
        //// Album-Band relationships
        //if (!await context.AlbumBands.AnyAsync())
        //{
        //    logger.LogInformation("Seeding AlbumBands...");
        //    await context.AlbumBands.AddRangeAsync(RelationshipsSeed.GetAlbumBands());
        //}

        //// Album-Genre relationships
        //if (!await context.AlbumGenres.AnyAsync())
        //{
        //    logger.LogInformation("Seeding AlbumGenres...");
        //    await context.AlbumGenres.AddRangeAsync(RelationshipsSeed.GetAlbumGenres());
        //}

        //// Album-Country relationships
        //if (!await context.AlbumCountries.AnyAsync())
        //{
        //    logger.LogInformation("Seeding AlbumCountries...");
        //    await context.AlbumCountries.AddRangeAsync(RelationshipsSeed.GetAlbumCountries());
        //}

        //// Album-Track relationships
        //if (!await context.AlbumTracks.AnyAsync())
        //{
        //    logger.LogInformation("Seeding AlbumTracks...");
        //    await context.AlbumTracks.AddRangeAsync(RelationshipsSeed.GetAlbumTracks());
        //}

        //// Band-Genre relationships
        //if (!await context.BandGenres.AnyAsync())
        //{
        //    logger.LogInformation("Seeding BandGenres...");
        //    await context.BandGenres.AddRangeAsync(RelationshipsSeed.GetBandGenres());
        //}

        // Save all junction tables
        await context.SaveChangesAsync();
        logger.LogInformation("Junction tables seeded successfully!");
    }
}