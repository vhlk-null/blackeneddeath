using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Storage;

public static class StorageServiceExtensions
{
    public static IServiceCollection AddStorageService(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        string? cloudName = configuration["Storage:CloudName"];
        string environmentPrefix = environment.IsProduction() ? "production" : "local";

        if (!string.IsNullOrEmpty(cloudName))
        {
            Account account = new(cloudName, configuration["Storage:ApiKey"], configuration["Storage:ApiSecret"]);
            services.AddSingleton(new Cloudinary(account) { Api = { Secure = true } });
            services.AddScoped<IStorageService>(sp => new CloudinaryStorageService(sp.GetRequiredService<Cloudinary>(), environmentPrefix));
        }
        else
        {
            services.AddScoped<IStorageService, NullStorageService>();
        }

        services.AddSingleton<IStorageUrlResolver, StorageUrlResolver>();

        return services;
    }
}
