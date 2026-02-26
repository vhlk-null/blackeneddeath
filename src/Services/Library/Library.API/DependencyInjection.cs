namespace Library.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        return services;
    }

    public static WebApplicationBuilder UseApiServices(this WebApplicationBuilder app)
    {
        //app.MapCarter();
        return app;
    }
}