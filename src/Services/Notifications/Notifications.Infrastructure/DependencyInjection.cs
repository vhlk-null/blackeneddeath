namespace Notifications.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string dbConnection = configuration.GetConnectionString("NotificationsDB")
                              ?? throw new InvalidOperationException("NotificationsDB connection string is missing");

        services.AddDbContext<NotificationsContext>(options =>
            options.UseNpgsql(dbConnection));

        services.AddScoped<IRepository<NotificationsContext>, NotificationsRepository>();

        services.AddHealthChecks()
            .AddNpgSql(dbConnection);

        return services;
    }
}
