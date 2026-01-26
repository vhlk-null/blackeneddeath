public static class CountrySeed
{
    public static List<Country> GetCountries()
    {
        return new List<Country>
        {
            new Country { Id = Guid.NewGuid(), Name = "Norway", Code = "NO" },
            new Country { Id = Guid.NewGuid(), Name = "Sweden", Code = "SE" },
            new Country { Id = Guid.NewGuid(), Name = "Finland", Code = "FI" },
            new Country { Id = Guid.NewGuid(), Name = "Poland", Code = "PL" },
            new Country { Id = Guid.NewGuid(), Name = "Ukraine", Code = "UA" },
            new Country { Id = Guid.NewGuid(), Name = "United States", Code = "US" },
            new Country { Id = Guid.NewGuid(), Name = "United Kingdom", Code = "GB" },
            new Country { Id = Guid.NewGuid(), Name = "Germany", Code = "DE" },
            new Country { Id = Guid.NewGuid(), Name = "France", Code = "FR" }
        };
    }
}