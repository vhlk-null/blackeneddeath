namespace UserContent.API.Data.Seed
{
    public static class BandSeed
    {
        public static List<Band> GetBands()
        {
            return new List<Band>
            {
                new Band
                {
                    BandId = SeedConstants.LibraryBands.Darkthrone,
                    BandName = "Darkthrone",
                    LogoUrl = "https://example.com/logos/darkthrone.png",
                    ReleaseDate = 1997
                },
                new Band
                {
                    BandId = SeedConstants.LibraryBands.Burzum,
                    BandName = "Burzum",
                    LogoUrl = "https://example.com/logos/burzum.png",
                    ReleaseDate = 1997
                },
                new Band
                {
                    BandId = SeedConstants.LibraryBands.Emperor,
                    BandName = "Emperor",
                    LogoUrl = "https://example.com/logos/emperor.png",
                    ReleaseDate = 1997
                },
                new Band
                {
                    BandId = SeedConstants.LibraryBands.Mayhem,
                    BandName = "Mayhem",
                    LogoUrl = "https://example.com/logos/mayhem.png",
                    ReleaseDate = 1997
                },
                new Band
                {
                    BandId = SeedConstants.LibraryBands.Behemoth,
                    BandName = "Behemoth",
                    LogoUrl = "https://example.com/logos/behemoth.png",
                    ReleaseDate = 1997
                },
                new Band
                {
                    BandId = SeedConstants.LibraryBands.Dissection,
                    BandName = "Dissection",
                    LogoUrl = "https://example.com/logos/dissection.png",
                    ReleaseDate = 1997
                }
            };
        }
    }
}
