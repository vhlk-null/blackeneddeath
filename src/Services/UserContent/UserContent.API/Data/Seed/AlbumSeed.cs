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
                    AlbumId = SeedConstants.ArchiveAlbums.TransilvanianHunger,
                    AlbumTitle = "Transilvanian Hunger",
                    CoverUrl = "https://example.com/covers/transilvanian-hunger.jpg",
                    ReleaseDate = 1994
                },
                new Album
                {
                    AlbumId = SeedConstants.ArchiveAlbums.Filosofem,
                    AlbumTitle = "Filosofem",
                    CoverUrl = "https://example.com/covers/filosofem.jpg",
                    ReleaseDate = 1996
                },
                new Album
                {
                    AlbumId = SeedConstants.ArchiveAlbums.NightsideEclipse,
                    AlbumTitle = "In the Nightside Eclipse",
                    CoverUrl = "https://example.com/covers/nightside-eclipse.jpg",
                    ReleaseDate = 1994
                },
                new Album
                {
                    AlbumId = SeedConstants.ArchiveAlbums.DeMysteriis,
                    AlbumTitle = "De Mysteriis Dom Sathanas",
                    CoverUrl = "https://example.com/covers/de-mysteriis.jpg",
                    ReleaseDate = 1994
                },
                new Album
                {
                    AlbumId = SeedConstants.ArchiveAlbums.TheSatanist,
                    AlbumTitle = "The Satanist",
                    CoverUrl = "https://example.com/covers/the-satanist.jpg",
                    ReleaseDate = 2014
                }
            };
        }
    }
}
