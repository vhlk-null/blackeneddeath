using BuildingBlocks.Storage;
using Library.Application.Services.Import;
using Library.Infrastructure.MusicBrainz;
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

        string contactEmail = configuration["MusicBrainz:ContactEmail"] ?? "unknown";
        services.AddHttpClient<MusicBrainzService>(client =>
        {
            client.BaseAddress = new Uri("https://musicbrainz.org/ws/2/");
            client.DefaultRequestHeaders.UserAgent.ParseAdd($"MetalArchiveSite/1.0 ({contactEmail})");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            client.Timeout = TimeSpan.FromSeconds(30);
        });
        services.AddScoped<IMusicBrainzImportService>(sp => sp.GetRequiredService<MusicBrainzService>());

        return services;
    }
}
