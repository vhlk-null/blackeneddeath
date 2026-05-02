namespace IdentityServer;

public class Config(IConfiguration configuration)
{
    public IEnumerable<Client> Clients
    {
        get
        {
            // Зчитуємо базові дані клієнта з appsettings (ClientId, URIs, etc.)
            var rawClients = configuration
                .GetSection("IdentityServer:Clients")
                .Get<List<Client>>() ?? [];

            foreach (var client in rawClients)
            {
                // Спільні налаштування для SPA клієнтів — тут можна додати
                // перевірку типу клієнта якщо їх буде кілька
                client.AllowedGrantTypes = GrantTypes.Code;
                client.RequirePkce = true;
                client.RequireClientSecret = false;
                client.AllowOfflineAccess = true;

                client.AllowedScopes =
                [
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    Scopes.Roles,
                    Scopes.LibraryApi,
                ];

                // Безпека: обмежуємо час життя токенів
                client.AccessTokenLifetime = 3600;        // 1 година
                client.IdentityTokenLifetime = 300;       // 5 хвилин
                client.AbsoluteRefreshTokenLifetime = 2592000;  // 30 днів
                client.SlidingRefreshTokenLifetime = 86400;     // 24 години
                client.RefreshTokenUsage = TokenUsage.OneTimeOnly; // безпечніше ніж ReUse
                client.RefreshTokenExpiration = TokenExpiration.Sliding;

                // CORS — якщо не задано в appsettings, використовуємо AllowedOrigins
                if (client.AllowedCorsOrigins?.Any() != true)
                {
                    var origins = configuration
                        .GetSection("Cors:AllowedOrigins")
                        .Get<string[]>() ?? [];
                    client.AllowedCorsOrigins = origins.ToList();
                }
            }

            return rawClients;
        }
    }

    public IEnumerable<ApiScope> ApiScopes =>
    [
        new(Scopes.LibraryApi, "Blackeneddeath API")
        {
            // Важливо: щоб role потрапляла в access token
            UserClaims = [JwtClaimTypes.Role]
        }
    ];

    public IEnumerable<ApiResource> ApiResources =>
    [
        new(Scopes.LibraryApi, "Blackeneddeath API")
        {
            Scopes = [Scopes.LibraryApi],
            UserClaims = [JwtClaimTypes.Role]
        }
    ];

    public IEnumerable<IdentityResource> IdentityResources =>
    [
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email(),
        // Важливо: name "roles" і claim type "role" — різні речі
        // "roles" — це назва scope
        // "role" (JwtClaimTypes.Role) — це назва claim у токені
        new(
            name: Scopes.Roles,
            displayName: "Your role(s)",
            userClaims: [JwtClaimTypes.Role])
    ];

    // TestUsers використовується ТІЛЬКИ для seed в БД,
    // після чого ASP.NET Identity керує користувачами
    public List<TestUser> TestUsers =>
    [
        new()
        {
            SubjectId = "1",
            Username = "alice",
            Password = "Alice@7392!",
            Claims =
            [
                new Claim(JwtClaimTypes.Name, "Alice Smith"),
                new Claim(JwtClaimTypes.Email, "alice@example.com"),
                new Claim(JwtClaimTypes.Role, "user"),
            ]
        },
        new()
        {
            SubjectId = "2",
            Username = "bob",
            Password = "Bob#5814@",
            Claims =
            [
                new Claim(JwtClaimTypes.Name, "Bob Jones"),
                new Claim(JwtClaimTypes.Email, "bob@example.com"),
                new Claim(JwtClaimTypes.Role, "admin"),
            ]
        }
    ];
}