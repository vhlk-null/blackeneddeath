namespace UserContent.Infrastructure.Data.Seed;

public static class FavoriteAlbumSeed
{
    public static List<FavoriteAlbum> GetFavoriteAlbums()
    {
        return new List<FavoriteAlbum>
        {
            new FavoriteAlbum { UserId = SeedConstants.Users.MetalHead666, AlbumId = SeedConstants.LibraryAlbums.TransilvanianHunger, AddedDate = new DateTime(2023, 2, 1, 0, 0, 0, DateTimeKind.Utc), UserRating = 10, UserReview = "The epitome of raw black metal. Hypnotic and relentless." },
            new FavoriteAlbum { UserId = SeedConstants.Users.MetalHead666, AlbumId = SeedConstants.LibraryAlbums.Filosofem, AddedDate = new DateTime(2023, 2, 15, 0, 0, 0, DateTimeKind.Utc), UserRating = 9, UserReview = "Atmospheric masterpiece. Dunkelheit is haunting." },
            new FavoriteAlbum { UserId = SeedConstants.Users.DarkSoulFan, AlbumId = SeedConstants.LibraryAlbums.NightsideEclipse, AddedDate = new DateTime(2022, 7, 10, 0, 0, 0, DateTimeKind.Utc), UserRating = 10, UserReview = "Perfect symphonic black metal. The keyboards create an otherworldly atmosphere." },
            new FavoriteAlbum { UserId = SeedConstants.Users.DarkSoulFan, AlbumId = SeedConstants.LibraryAlbums.DeMysteriis, AddedDate = new DateTime(2022, 8, 5, 0, 0, 0, DateTimeKind.Utc), UserRating = 10, UserReview = "The most influential black metal album ever. Attila's vocals are unique." },
            new FavoriteAlbum { UserId = SeedConstants.Users.BlastBeatLover, AlbumId = SeedConstants.LibraryAlbums.TheSatanist, AddedDate = new DateTime(2024, 4, 1, 0, 0, 0, DateTimeKind.Utc), UserRating = 9, UserReview = "Behemoth at their peak. Blow Your Trumpets Gabriel is incredible." }
        };
    }
}
