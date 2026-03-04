using Mediator;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                ["ConnectionStrings:LibraryDb"] = "Host=localhost;Database=test_db;Username=test;Password=test"
            });
        });

        builder.ConfigureServices(services =>
        {
            var descriptors = services.Where(d => d.ServiceType == typeof(ISender)).ToList();
            foreach (var d in descriptors)
                services.Remove(d);

            services.AddSingleton(SenderMock.Object);
        });

        builder.UseEnvironment("Testing");
    }
}
