using Library.Domain.Models;

namespace Library.API.Data.Seed
{
    public static class TrackSeed
    {
        public static List<Track> GetTracks()
        {
            return new List<Track>
            {
                // Transilvanian Hunger tracks
                new Track
                {
                    Id = SeedConstants.Tracks.TransilvanianHunger1,
                    Title = "Transilvanian Hunger"
                },
                new Track
                {
                    Id = SeedConstants.Tracks.TransilvanianHunger2,
                    Title = "Over Fjell og Gjennom Torner"
                },
                new Track
                {
                    Id = SeedConstants.Tracks.TransilvanianHunger3,
                    Title = "Skald av Satans Sol"
                },
                new Track
                {
                    Id = SeedConstants.Tracks.TransilvanianHunger4,
                    Title = "Slottet i Det Fjerne"
                },

                // Filosofem tracks
                new Track
                {
                    Id = SeedConstants.Tracks.Filosofem1,
                    Title = "Dunkelheit"
                },
                new Track
                {
                    Id = SeedConstants.Tracks.Filosofem2,
                    Title = "Jesus' Tod"
                },
                new Track
                {
                    Id = SeedConstants.Tracks.Filosofem3,
                    Title = "Erblicket die Töchter des Firmaments"
                },

                // In the Nightside Eclipse tracks
                new Track
                {
                    Id = SeedConstants.Tracks.NightsideEclipse1,
                    Title = "Into the Infinity of Thoughts"
                },
                new Track
                {
                    Id = SeedConstants.Tracks.NightsideEclipse2,
                    Title = "The Burning Shadows of Silence"
                },
                new Track
                {
                    Id = SeedConstants.Tracks.NightsideEclipse3,
                    Title = "Cosmic Keys to My Creations & Times"
                }
            };
        }
    }
}
