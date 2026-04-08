using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserContent.Infrastructure.Data.Seed;

namespace UserContent.Infrastructure.Data.Extensions;

public static class DatabaseSeeder
{
    public static async Task SeedDatabaseAsync(
        IServiceProvider serviceProvider,
        ILogger logger)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        UserContentContext context = scope.ServiceProvider.GetRequiredService<UserContentContext>();

        try
        {
            logger.LogInformation("Checking if UserContent database seeding is required...");

            if (await IsAlreadySeededAsync(context))
            {
                logger.LogInformation("UserContent database already seeded. Skipping...");
                return;
            }

            logger.LogInformation("Starting UserContent database seeding...");

            await using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();

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
        bool hasUserProfiles = await context.UserProfiles.AnyAsync();
        bool hasAlbums = await context.Albums.AnyAsync();
        bool hasBands = await context.Bands.AnyAsync();

        return hasUserProfiles && hasAlbums && hasBands;
    }

    private static async Task SeedEntitiesAsync(UserContentContext context, ILogger logger)
    {
        if (!await context.UserProfiles.AnyAsync())
        {
            logger.LogInformation("Seeding UserProfiles...");
            await context.UserProfiles.AddRangeAsync(UserProfileSeed.GetUserProfiles());
            await context.SaveChangesAsync();
            logger.LogInformation("UserProfiles seeded successfully!");
        }

        if (!await context.Albums.AnyAsync())
        {
            logger.LogInformation("Seeding Albums...");
            await context.Albums.AddRangeAsync(AlbumSeed.GetAlbums());
            await context.SaveChangesAsync();
            logger.LogInformation("Albums seeded successfully!");
        }

        if (!await context.Bands.AnyAsync())
        {
            logger.LogInformation("Seeding Bands...");
            await context.Bands.AddRangeAsync(BandSeed.GetBands());
            await context.SaveChangesAsync();
            logger.LogInformation("Bands seeded successfully!");
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
