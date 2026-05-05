namespace Notifications.Infrastructure.Data.Extensions;

public static class DatabaseInitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        NotificationsContext context = services.GetRequiredService<NotificationsContext>();
        ILogger<NotificationsContext> logger = services.GetRequiredService<ILogger<NotificationsContext>>();

        try
        {
            logger.LogInformation("Applying Notifications database migrations...");
            await context.Database.MigrateAsync();
            logger.LogInformation("Notifications database migrations applied successfully!");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the Notifications database");
            throw;
        }
    }
}
