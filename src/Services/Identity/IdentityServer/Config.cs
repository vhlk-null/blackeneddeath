namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<Client> Clients =>
        [
            new()
            {
                ClientId = "libraryClient",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "libraryAPI" }
            }
        ];

        public static IEnumerable<ApiScope> ApiScopes =>
        [
            new("libraryAPI","Library API")
        ];
        
        public static IEnumerable<ApiResource> ApiResources =>
        [

        ];
        
        public static IEnumerable<IdentityResource> IdentityResources =>
        [

        ];

        public static List<TestUser> TestUsers =>
        [
            new TestUser { SubjectId = "1", Username = "Alice", Password = "password" },
            new TestUser { SubjectId = "2", Username = "Bob", Password = "password" }
        ];

    }
}
