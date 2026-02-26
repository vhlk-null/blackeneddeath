using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Library.Infrastructure.Data.Extensions;

public static class DatabaseInitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();
        var logger  = scope.ServiceProvider.GetRequiredService<ILogger<LibraryContext>>();

        context.Database.MigrateAsync().GetAwaiter().GetResult();

        await SeedAsync(context, logger);
    }

    private static async Task SeedAsync(LibraryContext context, ILogger logger)
    {
        // Order matters: independent entities first, then those with FK dependencies.
        // Band/Album inserts cascade their junction rows automatically via EF Core
        // navigation tracking (BandGenres, BandCountries, AlbumBands, etc.).
        await SeedCountriesAsync(context, logger);
        await SeedGenresAsync(context, logger);
        await SeedTracksAsync(context, logger);
        await SeedBandsAsync(context, logger);
        await SeedAlbumsAsync(context, logger);
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
}
