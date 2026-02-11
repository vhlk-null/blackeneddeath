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
                    BandId = SeedConstants.ArchiveBands.Darkthrone,
                    BandName = "Darkthrone",
                    LogoUrl = "https://example.com/logos/darkthrone.png",
                    ReleaseDate = 1997
                },
                new Band
                {
                    BandId = SeedConstants.ArchiveBands.Burzum,
                    BandName = "Burzum",
                    LogoUrl = "https://example.com/logos/burzum.png",
                    ReleaseDate = 1997
                },
                new Band
                {
                    BandId = SeedConstants.ArchiveBands.Emperor,
                    BandName = "Emperor",
                    LogoUrl = "https://example.com/logos/emperor.png",
                    ReleaseDate = 1997
                },
                new Band
                {
                    BandId = SeedConstants.ArchiveBands.Mayhem,
                    BandName = "Mayhem",
                    LogoUrl = "https://example.com/logos/mayhem.png",
                    ReleaseDate = 1997
                },
                new Band
                {
                    BandId = SeedConstants.ArchiveBands.Behemoth,
                    BandName = "Behemoth",
                    LogoUrl = "https://example.com/logos/behemoth.png",
                    ReleaseDate = 1997
                },
                new Band
                {
                    BandId = SeedConstants.ArchiveBands.Dissection,
                    BandName = "Dissection",
                    LogoUrl = "https://example.com/logos/dissection.png",
                    ReleaseDate = 1997
                }
            };
        }
    }
}
