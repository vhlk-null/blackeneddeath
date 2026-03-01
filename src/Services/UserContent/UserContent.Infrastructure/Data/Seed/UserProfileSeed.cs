namespace UserContent.Infrastructure.Data.Seed;

public static class UserProfileSeed
{
    public static List<UserProfileInfo> GetUserProfiles()
    {
        return new List<UserProfileInfo>
        {
            new UserProfileInfo
            {
                UserId = SeedConstants.Users.MetalHead666,
                Username = "MetalHead666",
                Email = "metalhead666@example.com",
                AvatarUrl = "https://example.com/avatars/metalhead666.jpg",
                RegisteredDate = new DateTime(2023, 1, 15, 0, 0, 0, DateTimeKind.Utc),
                LastLoginDate = new DateTime(2024, 12, 20, 0, 0, 0, DateTimeKind.Utc),
                Bio = "Norwegian black metal enthusiast since the 90s. Darkthrone and Burzum forever!"
            },
            new UserProfileInfo
            {
                UserId = SeedConstants.Users.DarkSoulFan,
                Username = "DarkSoulFan",
                Email = "darksoul@example.com",
                AvatarUrl = "https://example.com/avatars/darksoul.jpg",
                RegisteredDate = new DateTime(2022, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                LastLoginDate = new DateTime(2024, 12, 18, 0, 0, 0, DateTimeKind.Utc),
                Bio = "Symphonic black metal lover. Emperor's In the Nightside Eclipse changed my life."
            },
            new UserProfileInfo
            {
                UserId = SeedConstants.Users.BlastBeatLover,
                Username = "BlastBeatLover",
                Email = "blastbeat@example.com",
                AvatarUrl = "https://example.com/avatars/blastbeat.jpg",
                RegisteredDate = new DateTime(2024, 3, 10, 0, 0, 0, DateTimeKind.Utc),
                LastLoginDate = new DateTime(2024, 12, 21, 0, 0, 0, DateTimeKind.Utc),
                Bio = "Blackened death metal fan. Behemoth and Dissection are my jam."
            }
        };
    }
}
