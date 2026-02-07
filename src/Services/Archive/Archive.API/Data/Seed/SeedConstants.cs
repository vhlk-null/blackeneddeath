namespace Archive.API.Data.Seed
{
    public static class SeedConstants
    {
        // Countries
        public static class Countries
        {
            public static readonly Guid Norway = Guid.Parse("c0000000-0000-0000-0000-000000000001");
            public static readonly Guid Sweden = Guid.Parse("c0000000-0000-0000-0000-000000000002");
            public static readonly Guid Finland = Guid.Parse("c0000000-0000-0000-0000-000000000003");
            public static readonly Guid Poland = Guid.Parse("c0000000-0000-0000-0000-000000000004");
            public static readonly Guid Ukraine = Guid.Parse("c0000000-0000-0000-0000-000000000005");
            public static readonly Guid UnitedStates = Guid.Parse("c0000000-0000-0000-0000-000000000006");
            public static readonly Guid UnitedKingdom = Guid.Parse("c0000000-0000-0000-0000-000000000007");
            public static readonly Guid Germany = Guid.Parse("c0000000-0000-0000-0000-000000000008");
            public static readonly Guid France = Guid.Parse("c0000000-0000-0000-0000-000000000009");
        }

        // Bands
        public static class Bands
        {
            public static readonly Guid Darkthrone = Guid.Parse("b0000000-0000-0000-0000-000000000001");
            public static readonly Guid Burzum = Guid.Parse("b0000000-0000-0000-0000-000000000002");
            public static readonly Guid Emperor = Guid.Parse("b0000000-0000-0000-0000-000000000003");
            public static readonly Guid Mayhem = Guid.Parse("b0000000-0000-0000-0000-000000000004");
            public static readonly Guid Dissection = Guid.Parse("b0000000-0000-0000-0000-000000000005");
            public static readonly Guid Behemoth = Guid.Parse("b0000000-0000-0000-0000-000000000006");
        }

        // Albums
        public static class Albums
        {
            public static readonly Guid TransilvanianHunger = Guid.Parse("a0000000-0000-0000-0000-000000000001");
            public static readonly Guid Filosofem = Guid.Parse("a0000000-0000-0000-0000-000000000002");
            public static readonly Guid NightsideEclipse = Guid.Parse("a0000000-0000-0000-0000-000000000003");
            public static readonly Guid DeMysteriis = Guid.Parse("a0000000-0000-0000-0000-000000000004");
            public static readonly Guid StormOfLightsBane = Guid.Parse("a0000000-0000-0000-0000-000000000005");
            public static readonly Guid TheSatanist = Guid.Parse("a0000000-0000-0000-0000-000000000006");
        }

        // Genres - Main
        public static class Genres
        {
            public static readonly Guid Metal = Guid.Parse("10000000-0000-0000-0000-000000000001");

            // Level 1
            public static readonly Guid BlackMetal = Guid.Parse("20000000-0000-0000-0000-000000000001");
            public static readonly Guid DeathMetal = Guid.Parse("20000000-0000-0000-0000-000000000002");
            public static readonly Guid DoomMetal = Guid.Parse("20000000-0000-0000-0000-000000000003");
            public static readonly Guid ThrashMetal = Guid.Parse("20000000-0000-0000-0000-000000000004");

            // Level 2 - Black Metal
            public static readonly Guid RawBlackMetal = Guid.Parse("30000000-0000-0000-0000-000000000001");
            public static readonly Guid MelodicBlackMetal = Guid.Parse("30000000-0000-0000-0000-000000000002");
            public static readonly Guid AtmosphericBlackMetal = Guid.Parse("30000000-0000-0000-0000-000000000003");
            public static readonly Guid SymphonicBlackMetal = Guid.Parse("30000000-0000-0000-0000-000000000004");
            public static readonly Guid DepressiveBlackMetal = Guid.Parse("30000000-0000-0000-0000-000000000005");

            // Level 2 - Death Metal
            public static readonly Guid MelodicDeathMetal = Guid.Parse("30000000-0000-0000-0000-000000000006");
            public static readonly Guid TechnicalDeathMetal = Guid.Parse("30000000-0000-0000-0000-000000000007");
            public static readonly Guid BrutalDeathMetal = Guid.Parse("30000000-0000-0000-0000-000000000008");

            // Level 2 - Doom Metal
            public static readonly Guid FuneralDoom = Guid.Parse("30000000-0000-0000-0000-000000000009");
            public static readonly Guid StonerDoom = Guid.Parse("3000000a-0000-0000-0000-000000000000");
        }

        // Tracks
        public static class Tracks
        {
            // Transilvanian Hunger tracks
            public static readonly Guid TransilvanianHunger1 = Guid.Parse("00000000-0000-0000-0002-000000000001");
            public static readonly Guid TransilvanianHunger2 = Guid.Parse("00000000-0000-0000-0002-000000000002");
            public static readonly Guid TransilvanianHunger3 = Guid.Parse("00000000-0000-0000-0002-000000000003");
            public static readonly Guid TransilvanianHunger4 = Guid.Parse("00000000-0000-0000-0002-000000000004");

            // Filosofem tracks
            public static readonly Guid Filosofem1 = Guid.Parse("00000000-0000-0000-0002-000000000005");
            public static readonly Guid Filosofem2 = Guid.Parse("00000000-0000-0000-0002-000000000006");
            public static readonly Guid Filosofem3 = Guid.Parse("00000000-0000-0000-0002-000000000007");

            // In the Nightside Eclipse tracks
            public static readonly Guid NightsideEclipse1 = Guid.Parse("00000000-0000-0000-0002-000000000008");
            public static readonly Guid NightsideEclipse2 = Guid.Parse("00000000-0000-0000-0002-000000000009");
            public static readonly Guid NightsideEclipse3 = Guid.Parse("0000000a-0000-0000-0002-000000000000");
        }
    }
}
