using Library.API.Models;

namespace Library.API.Data.Seed
{
    public static class GenreSeed
    {
        public static List<Genre> GetGenres()
        {
            // Main genre
            var metal = new Genre
            {
                Id = SeedConstants.Genres.Metal,
                Name = "Metal",
                ParentGenreId = null
            };

            // Sub-genres level 1
            var blackMetal = new Genre
            {
                Id = SeedConstants.Genres.BlackMetal,
                Name = "Black Metal",
                ParentGenreId = metal.Id
            };

            var deathMetal = new Genre
            {
                Id = SeedConstants.Genres.DeathMetal,
                Name = "Death Metal",
                ParentGenreId = metal.Id
            };

            var doomMetal = new Genre
            {
                Id = SeedConstants.Genres.DoomMetal,
                Name = "Doom Metal",
                ParentGenreId = metal.Id
            };

            var thrashMetal = new Genre
            {
                Id = SeedConstants.Genres.ThrashMetal,
                Name = "Thrash Metal",
                ParentGenreId = metal.Id
            };

            // Sub-genres level 2 (Black Metal)
            var rawBlackMetal = new Genre
            {
                Id = SeedConstants.Genres.RawBlackMetal,
                Name = "Raw Black Metal",
                ParentGenreId = blackMetal.Id
            };

            var melodicBlackMetal = new Genre
            {
                Id = SeedConstants.Genres.MelodicBlackMetal,
                Name = "Melodic Black Metal",
                ParentGenreId = blackMetal.Id
            };

            var atmosphericBlackMetal = new Genre
            {
                Id = SeedConstants.Genres.AtmosphericBlackMetal,
                Name = "Atmospheric Black Metal",
                ParentGenreId = blackMetal.Id
            };

            var symphonicBlackMetal = new Genre
            {
                Id = SeedConstants.Genres.SymphonicBlackMetal,
                Name = "Symphonic Black Metal",
                ParentGenreId = blackMetal.Id
            };

            var depressiveBlackMetal = new Genre
            {
                Id = SeedConstants.Genres.DepressiveBlackMetal,
                Name = "Depressive Black Metal",
                ParentGenreId = blackMetal.Id
            };

            // Sub-genres level 2 (Death Metal)
            var melodicDeathMetal = new Genre
            {
                Id = SeedConstants.Genres.MelodicDeathMetal,
                Name = "Melodic Death Metal",
                ParentGenreId = deathMetal.Id
            };

            var technicalDeathMetal = new Genre
            {
                Id = SeedConstants.Genres.TechnicalDeathMetal,
                Name = "Technical Death Metal",
                ParentGenreId = deathMetal.Id
            };

            var brutalDeathMetal = new Genre
            {
                Id = SeedConstants.Genres.BrutalDeathMetal,
                Name = "Brutal Death Metal",
                ParentGenreId = deathMetal.Id
            };

            // Sub-genres level 2 (Doom Metal)
            var funeralDoom = new Genre
            {
                Id = SeedConstants.Genres.FuneralDoom,
                Name = "Funeral Doom",
                ParentGenreId = doomMetal.Id
            };

            var stonerDoom = new Genre
            {
                Id = SeedConstants.Genres.StonerDoom,
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
