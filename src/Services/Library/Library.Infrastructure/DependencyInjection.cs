using BuildingBlocks.Storage;
using Library.Application.Services.Import;
using Library.Infrastructure.Discogs;
using Library.Infrastructure.MusicBrainz;
using Library.Infrastructure.Odesli;
using Library.Infrastructure.Resolvers;
using Library.Infrastructure.Search;
using Microsoft.Extensions.Logging;

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
                    errorCodesToAdd: null)
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        });

        services.AddScoped<ILibraryDbContext, LibraryContext>();
        services.Decorate<ILibraryDbContext, CachingLibraryContext>();
        services.AddScoped<IAlbumCacheInvalidator, AlbumCacheInvalidator>();

        string? redisConnection = configuration.GetConnectionString("Redis");
        if (!string.IsNullOrWhiteSpace(redisConnection))
            services.AddStackExchangeRedisCache(o => o.Configuration = redisConnection);
        else
            services.AddDistributedMemoryCache();
        services.AddScoped<ISearchService, MeilisearchService>();

        string contactEmail = configuration["MusicBrainz:ContactEmail"] ?? "unknown";
        services.AddHttpClient<MusicBrainzService>(client =>
        {
            client.BaseAddress = new Uri("https://musicbrainz.org/ws/2/");
            client.DefaultRequestHeaders.UserAgent.ParseAdd($"MetalArchiveSite/1.0 ({contactEmail})");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            client.Timeout = TimeSpan.FromSeconds(30);
        });
        // MusicBrainz import — commented while using Discogs as primary
        // services.AddScoped<IMusicBrainzImportService>(sp => sp.GetRequiredService<MusicBrainzService>());
        // services.AddScoped<IBandImportService>(sp => sp.GetRequiredService<MusicBrainzService>());

        string discogsToken = configuration["Discogs:Token"] ?? string.Empty;
        services.AddHttpClient<DiscogsService>(client =>
        {
            client.BaseAddress = new Uri("https://api.discogs.com/");
            client.DefaultRequestHeaders.UserAgent.ParseAdd("MetalArchiveSite/1.0");
            if (!string.IsNullOrWhiteSpace(discogsToken))
                client.DefaultRequestHeaders.Add("Authorization", $"Discogs token={discogsToken}");
            client.Timeout = TimeSpan.FromSeconds(15);
        });
        services.AddScoped<IDiscogsService>(sp => sp.GetRequiredService<DiscogsService>());

        services.AddHttpClient<DiscogsImportService>(client =>
        {
            client.BaseAddress = new Uri("https://api.discogs.com/");
            client.DefaultRequestHeaders.UserAgent.ParseAdd("MetalArchiveSite/1.0");
            if (!string.IsNullOrWhiteSpace(discogsToken))
                client.DefaultRequestHeaders.Add("Authorization", $"Discogs token={discogsToken}");
            client.Timeout = TimeSpan.FromSeconds(15);
        });
        services.AddScoped<IBandImportService>(sp => sp.GetRequiredService<DiscogsImportService>());

        services.AddHttpClient<OdesliService>(client =>
        {
            client.BaseAddress = new Uri("https://api.song.link/v1-alpha.1/");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            client.Timeout = TimeSpan.FromSeconds(15);
        });
        services.AddScoped<IOdesliService>(sp => sp.GetRequiredService<OdesliService>());

        // Apple Music — commented while testing Discogs
        // services.AddHttpClient<AppleMusicResolver>(client =>
        // {
        //     client.BaseAddress = new Uri("https://itunes.apple.com/");
        //     client.Timeout = TimeSpan.FromSeconds(10);
        // });
        // services.AddScoped<IStreamingLinkResolver>(sp => sp.GetRequiredService<AppleMusicResolver>());

        // Deezer — commented while testing Discogs
        // services.AddHttpClient<DeezerResolver>(client =>
        // {
        //     client.BaseAddress = new Uri("https://api.deezer.com/");
        //     client.Timeout = TimeSpan.FromSeconds(10);
        // });
        // services.AddScoped<IStreamingLinkResolver>(sp => sp.GetRequiredService<DeezerResolver>());

        string youTubeApiKey = configuration["YouTube:ApiKey"] ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(youTubeApiKey))
        {
            services.AddHttpClient("YouTube", client =>
            {
                client.BaseAddress = new Uri("https://www.googleapis.com/youtube/v3/");
                client.Timeout = TimeSpan.FromSeconds(10);
            });
            services.AddScoped<YouTubeResolver>(sp =>
                new YouTubeResolver(
                    sp.GetRequiredService<IHttpClientFactory>().CreateClient("YouTube"),
                    sp.GetRequiredService<ILogger<YouTubeResolver>>(),
                    youTubeApiKey));
        }

        return services;
    }
}
