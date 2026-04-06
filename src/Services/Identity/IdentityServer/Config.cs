namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<Client> Clients =>
        [
            new()
            {
                ClientId = "angular",
                ClientName = "Angular Client",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                AllowedCorsOrigins = { "http://localhost:4200" },
                RedirectUris = { "http://localhost:4200/auth/callback" },
                PostLogoutRedirectUris = { "http://localhost:4200" },
                AllowedScopes = { "openid", "profile", "libraryAPI" },
                AllowOfflineAccess = true
            }
        ];

        public static List<ApiScope> ApiScopes =>
        [
            new("libraryAPI", "Library API")
        ];

        public static IEnumerable<ApiResource> ApiResources =>
        [
            new("libraryAPI", "Library API")
        ];

        public static IEnumerable<IdentityResource> IdentityResources =>
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        ];

        public static List<TestUser> TestUsers =>
        [
            new()
            {
                SubjectId = "1",
                Username = "alice",
                Password = "Password1!",
                Claims =
                [
                    new(JwtClaimTypes.Name, "Alice Smith"),
                    new(JwtClaimTypes.Email, "alice@example.com")
                ]
            },
            new()
            {
                SubjectId = "2",
                Username = "bob",
                Password = "Password1!",
                Claims =
                [
                    new(JwtClaimTypes.Name, "Bob Jones"),
                    new(JwtClaimTypes.Email, "bob@example.com")
                ]
            }
        ];
    }
}
