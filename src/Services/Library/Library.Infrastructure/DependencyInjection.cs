using Library.Infrastructure.Data;
using Library.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("LibraryDb");

        services.AddSingleton<AuditableEntityInterceptor>();
        services.AddSingleton<SlowQueryInterceptor>();

        services.AddDbContext<LibraryContext>((sp, options) =>
        {
            options.AddInterceptors(
                sp.GetRequiredService<AuditableEntityInterceptor>(),
                sp.GetRequiredService<SlowQueryInterceptor>()
            );
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<LibraryContext>());
        return services;
    }
}