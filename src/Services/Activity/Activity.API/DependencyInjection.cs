using Activity.API.Filters;
using BuildingBlocks.Authentication;
using BuildingBlocks.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Activity.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddScoped<ExtractUserIdFilter>();
        services.AddControllers();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddAuthentication("GatewayHeader")
            .AddScheme<GatewayHeaderAuthenticationOptions, GatewayHeaderAuthenticationHandler>(
                "GatewayHeader", _ => { });

        services.AddAuthorization();

        return services;
    }
}
