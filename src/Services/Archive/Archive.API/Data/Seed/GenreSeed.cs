namespace Archive.API.Data.Seeds
{
    public static class GenreSeed
    {
        public static List<Genre> GetGenres()
        {
            // Main genre
            var metal = new Genre
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                Name = "Metal",
                ParentGenreId = null
            };

            // Sub-genres level 1
            var blackMetal = new Genre
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000001"),
                Name = "Black Metal",
                ParentGenreId = metal.Id
            };

            var deathMetal = new Genre
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000002"),
                Name = "Death Metal",
                ParentGenreId = metal.Id
            };

            var doomMetal = new Genre
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000003"),
                Name = "Doom Metal",
                ParentGenreId = metal.Id
            };

            var thrashMetal = new Genre
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000004"),
                Name = "Thrash Metal",
                ParentGenreId = metal.Id
            };

            // Sub-genres level 2 (Black Metal)
            var rawBlackMetal = new Genre
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000001"),
                Name = "Raw Black Metal",
                ParentGenreId = blackMetal.Id
            };

            var melodicBlackMetal = new Genre
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000002"),
                Name = "Melodic Black Metal",
                ParentGenreId = blackMetal.Id
            };

            var atmosphericBlackMetal = new Genre
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000003"),
                Name = "Atmospheric Black Metal",
                ParentGenreId = blackMetal.Id
            };

            var symphonicBlackMetal = new Genre
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000004"),
                Name = "Symphonic Black Metal",
                ParentGenreId = blackMetal.Id
            };

            var depressiveBlackMetal = new Genre
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000005"),
                Name = "Depressive Black Metal",
                ParentGenreId = blackMetal.Id
            };

            // Sub-genres level 2 (Death Metal)
            var melodicDeathMetal = new Genre
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000006"),
                Name = "Melodic Death Metal",
                ParentGenreId = deathMetal.Id
            };

            var technicalDeathMetal = new Genre
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000007"),
                Name = "Technical Death Metal",
                ParentGenreId = deathMetal.Id
            };

            var brutalDeathMetal = new Genre
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000008"),
                Name = "Brutal Death Metal",
                ParentGenreId = deathMetal.Id
            };

            // Sub-genres level 2 (Doom Metal)
            var funeralDoom = new Genre
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000009"),
                Name = "Funeral Doom",
                ParentGenreId = doomMetal.Id
            };

            var stonerDoom = new Genre
            {
                Id = Guid.Parse("3000000a-0000-0000-0000-000000000000"),
                Name = "Stoner Doom",
                ParentGenreId = doomMetal.Id
            };

            return new List<Genre>
            {
                // Main
                metal,
                
                // Level 1
                blackMetal,
                deathMetal,
                doomMetal,
                thrashMetal,
                
                // Level 2 (Black Metal)
                rawBlackMetal,
                melodicBlackMetal,
                atmosphericBlackMetal,
                symphonicBlackMetal,
                depressiveBlackMetal,
                
                // Level 2 (Death Metal)
                melodicDeathMetal,
                technicalDeathMetal,
                brutalDeathMetal,
                
                // Level 2 (Doom Metal)
                funeralDoom,
                stonerDoom
            };
        }
    }
}