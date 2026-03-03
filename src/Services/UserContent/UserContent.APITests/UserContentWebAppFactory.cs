using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using UserContent.Application.Abstractions;

namespace UserContent.APITests;

public class UserContentWebAppFactory : WebApplicationFactory<Program>
{
    public Mock<IUserContentService> ServiceMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove all real infrastructure/application registrations and replace with mocks
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IUserContentService));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddSingleton(ServiceMock.Object);
        });

        builder.UseEnvironment("Testing");
    }
}
