public static class CountrySeed
{
    public static List<Country> GetCountries()
    {
        return new List<Country>
        {
            new Country
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000001"),
                Name = "Norway",
                Code = "NO"
            },
            new Country
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000002"),
                Name = "Sweden",
                Code = "SE"
            },
            new Country
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000003"),
                Name = "Finland",
                Code = "FI"
            },
            new Country
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000004"),
                Name = "Poland",
                Code = "PL"
            },
            new Country
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000005"),
                Name = "Ukraine",
                Code = "UA"
            },
            new Country
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000006"),
                Name = "United States",
                Code = "US"
            },
            new Country
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000007"),
                Name = "United Kingdom",
                Code = "GB"
            },
            new Country
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000008"),
                Name = "Germany",
                Code = "DE"
            },
            new Country
            {
                Id = Guid.Parse("c0000000-0000-0000-0000-000000000009"),
                Name = "France",
                Code = "FR"
            }
        };
    }
}