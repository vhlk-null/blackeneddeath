using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace YarpApiGateway.Tests;

public class YarpGatewayWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ReverseProxy:Clusters:library-cluster:Destinations:destination1:Address"] = "http://localhost:19999/",
                ["ReverseProxy:Clusters:usercontent-cluster:Destinations:destination1:Address"] = "http://localhost:19998/",
                ["RateLimiter:Window"] = "00:05:00",
            });
        });

        builder.UseEnvironment("Testing");
    }
}
