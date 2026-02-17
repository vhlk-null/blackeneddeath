using Library.API.Models;

namespace Library.API.Data.Seed
{
    public static class AlbumSeed
    {
        public static List<Album> GetAlbums()
        {
            return new List<Album>
            {
                new Album
                {
                    Id = SeedConstants.Albums.TransilvanianHunger,
                    Title = "Transilvanian Hunger",
                    ReleaseDate = 1994,
                    Type = AlbumType.FullLength,
                    Format = AlbumFormat.CD,
                    Label = "Peaceville Records",
                    CoverUrl = "https://example.com/covers/transilvanian-hunger.jpg"
                },
                new Album
                {
                    Id = SeedConstants.Albums.Filosofem,
                    Title = "Filosofem",
                    ReleaseDate = 1996,
                    Type = AlbumType.FullLength,
                    Format = AlbumFormat.CD,
                    Label = "Misanthropy Records",
                    CoverUrl = "https://example.com/covers/filosofem.jpg"
                },
                new Album
                {
                    Id = SeedConstants.Albums.NightsideEclipse,
                    Title = "In the Nightside Eclipse",
                    ReleaseDate = 1994,
                    Type = AlbumType.FullLength,
                    Format = AlbumFormat.CD,
                    Label = "Candlelight Records",
                    CoverUrl = "https://example.com/covers/in-the-nightside-eclipse.jpg"
                },
                new Album
                {
                    Id = SeedConstants.Albums.DeMysteriis,
                    Title = "De Mysteriis Dom Sathanas",
                    ReleaseDate = 1994,
                    Type = AlbumType.FullLength,
                    Format = AlbumFormat.CD,
                    Label = "Deathlike Silence Productions",
                    CoverUrl = "https://example.com/covers/de-mysteriis-dom-sathanas.jpg"
                },
                new Album
                {
                    Id = SeedConstants.Albums.StormOfLightsBane,
                    Title = "Storm of the Light's Bane",
                    ReleaseDate = 1995,
                    Type = AlbumType.FullLength,
                    Format = AlbumFormat.CD,
                    Label = "Nuclear Blast",
                    CoverUrl = "https://example.com/covers/storm-of-the-lights-bane.jpg"
                },
                new Album
                {
                    Id = SeedConstants.Albums.TheSatanist,
                    Title = "The Satanist",
                    ReleaseDate = 2014,
                    Type = AlbumType.FullLength,
                    Format = AlbumFormat.CD,
                    Label = "Nuclear Blast",
                    CoverUrl = "https://example.com/covers/the-satanist.jpg"
                }
            };
        }
    }
}
