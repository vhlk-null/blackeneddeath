namespace Library.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString("LibraryDb");

        var cloudinaryAccount = new Account(
            configuration["Storage:CloudName"],
            configuration["Storage:ApiKey"],
            configuration["Storage:ApiSecret"]);

        var environmentPrefix = environment.IsProduction() ? "production" : "local";

        services.AddSingleton(new Cloudinary(cloudinaryAccount) { Api = { Secure = true } });
        services.AddScoped<IStorageService>(sp => new CloudinaryStorageService(
            sp.GetRequiredService<Cloudinary>(), environmentPrefix));
        services.AddSingleton<IStorageUrlResolver, StorageUrlResolver>();

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
        return services;
    }
}
