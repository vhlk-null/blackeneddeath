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
                    UserId = SeedConstants.Users.MetalHead666,
                    BandId = SeedConstants.ArchiveBands.Darkthrone,
                    AddedDate = new DateTime(2023, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                    IsFollowing = true
                },
                new FavoriteBand
                {
                    UserId = SeedConstants.Users.MetalHead666,
                    BandId = SeedConstants.ArchiveBands.Burzum,
                    AddedDate = new DateTime(2023, 1, 25, 0, 0, 0, DateTimeKind.Utc),
                    IsFollowing = true
                },

                // DarkSoulFan's favorite bands
                new FavoriteBand
                {
                    UserId = SeedConstants.Users.DarkSoulFan,
                    BandId = SeedConstants.ArchiveBands.Emperor,
                    AddedDate = new DateTime(2022, 6, 15, 0, 0, 0, DateTimeKind.Utc),
                    IsFollowing = true
                },
                new FavoriteBand
                {
                    UserId = SeedConstants.Users.DarkSoulFan,
                    BandId = SeedConstants.ArchiveBands.Mayhem,
                    AddedDate = new DateTime(2022, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                    IsFollowing = true
                },

                // BlastBeatLover's favorite bands
                new FavoriteBand
                {
                    UserId = SeedConstants.Users.BlastBeatLover,
                    BandId = SeedConstants.ArchiveBands.Behemoth,
                    AddedDate = new DateTime(2024, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                    IsFollowing = true
                },
                new FavoriteBand
                {
                    UserId = SeedConstants.Users.BlastBeatLover,
                    BandId = SeedConstants.ArchiveBands.Dissection,
                    AddedDate = new DateTime(2024, 3, 20, 0, 0, 0, DateTimeKind.Utc),
                    IsFollowing = false
                }
            };
        }
    }
}
