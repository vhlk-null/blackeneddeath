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
                    AddedDate = new DateTime(2023, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    UserRating = 10,
                    UserReview = "The epitome of raw black metal. Hypnotic and relentless."
                },
                new FavoriteAlbum
                {
                    Id = SeedConstants.FavoriteAlbums.MetalHead666_Filosofem,
                    UserId = SeedConstants.Users.MetalHead666,
                    AlbumId = SeedConstants.ArchiveAlbums.Filosofem,
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
                    AddedDate = new DateTime(2022, 7, 10, 0, 0, 0, DateTimeKind.Utc),
                    UserRating = 10,
                    UserReview = "Perfect symphonic black metal. The keyboards create an otherworldly atmosphere."
                },
                new FavoriteAlbum
                {
                    Id = SeedConstants.FavoriteAlbums.DarkSoulFan_DeMysteriis,
                    UserId = SeedConstants.Users.DarkSoulFan,
                    AlbumId = SeedConstants.ArchiveAlbums.DeMysteriis,
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
                    AddedDate = new DateTime(2024, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                    UserRating = 9,
                    UserReview = "Behemoth at their peak. Blow Your Trumpets Gabriel is incredible."
                }
            };
        }
    }
}
