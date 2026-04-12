using BuildingBlocks.Authentication;
using BuildingBlocks.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace UserContent.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddControllers();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddAuthentication("GatewayHeader")
            .AddScheme<GatewayHeaderAuthenticationOptions, GatewayHeaderAuthenticationHandler>(
                "GatewayHeader", _ => { });

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("admin"));
        });

        return services;
    }
}
