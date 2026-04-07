namespace IdentityServer
{
    public class Config(IConfiguration configuration)
    {
        public IEnumerable<Client> Clients
        {
            get
            {
                var clients = new List<Client>();
                configuration.GetSection("IdentityServer:Clients").Bind(clients);

                foreach (var client in clients)
                {
                    client.AllowedGrantTypes = GrantTypes.Code;
                    client.RequirePkce = true;
                    client.RequireClientSecret = false;
                    client.AllowOfflineAccess = true;
                    client.AllowedScopes =
                    [
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
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
        ];

        public IEnumerable<IdentityResource> IdentityResources =>
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        ];

        public List<TestUser> TestUsers =>
        [
            new()
            {
                SubjectId = "1",
                Username = "alice",
                Password = "alice",
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
                Password = "bob",
                Claims =
                [
                    new(JwtClaimTypes.Name, "Bob Jones"),
                    new(JwtClaimTypes.Email, "bob@example.com")
                ]
            }
        ];
    }
}
