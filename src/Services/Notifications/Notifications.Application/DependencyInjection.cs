namespace Notifications.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessageBroker(configuration, "notifications", Assembly.GetExecutingAssembly());
        services.AddHttpContextAccessor();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddSingleton<SseChannelService>();

        return services;
    }
}
