using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using UserContent.Application.Abstractions;

namespace UserContent.APITests;

public class UserContentWebAppFactory : WebApplicationFactory<Program>
{
    public Mock<IUserContentService> ServiceMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:UserContentDB"] = "Host=localhost;Database=test_db;Username=test;Password=test",
                ["ConnectionStrings:Redis"] = "localhost:6379,abortConnect=false",
                ["GrpcSettings:LibraryUrl"] = "http://localhost:5000",
                ["MessageBroker:Host"] = "amqp://localhost:5672",
                ["MessageBroker:UserName"] = "guest",
                ["MessageBroker:Password"] = "guest"
            });
        });

        builder.ConfigureServices(services =>
        {
            ServiceDescriptor? descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IUserContentService));
            if (descriptor != null)
                services.Remove(descriptor);
            services.AddSingleton(ServiceMock.Object);

            // Remove MassTransit hosted services to prevent RabbitMQ connection attempts
            List<ServiceDescriptor> hostedServiceDescriptors = services
                .Where(d => d.ServiceType == typeof(IHostedService) &&
                            d.ImplementationType?.FullName?.Contains("MassTransit") == true)
                .ToList();
            foreach (ServiceDescriptor d in hostedServiceDescriptors)
                services.Remove(d);

            // Replace IPublishEndpoint with a mock so MassTransit DI is satisfied
            List<ServiceDescriptor> publishDescriptors = services.Where(d => d.ServiceType == typeof(IPublishEndpoint)).ToList();
            foreach (ServiceDescriptor d in publishDescriptors)
                services.Remove(d);
            services.AddSingleton(new Mock<IPublishEndpoint>().Object);
        });

        builder.UseEnvironment("Testing");
    }
}
