namespace Notifications.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddScoped<Filters.ExtractUserIdFilter>();
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
