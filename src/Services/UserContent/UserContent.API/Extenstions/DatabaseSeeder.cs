using UserContent.API.Data;
using UserContent.API.Data.Seed;

namespace UserContent.API.Extenstions
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDatabaseAsync(
            IServiceProvider serviceProvider,
            ILogger logger)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UserContentContext>();

            try
            {
                logger.LogInformation("Checking if UserContent database seeding is required...");

                if (await IsAlreadySeededAsync(context))
                {
                    logger.LogInformation("UserContent database already seeded. Skipping...");
                    return;
                }

                logger.LogInformation("Starting UserContent database seeding...");

                await using var transaction = await context.Database.BeginTransactionAsync();

                try
                {
                    await SeedEntitiesAsync(context, logger);

                    await transaction.CommitAsync();
                    logger.LogInformation("UserContent database seeding completed successfully!");
                }
                catch
                {
                    await transaction.RollbackAsync();
                    logger.LogError("UserContent database seeding failed. Transaction rolled back.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the UserContent database");
                throw;
            }
        }

        private static async Task<bool> IsAlreadySeededAsync(UserContentContext context)
        {
            var hasUserProfiles = await context.UserProfiles.AnyAsync();
            var hasFavoriteAlbums = await context.FavoriteAlbums.AnyAsync();
            var hasFavoriteBands = await context.FavoriteBands.AnyAsync();

            return hasUserProfiles && hasFavoriteAlbums && hasFavoriteBands;
        }

        private static async Task SeedEntitiesAsync(UserContentContext context, ILogger logger)
        {
            // Seed in dependency order: UserProfiles first (parent), then favorites

            if (!await context.UserProfiles.AnyAsync())
            {
                logger.LogInformation("Seeding UserProfiles...");
                await context.UserProfiles.AddRangeAsync(UserProfileSeed.GetUserProfiles());
                await context.SaveChangesAsync();
                logger.LogInformation("UserProfiles seeded successfully!");
            }

            if (!await context.FavoriteAlbums.AnyAsync())
            {
                logger.LogInformation("Seeding FavoriteAlbums...");
                await context.FavoriteAlbums.AddRangeAsync(FavoriteAlbumSeed.GetFavoriteAlbums());
                await context.SaveChangesAsync();
                logger.LogInformation("FavoriteAlbums seeded successfully!");
            }

            if (!await context.FavoriteBands.AnyAsync())
            {
                logger.LogInformation("Seeding FavoriteBands...");
                await context.FavoriteBands.AddRangeAsync(FavoriteBandSeed.GetFavoriteBands());
                await context.SaveChangesAsync();
                logger.LogInformation("FavoriteBands seeded successfully!");
            }
        }
    }
}
