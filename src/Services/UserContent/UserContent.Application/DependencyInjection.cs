namespace UserContent.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessageBroker(configuration, "usercontent", Assembly.GetExecutingAssembly());
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContentService, UserContentService>();

        return services;
    }
}
