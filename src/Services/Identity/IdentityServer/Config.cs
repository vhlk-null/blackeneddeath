namespace IdentityServer
{
    public class Config(IConfiguration configuration)
    {
        public IEnumerable<Client> Clients
        {
            get
            {
                List<Client> clients = new List<Client>();
                configuration.GetSection("IdentityServer:Clients").Bind(clients);

                foreach (Client client in clients)
                {
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
                        Scopes.LibraryApi
                    ];
                }

                return clients;
            }
        }

        public List<ApiScope> ApiScopes =>
        [
            new(Scopes.LibraryApi, "Library API")
        ];

        public IEnumerable<ApiResource> ApiResources =>
        [
            new(Scopes.LibraryApi, "Library API")
            {
                Scopes = { Scopes.LibraryApi },
                UserClaims = { JwtClaimTypes.Role }
            }
        ];

        public IEnumerable<IdentityResource> IdentityResources =>
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource(
                "roles","Your role(s)", new List<string>() {"role"})
        ];

        public List<TestUser> TestUsers =>
        [
            new()
            {
                SubjectId = "1",
                Username = "alice",
                Password = "Alice@7392!",
                Claims =
                [
                    new(JwtClaimTypes.Name, "Alice Smith"),
                    new(JwtClaimTypes.Email, "alice@example.com"),
                    new Claim(JwtClaimTypes.Role, "user")

                ]
            },
            new()
            {
                SubjectId = "2",
                Username = "bob",
                Password = "Bob#5814@",
                Claims =
                [
                    new(JwtClaimTypes.Name, "Bob Jones"),
                    new(JwtClaimTypes.Email, "bob@example.com"),
                    new Claim(JwtClaimTypes.Role, "admin")
                ]
            }
        ];
    }
}
