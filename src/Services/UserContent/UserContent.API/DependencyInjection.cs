using BuildingBlocks.Exceptions;
using Carter;
using Microsoft.Extensions.DependencyInjection;

namespace UserContent.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddCarter();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
