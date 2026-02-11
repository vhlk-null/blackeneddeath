namespace UserContent.API.Data.Seed
{
    public static class SeedConstants
    {
        // Users
        public static class Users
        {
            public static readonly Guid MetalHead666 = Guid.Parse("d0000000-0000-0000-0000-000000000001");
            public static readonly Guid DarkSoulFan = Guid.Parse("d0000000-0000-0000-0000-000000000002");
            public static readonly Guid BlastBeatLover = Guid.Parse("d0000000-0000-0000-0000-000000000003");
        }

        // Reference to Archive.API album IDs (must match Archive seed data)
        public static class ArchiveAlbums
        {
            public static readonly Guid TransilvanianHunger = Guid.Parse("a0000000-0000-0000-0000-000000000001");
            public static readonly Guid Filosofem = Guid.Parse("a0000000-0000-0000-0000-000000000002");
            public static readonly Guid NightsideEclipse = Guid.Parse("a0000000-0000-0000-0000-000000000003");
            public static readonly Guid DeMysteriis = Guid.Parse("a0000000-0000-0000-0000-000000000004");
            public static readonly Guid StormOfLightsBane = Guid.Parse("a0000000-0000-0000-0000-000000000005");
            public static readonly Guid TheSatanist = Guid.Parse("a0000000-0000-0000-0000-000000000006");
        }

        // Reference to Archive.API band IDs (must match Archive seed data)
        public static class ArchiveBands
        {
            public static readonly Guid Darkthrone = Guid.Parse("b0000000-0000-0000-0000-000000000001");
            public static readonly Guid Burzum = Guid.Parse("b0000000-0000-0000-0000-000000000002");
            public static readonly Guid Emperor = Guid.Parse("b0000000-0000-0000-0000-000000000003");
            public static readonly Guid Mayhem = Guid.Parse("b0000000-0000-0000-0000-000000000004");
            public static readonly Guid Dissection = Guid.Parse("b0000000-0000-0000-0000-000000000005");
            public static readonly Guid Behemoth = Guid.Parse("b0000000-0000-0000-0000-000000000006");
        }
    }
}
