namespace UserContent.API.Data.Seed
{
    public static class FavoriteBandSeed
    {
        public static List<FavoriteBand> GetFavoriteBands()
        {
            return new List<FavoriteBand>
            {
                // MetalHead666's favorite bands
                new FavoriteBand
                {
                    Id = SeedConstants.FavoriteBands.MetalHead666_Darkthrone,
                    UserId = SeedConstants.Users.MetalHead666,
                    BandId = SeedConstants.ArchiveBands.Darkthrone,
                    BandName = "Darkthrone",
                    LogoUrl = "https://example.com/logos/darkthrone.png",
                    AddedDate = new DateTime(2023, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                    ReleaseDate = 1997,
                    IsFollowing = true
                },
                new FavoriteBand
                {
                    Id = SeedConstants.FavoriteBands.MetalHead666_Burzum,
                    UserId = SeedConstants.Users.MetalHead666,
                    BandId = SeedConstants.ArchiveBands.Burzum,
                    BandName = "Burzum",
                    LogoUrl = "https://example.com/logos/burzum.png",
                    AddedDate = new DateTime(2023, 1, 25, 0, 0, 0, DateTimeKind.Utc),
                    ReleaseDate = 1997,
                    IsFollowing = true
                },

                // DarkSoulFan's favorite bands
                new FavoriteBand
                {
                    Id = SeedConstants.FavoriteBands.DarkSoulFan_Emperor,
                    UserId = SeedConstants.Users.DarkSoulFan,
                    BandId = SeedConstants.ArchiveBands.Emperor,
                    BandName = "Emperor",
                    LogoUrl = "https://example.com/logos/emperor.png",
                    AddedDate = new DateTime(2022, 6, 15, 0, 0, 0, DateTimeKind.Utc),
                    ReleaseDate = 1997,
                    IsFollowing = true
                },
                new FavoriteBand
                {
                    Id = SeedConstants.FavoriteBands.DarkSoulFan_Mayhem,
                    UserId = SeedConstants.Users.DarkSoulFan,
                    BandId = SeedConstants.ArchiveBands.Mayhem,
                    BandName = "Mayhem",
                    LogoUrl = "https://example.com/logos/mayhem.png",
                    AddedDate = new DateTime(2022, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                    ReleaseDate = 1997,
                    IsFollowing = true
                },

                // BlastBeatLover's favorite bands
                new FavoriteBand
                {
                    Id = SeedConstants.FavoriteBands.BlastBeatLover_Behemoth,
                    UserId = SeedConstants.Users.BlastBeatLover,
                    BandId = SeedConstants.ArchiveBands.Behemoth,
                    BandName = "Behemoth",
                    LogoUrl = "https://example.com/logos/behemoth.png",
                    AddedDate = new DateTime(2024, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                    ReleaseDate = 1997,
                    IsFollowing = true
                },
                new FavoriteBand
                {
                    Id = SeedConstants.FavoriteBands.BlastBeatLover_Dissection,
                    UserId = SeedConstants.Users.BlastBeatLover,
                    BandId = SeedConstants.ArchiveBands.Dissection,
                    BandName = "Dissection",
                    LogoUrl = "https://example.com/logos/dissection.png",
                    AddedDate = new DateTime(2024, 3, 20, 0, 0, 0, DateTimeKind.Utc),
                    ReleaseDate = 1997,
                    IsFollowing = false
                }
            };
        }
    }
}
