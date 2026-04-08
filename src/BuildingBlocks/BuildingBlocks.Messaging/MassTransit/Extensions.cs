using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Messaging.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMessageBroker(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] consumerAssemblies)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            if (consumerAssemblies.Length > 0)
                config.AddConsumers(consumerAssemblies);

            config.UsingRabbitMq((context, cfg) =>
            {
                ILogger<IBus> logger = context.GetRequiredService<ILogger<IBus>>();

                string? host = configuration["MessageBroker:Host"];
                string? username = configuration["MessageBroker:Username"];

                if (string.IsNullOrWhiteSpace(host))
                {
                    logger.LogError("MessageBroker:Host is not configured.");
                    throw new InvalidOperationException("MessageBroker:Host is not configured.");
                }

                logger.LogInformation("Connecting to RabbitMQ at {Host} as {Username}", host, username);

                cfg.Host(new Uri(host), h =>
                {
                    h.Username(username!);
                    h.Password(configuration["MessageBroker:Password"]!);
                });

                cfg.ConfigureEndpoints(context);

                logger.LogInformation("RabbitMQ configured successfully.");
            });
        });

        return services;
    }
}