// Data/DatabaseSeeder.cs
using Archive.API.Data.Seeds;

namespace Archive.API.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDatabaseAsync(
            IServiceProvider serviceProvider,
            ILogger logger)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ArchiveContext>();

            try
            {
                logger.LogInformation("Checking if database seeding is required...");

                // Перевірити чи потрібно seed
                if (await context.AlbumBands.AnyAsync())
                {
                    logger.LogInformation("Database already seeded. Skipping...");
                    return;
                }

                logger.LogInformation("Starting database seeding...");

                // Seed junction tables
                await SeedJunctionTablesAsync(context, logger);

                logger.LogInformation("Database seeding completed successfully!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }

        private static async Task SeedJunctionTablesAsync(
            ArchiveContext context,
            ILogger logger)
        {
            // Album-Band relationships
            if (!await context.AlbumBands.AnyAsync())
            {
                logger.LogInformation("Seeding AlbumBands...");
                await context.AlbumBands.AddRangeAsync(
                    AlbumRelationshipsSeed.GetAlbumBands());
            }

            // Album-Genre relationships
            if (!await context.AlbumGenres.AnyAsync())
            {
                logger.LogInformation("Seeding AlbumGenres...");
                await context.AlbumGenres.AddRangeAsync(
                    AlbumRelationshipsSeed.GetAlbumGenres());
            }

            // Album-Country relationships
            if (!await context.AlbumCountries.AnyAsync())
            {
                logger.LogInformation("Seeding AlbumCountries...");
                await context.AlbumCountries.AddRangeAsync(
                    AlbumRelationshipsSeed.GetAlbumCountries());
            }

            // Album-Track relationships
            if (!await context.AlbumTracks.AnyAsync())
            {
                logger.LogInformation("Seeding AlbumTracks...");
                await context.AlbumTracks.AddRangeAsync(
                    AlbumRelationshipsSeed.GetAlbumTracks());
            }

            // Save all changes
            await context.SaveChangesAsync();
            logger.LogInformation("Junction tables seeded successfully!");
        }
    }
}