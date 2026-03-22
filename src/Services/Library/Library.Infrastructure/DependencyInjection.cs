using CloudinaryDotNet;
using Library.Infrastructure.Repositories;

namespace Library.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("LibraryDb");

        var cloudinaryAccount = new Account(
            configuration["Storage:CloudName"],
            configuration["Storage:ApiKey"],
            configuration["Storage:ApiSecret"]);

        services.AddSingleton(new Cloudinary(cloudinaryAccount) { Api = { Secure = true } });
        services.AddScoped<IStorageService, CloudinaryStorageService>();
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
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<ILibraryDbContext, LibraryContext>();
        services.AddScoped<IRepository<LibraryContext>, LibraryRepository>();
        return services;
    }
}
