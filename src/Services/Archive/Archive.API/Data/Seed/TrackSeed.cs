namespace Archive.API.Data.Seeds
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
                    Id = Guid.Parse("00000000-0000-0000-0002-000000000001"),
                    Title = "Transilvanian Hunger"
                },
                new Track
                {
                    Id = Guid.Parse("00000000-0000-0000-0002-000000000002"),
                    Title = "Over Fjell og Gjennom Torner"
                },
                new Track
                {
                    Id = Guid.Parse("00000000-0000-0000-0002-000000000003"),
                    Title = "Skald av Satans Sol"
                },
                new Track
                {
                    Id = Guid.Parse("00000000-0000-0000-0002-000000000004"),
                    Title = "Slottet i Det Fjerne"
                },
                
                // Filosofem tracks
                new Track
                {
                    Id = Guid.Parse("00000000-0000-0000-0002-000000000005"),
                    Title = "Dunkelheit"
                },
                new Track
                {
                    Id = Guid.Parse("00000000-0000-0000-0002-000000000006"),
                    Title = "Jesus' Tod"
                },
                new Track
                {
                    Id = Guid.Parse("00000000-0000-0000-0002-000000000007"),
                    Title = "Erblicket die Töchter des Firmaments"
                },
                
                // In the Nightside Eclipse tracks
                new Track
                {
                    Id = Guid.Parse("00000000-0000-0000-0002-000000000008"),
                    Title = "Into the Infinity of Thoughts"
                },
                new Track
                {
                    Id = Guid.Parse("00000000-0000-0000-0002-000000000009"),
                    Title = "The Burning Shadows of Silence"
                },
                new Track
                {
                    Id = Guid.Parse("0000000a-0000-0000-0002-000000000000"),
                    Title = "Cosmic Keys to My Creations & Times"
                }
            };
        }
    }
}