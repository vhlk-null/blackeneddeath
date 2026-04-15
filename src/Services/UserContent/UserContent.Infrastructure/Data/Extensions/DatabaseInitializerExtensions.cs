

namespace UserContent.Infrastructure.Data.Extensions;

public static class DatabaseInitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        UserContentContext context = services.GetRequiredService<UserContentContext>();
        ILogger<UserContentContext> logger = services.GetRequiredService<ILogger<UserContentContext>>();

        try
        {
            logger.LogInformation("Applying UserContent database migrations...");
            await context.Database.MigrateAsync();
            logger.LogInformation("UserContent database migrations applied successfully!");

            // await DatabaseSeeder.SeedDatabaseAsync(services, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating or seeding the UserContent database");
            throw;
        }
    }
}
