using System.Reflection;
using Activity.Application.Abstractions;
using Activity.Application.Services;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Activity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IActivityService, ActivityService>();
        services.AddMessageBroker(configuration, "activity", Assembly.GetExecutingAssembly());

        return services;
    }
}
