using UserContent.API.Models;

namespace UserContent.API.Data.Seed
{
    public static class FavoriteAlbumSeed
    {
        public static List<FavoriteAlbum> GetFavoriteAlbums()
        {
            return new List<FavoriteAlbum>
            {
                // MetalHead666's favorites
                new FavoriteAlbum
                {
                    Id = SeedConstants.FavoriteAlbums.MetalHead666_TransilvanianHunger,
                    UserId = SeedConstants.Users.MetalHead666,
                    AlbumId = SeedConstants.ArchiveAlbums.TransilvanianHunger,
                    AlbumTitle = "Transilvanian Hunger",
                    CoverUrl = "https://example.com/covers/transilvanian-hunger.jpg",
                    ReleaseYear = 1994,
                    AddedDate = new DateTime(2023, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    UserRating = 10,
                    UserReview = "The epitome of raw black metal. Hypnotic and relentless."
                },
                new FavoriteAlbum
                {
                    Id = SeedConstants.FavoriteAlbums.MetalHead666_Filosofem,
                    UserId = SeedConstants.Users.MetalHead666,
                    AlbumId = SeedConstants.ArchiveAlbums.Filosofem,
                    AlbumTitle = "Filosofem",
                    CoverUrl = "https://example.com/covers/filosofem.jpg",
                    ReleaseYear = 1996,
                    AddedDate = new DateTime(2023, 2, 15, 0, 0, 0, DateTimeKind.Utc),
                    UserRating = 9,
                    UserReview = "Atmospheric masterpiece. Dunkelheit is haunting."
                },

                // DarkSoulFan's favorites
                new FavoriteAlbum
                {
                    Id = SeedConstants.FavoriteAlbums.DarkSoulFan_NightsideEclipse,
                    UserId = SeedConstants.Users.DarkSoulFan,
                    AlbumId = SeedConstants.ArchiveAlbums.NightsideEclipse,
                    AlbumTitle = "In the Nightside Eclipse",
                    CoverUrl = "https://example.com/covers/nightside-eclipse.jpg",
                    ReleaseYear = 1994,
                    AddedDate = new DateTime(2022, 7, 10, 0, 0, 0, DateTimeKind.Utc),
                    UserRating = 10,
                    UserReview = "Perfect symphonic black metal. The keyboards create an otherworldly atmosphere."
                },
                new FavoriteAlbum
                {
                    Id = SeedConstants.FavoriteAlbums.DarkSoulFan_DeMysteriis,
                    UserId = SeedConstants.Users.DarkSoulFan,
                    AlbumId = SeedConstants.ArchiveAlbums.DeMysteriis,
                    AlbumTitle = "De Mysteriis Dom Sathanas",
                    CoverUrl = "https://example.com/covers/de-mysteriis.jpg",
                    ReleaseYear = 1994,
                    AddedDate = new DateTime(2022, 8, 5, 0, 0, 0, DateTimeKind.Utc),
                    UserRating = 10,
                    UserReview = "The most influential black metal album ever. Attila's vocals are unique."
                },

                // BlastBeatLover's favorites
                new FavoriteAlbum
                {
                    Id = SeedConstants.FavoriteAlbums.BlastBeatLover_TheSatanist,
                    UserId = SeedConstants.Users.BlastBeatLover,
                    AlbumId = SeedConstants.ArchiveAlbums.TheSatanist,
                    AlbumTitle = "The Satanist",
                    CoverUrl = "https://example.com/covers/the-satanist.jpg",
                    ReleaseYear = 2014,
                    AddedDate = new DateTime(2024, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                    UserRating = 9,
                    UserReview = "Behemoth at their peak. Blow Your Trumpets Gabriel is incredible."
                }
            };
        }
    }
}
