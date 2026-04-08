using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer;

public static class HostingExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        await scope.ServiceProvider
            .GetRequiredService<PersistedGrantDbContext>()
            .Database.MigrateAsync();

        ConfigurationDbContext configContext = scope.ServiceProvider
            .GetRequiredService<ConfigurationDbContext>();

        await configContext.Database.MigrateAsync();

        Config config = new(app.Configuration);

        if (!configContext.Clients.Any())
        {
            foreach (Duende.IdentityServer.Models.Client client in config.Clients)
                configContext.Clients.Add(client.ToEntity());
        }

        if (!configContext.ApiScopes.Any())
        {
            foreach (Duende.IdentityServer.Models.ApiScope scope2 in config.ApiScopes)
                configContext.ApiScopes.Add(scope2.ToEntity());
        }

        if (!configContext.ApiResources.Any())
        {
            foreach (Duende.IdentityServer.Models.ApiResource resource in config.ApiResources)
                configContext.ApiResources.Add(resource.ToEntity());
        }

        if (!configContext.IdentityResources.Any())
        {
            foreach (Duende.IdentityServer.Models.IdentityResource resource in config.IdentityResources)
                configContext.IdentityResources.Add(resource.ToEntity());
        }

        await configContext.SaveChangesAsync();
    }
}
