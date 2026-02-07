namespace UserContent.API.Extenstions
{
    public static class DatabaseInitializerExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
                return;

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var context = services.GetRequiredService<UserContentContext>();

                logger.LogInformation("Applying UserContent database migrations...");
                await context.Database.MigrateAsync();
                logger.LogInformation("UserContent database migrations applied successfully!");

                await DatabaseSeeder.SeedDatabaseAsync(services, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating or seeding the UserContent database");
                throw;
            }
        }
    }
}
