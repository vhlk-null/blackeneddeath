namespace Archive.API.Data.Seeds
{
    public static class AlbumSeed
    {
        public static List<Album> GetAlbums()
        {
            return new List<Album>
            {
                new Album
                {
                    Id = Guid.Parse("a0000000-0000-0000-0000-000000000001"),
                    Title = "Transilvanian Hunger",
                    ReleaseDate = 1994,
                    Type = AlbumType.FullLength,
                    Format = AlbumFormat.CD,
                    Label = "Peaceville Records",
                    CoverUrl = null
                },
                new Album
                {
                    Id = Guid.Parse("a0000000-0000-0000-0000-000000000002"),
                    Title = "Filosofem",
                    ReleaseDate = 1996,
                    Type = AlbumType.FullLength,
                    Format = AlbumFormat.CD,
                    Label = "Misanthropy Records",
                    CoverUrl = null
                },
                new Album
                {
                    Id = Guid.Parse("a0000000-0000-0000-0000-000000000003"),
                    Title = "In the Nightside Eclipse",
                    ReleaseDate = 1994,
                    Type = AlbumType.FullLength,
                    Format = AlbumFormat.CD,
                    Label = "Candlelight Records",
                    CoverUrl = null
                },
                new Album
                {
                    Id = Guid.Parse("a0000000-0000-0000-0000-000000000004"),
                    Title = "De Mysteriis Dom Sathanas",
                    ReleaseDate = 1994,
                    Type = AlbumType.FullLength,
                    Format = AlbumFormat.CD,
                    Label = "Deathlike Silence Productions",
                    CoverUrl = null
                },
                new Album
                {
                    Id = Guid.Parse("a0000000-0000-0000-0000-000000000005"),
                    Title = "Storm of the Light's Bane",
                    ReleaseDate = 1995,
                    Type = AlbumType.FullLength,
                    Format = AlbumFormat.CD,
                    Label = "Nuclear Blast",
                    CoverUrl = null
                },
                new Album
                {
                    Id = Guid.Parse("a0000000-0000-0000-0000-000000000006"),
                    Title = "The Satanist",
                    ReleaseDate = 2014,
                    Type = AlbumType.FullLength,
                    Format = AlbumFormat.CD,
                    Label = "Nuclear Blast",
                    CoverUrl = null
                }
            };
        }
    }
}