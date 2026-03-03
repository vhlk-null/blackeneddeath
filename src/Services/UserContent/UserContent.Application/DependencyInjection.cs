using Microsoft.Extensions.DependencyInjection;
using UserContent.Application.Services;

namespace UserContent.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserContentService, UserContentService>();
        return services;
    }
}
