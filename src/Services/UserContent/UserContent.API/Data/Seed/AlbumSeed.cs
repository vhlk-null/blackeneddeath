namespace UserContent.API.Data.Seed
{
    public static class AlbumSeed
    {
        public static List<Album> GetAlbums()
        {
            return new List<Album>
            {
                new Album
                {
                    Id = SeedConstants.LibraryAlbums.TransilvanianHunger,
                    Title = "Transilvanian Hunger",
                    CoverUrl = "https://example.com/covers/transilvanian-hunger.jpg",
                    ReleaseDate = 1994
                },
                new Album
                {
                    Id = SeedConstants.LibraryAlbums.Filosofem,
                    Title = "Filosofem",
                    CoverUrl = "https://example.com/covers/filosofem.jpg",
                    ReleaseDate = 1996
                },
                new Album
                {
                    Id = SeedConstants.LibraryAlbums.NightsideEclipse,
                    Title = "In the Nightside Eclipse",
                    CoverUrl = "https://example.com/covers/nightside-eclipse.jpg",
                    ReleaseDate = 1994
                },
                new Album
                {
                    Id = SeedConstants.LibraryAlbums.DeMysteriis,
                    Title = "De Mysteriis Dom Sathanas",
                    CoverUrl = "https://example.com/covers/de-mysteriis.jpg",
                    ReleaseDate = 1994
                },
                new Album
                {
                    Id = SeedConstants.LibraryAlbums.TheSatanist,
                    Title = "The Satanist",
                    CoverUrl = "https://example.com/covers/the-satanist.jpg",
                    ReleaseDate = 2014
                }
            };
        }
    }
}
