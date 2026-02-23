using Library.Domain.Models;

namespace Library.API.Data.Seed
{
    public static class BandSeed
    {
        public static List<Band> GetBands()
        {
            return new List<Band>
            {
                new Band
                {
                    Id = SeedConstants.Bands.Darkthrone,
                    Name = "Darkthrone",
                    Bio = "Norwegian black metal band formed in 1986.",
                    CountryId = SeedConstants.Countries.Norway,
                    LogoUrl = "https://example.com/logos/darkthrone.png",
                    FormedYear = 1986,
                    Status = BandStatus.Active
                },
                new Band
                {
                    Id = SeedConstants.Bands.Burzum,
                    Name = "Burzum",
                    Bio = "Norwegian black metal solo project by Varg Vikernes.",
                    CountryId = SeedConstants.Countries.Norway,
                    LogoUrl = "https://example.com/logos/burzum.png",
                    FormedYear = 1991,
                    Status = BandStatus.OnHold
                },
                new Band
                {
                    Id = SeedConstants.Bands.Emperor,
                    Name = "Emperor",
                    Bio = "Norwegian black metal band, pioneers of symphonic black metal.",
                    CountryId = SeedConstants.Countries.Norway,
                    LogoUrl = "https://example.com/logos/emperor.png",
                    FormedYear = 1991,
                    DisbandedYear = 2001,
                    Status = BandStatus.SplitUp
                },
                new Band
                {
                    Id = SeedConstants.Bands.Mayhem,
                    Name = "Mayhem",
                    Bio = "Norwegian black metal band, one of the genre's founders.",
                    CountryId = SeedConstants.Countries.Norway,
                    LogoUrl = "https://example.com/logos/mayhem.png",
                    FormedYear = 1984,
                    Status = BandStatus.Active
                },
                new Band
                {
                    Id = SeedConstants.Bands.Dissection,
                    Name = "Dissection",
                    Bio = "Swedish melodic black/death metal band.",
                    CountryId = SeedConstants.Countries.Sweden,
                    LogoUrl = "https://example.com/logos/dissection.png",
                    FormedYear = 1989,
                    DisbandedYear = 2006,
                    Status = BandStatus.SplitUp
                },
                new Band
                {
                    Id = SeedConstants.Bands.Behemoth,
                    Name = "Behemoth",
                    Bio = "Polish blackened death metal band.",
                    CountryId = SeedConstants.Countries.Poland,
                    LogoUrl = "https://example.com/logos/behemoth.png",
                    FormedYear = 1991,
                    Status = BandStatus.Active
                }
            };
        }
    }
}
