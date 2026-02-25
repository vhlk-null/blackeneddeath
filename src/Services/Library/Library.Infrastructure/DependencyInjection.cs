using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("LibraryDb");

        services.AddDbContext<LibraryContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<LibraryContext>());

        return services;
    }
}