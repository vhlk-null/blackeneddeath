using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer;

public static class HostingExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        await scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>()
            .Database.MigrateAsync();

        await scope.ServiceProvider
            .GetRequiredService<PersistedGrantDbContext>()
            .Database.MigrateAsync();

        ConfigurationDbContext configContext = scope.ServiceProvider
            .GetRequiredService<ConfigurationDbContext>();

        await configContext.Database.MigrateAsync();

        await SyncConfigurationAsync(configContext, app.Configuration);

        if (app.Environment.IsDevelopment())
            await SeedUsersAsync(scope.ServiceProvider, app.Configuration);
    }

    private static async Task SyncConfigurationAsync(ConfigurationDbContext configContext, IConfiguration configuration)
    {
        Config config = new(configuration);

        if (!configContext.Clients.Any())
        {
            foreach (Duende.IdentityServer.Models.Client client in config.Clients)
                configContext.Clients.Add(client.ToEntity());
        }

        foreach (Duende.IdentityServer.Models.ApiScope apiScope in config.ApiScopes)
        {
            Duende.IdentityServer.EntityFramework.Entities.ApiScope? existing =
                configContext.ApiScopes
                    .Include(s => s.UserClaims)
                    .FirstOrDefault(s => s.Name == apiScope.Name);

            if (existing is null)
            {
                configContext.ApiScopes.Add(apiScope.ToEntity());
            }
            else
            {
                existing.UserClaims.Clear();
                foreach (string claim in apiScope.UserClaims)
                    existing.UserClaims.Add(new() { Type = claim });
            }
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

    private static async Task SeedUsersAsync(IServiceProvider services, IConfiguration configuration)
    {
        UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        Config config = new(configuration);

        foreach (Duende.IdentityServer.Test.TestUser testUser in config.TestUsers)
        {
            if (await userManager.FindByNameAsync(testUser.Username) is not null)
                continue;

            ApplicationUser user = new()
            {
                UserName = testUser.Username,
                Email = testUser.Claims.FirstOrDefault(c => c.Type == "email")?.Value
            };

            await userManager.CreateAsync(user, testUser.Password);
            await userManager.AddClaimsAsync(user, testUser.Claims);
        }
    }
}
