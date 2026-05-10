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
        IServiceProvider services = scope.ServiceProvider;

        await services.GetRequiredService<ApplicationDbContext>()
            .Database.MigrateAsync();

        await services.GetRequiredService<PersistedGrantDbContext>()
            .Database.MigrateAsync();

        ConfigurationDbContext configContext = services
            .GetRequiredService<ConfigurationDbContext>();

        await configContext.Database.MigrateAsync();
        await SyncConfigurationAsync(configContext, app.Configuration);

        // Seed тільки в dev, але перевіряємо результат
        if (app.Environment.IsDevelopment())
            await SeedUsersAsync(services, app.Configuration);
    }

    private static async Task SyncConfigurationAsync(
        ConfigurationDbContext configContext,
        IConfiguration configuration)
    {
        Config config = new(configuration);

        // --- Clients ---
        foreach (var client in config.Clients)
        {
            var existing = await configContext.Clients
                .FirstOrDefaultAsync(c => c.ClientId == client.ClientId);

            if (existing is null)
            {
                configContext.Clients.Add(client.ToEntity());
            }
            else
            {
                var entity = client.ToEntity();
                existing.AccessTokenLifetime = entity.AccessTokenLifetime;
                existing.IdentityTokenLifetime = entity.IdentityTokenLifetime;
                existing.AbsoluteRefreshTokenLifetime = entity.AbsoluteRefreshTokenLifetime;
                existing.SlidingRefreshTokenLifetime = entity.SlidingRefreshTokenLifetime;
                existing.RefreshTokenUsage = entity.RefreshTokenUsage;
                existing.RefreshTokenExpiration = entity.RefreshTokenExpiration;
                existing.AllowOfflineAccess = entity.AllowOfflineAccess;
                existing.RequirePkce = entity.RequirePkce;
                existing.RequireClientSecret = entity.RequireClientSecret;
            }
        }

        // --- ApiScopes ---
        foreach (var apiScope in config.ApiScopes)
        {
            var existing = await configContext.ApiScopes
                .Include(s => s.UserClaims)
                .FirstOrDefaultAsync(s => s.Name == apiScope.Name);

            if (existing is null)
            {
                configContext.ApiScopes.Add(apiScope.ToEntity());
            }
            else
            {
                existing.DisplayName = apiScope.DisplayName;
                existing.UserClaims.Clear();

                foreach (string claim in apiScope.UserClaims)
                    existing.UserClaims.Add(new() { Type = claim });
            }
        }

        // --- ApiResources ---
        foreach (var resource in config.ApiResources)
        {
            bool exists = await configContext.ApiResources
                .AnyAsync(r => r.Name == resource.Name);

            if (!exists)
                configContext.ApiResources.Add(resource.ToEntity());
        }

        // --- IdentityResources ---
        foreach (var resource in config.IdentityResources)
        {
            bool exists = await configContext.IdentityResources
                .AnyAsync(r => r.Name == resource.Name);

            if (!exists)
                configContext.IdentityResources.Add(resource.ToEntity());
        }

        await configContext.SaveChangesAsync();
    }

    private static async Task SeedUsersAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        Config config = new(configuration);

        // Спочатку створюємо ролі — бо AddToRoleAsync потребує їх існування
        string[] roles = ["admin", "user"];
        foreach (string role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        foreach (var testUser in config.TestUsers)
        {
            if (await userManager.FindByNameAsync(testUser.Username) is not null)
                continue;

            var user = new ApplicationUser
            {
                UserName = testUser.Username,
                Email = testUser.Claims
                    .FirstOrDefault(c => c.Type == JwtClaimTypes.Email)?.Value,
                EmailConfirmed = true, // без цього логін може блокуватись
            };

            // Проблема: було без перевірки результату — помилки проковтувались мовчки
            var createResult = await userManager.CreateAsync(user, testUser.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException(
                    $"Failed to create user '{testUser.Username}': {errors}");
            }

            // Проблема: було AddClaimsAsync(user, testUser.Claims) — але Claims містить
            // і role claim, яку треба додавати через AddToRoleAsync, а не через claims.
            // Якщо додати "role" як звичайний claim — ASP.NET Identity не буде його
            // враховувати в IsInRole(), GetRolesAsync() тощо.
            var nonRoleClaims = testUser.Claims
                .Where(c => c.Type != JwtClaimTypes.Role)
                .ToList();

            if (nonRoleClaims.Count > 0)
            {
                var claimsResult = await userManager.AddClaimsAsync(user, nonRoleClaims);
                if (!claimsResult.Succeeded)
                {
                    var errors = string.Join(", ", claimsResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException(
                        $"Failed to add claims for '{testUser.Username}': {errors}");
                }
            }

            // Ролі — через правильний механізм Identity
            var userRoles = testUser.Claims
                .Where(c => c.Type == JwtClaimTypes.Role)
                .Select(c => c.Value);

            foreach (string role in userRoles)
            {
                var roleResult = await userManager.AddToRoleAsync(user, role);
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new InvalidOperationException(
                        $"Failed to add role '{role}' to '{testUser.Username}': {errors}");
                }
            }
        }
    }
}