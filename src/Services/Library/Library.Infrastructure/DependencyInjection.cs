using BuildingBlocks.Storage;
using Library.Infrastructure.Search;

namespace Library.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        string? connectionString = configuration.GetConnectionString("LibraryDb");

        services.AddStorageService(configuration, environment);

        services.AddHttpContextAccessor();
        services.AddScoped<AuditableEntityInterceptor>();
        services.AddScoped<DispatchDomainEventsInterceptor>();
        services.AddSingleton<SlowQueryInterceptor>();

        services.AddDbContext<LibraryContext>((sp, options) =>
        {
            options.AddInterceptors(
                sp.GetRequiredService<AuditableEntityInterceptor>(),
                sp.GetRequiredService<DispatchDomainEventsInterceptor>(),
                sp.GetRequiredService<SlowQueryInterceptor>()
            );
            options.UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null));
        });

        services.AddScoped<ILibraryDbContext, LibraryContext>();
        services.AddScoped<IRepository<LibraryContext>, LibraryRepository>();
        services.AddScoped<ISearchService, MeilisearchService>();
        return services;
    }
}
