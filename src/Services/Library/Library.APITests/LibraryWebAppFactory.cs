using MassTransit;
using Mediator;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace Library.APITests;

public class LibraryWebAppFactory : WebApplicationFactory<Program>
{
    public Mock<ISender> SenderMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:LibraryDb"] = "Host=localhost;Database=test_db;Username=test;Password=test",
                ["MessageBroker:Host"] = "amqp://localhost:5672",
                ["MessageBroker:UserName"] = "guest",
                ["MessageBroker:Password"] = "guest",
                ["FeatureManagement:AlbumFullfilment"] = "false"
            });
        });

        builder.ConfigureServices(services =>
        {
            // Replace Mediator ISender with mock
            var descriptors = services.Where(d => d.ServiceType == typeof(ISender)).ToList();
            foreach (var d in descriptors)
                services.Remove(d);
            services.AddSingleton(SenderMock.Object);

            // Remove MassTransit hosted services to prevent RabbitMQ connection attempts
            var hostedServiceDescriptors = services
                .Where(d => d.ServiceType == typeof(IHostedService) &&
                            d.ImplementationType?.FullName?.Contains("MassTransit") == true)
                .ToList();
            foreach (var d in hostedServiceDescriptors)
                services.Remove(d);

            // Replace IPublishEndpoint with a mock so MassTransit DI is satisfied
            var publishDescriptors = services.Where(d => d.ServiceType == typeof(IPublishEndpoint)).ToList();
            foreach (var d in publishDescriptors)
                services.Remove(d);
            services.AddSingleton(new Mock<IPublishEndpoint>().Object);
        });

        builder.UseEnvironment("Testing");
    }
}
