namespace Archive.API.Data.Seeds
{
    public static class CountrySeed
    {
        public static List<Country> GetCountries()
        {
        return new List<Country>
        {
            new Country
            {
                Id = SeedConstants.Countries.Norway,
                Name = "Norway",
                Code = "NO"
            },
            new Country
            {
                Id = SeedConstants.Countries.Sweden,
                Name = "Sweden",
                Code = "SE"
            },
            new Country
            {
                Id = SeedConstants.Countries.Finland,
                Name = "Finland",
                Code = "FI"
            },
            new Country
            {
                Id = SeedConstants.Countries.Poland,
                Name = "Poland",
                Code = "PL"
            },
            new Country
            {
                Id = SeedConstants.Countries.Ukraine,
                Name = "Ukraine",
                Code = "UA"
            },
            new Country
            {
                Id = SeedConstants.Countries.UnitedStates,
                Name = "United States",
                Code = "US"
            },
            new Country
            {
                Id = SeedConstants.Countries.UnitedKingdom,
                Name = "United Kingdom",
                Code = "GB"
            },
            new Country
            {
                Id = SeedConstants.Countries.Germany,
                Name = "Germany",
                Code = "DE"
            },
            new Country
            {
                Id = SeedConstants.Countries.France,
                Name = "France",
                Code = "FR"
            }
        };
        }
    }
}