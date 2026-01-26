namespace Archive.API.Data.Seeds
{
    public static class AlbumRelationshipsSeed
    {
        // Album-Band relationships
        public static List<AlbumBand> GetAlbumBands()
        {
            var darkthrone = Guid.Parse("b0000000-0000-0000-0000-000000000001");
            var burzum = Guid.Parse("b0000000-0000-0000-0000-000000000002");
            var emperor = Guid.Parse("b0000000-0000-0000-0000-000000000003");
            var mayhem = Guid.Parse("b0000000-0000-0000-0000-000000000004");
            var dissection = Guid.Parse("b0000000-0000-0000-0000-000000000005");
            var behemoth = Guid.Parse("b0000000-0000-0000-0000-000000000006");

            var transilvanianHunger = Guid.Parse("a0000000-0000-0000-0000-000000000001");
            var filosofem = Guid.Parse("a0000000-0000-0000-0000-000000000002");
            var nightsideEclipse = Guid.Parse("a0000000-0000-0000-0000-000000000003");
            var deMysteriis = Guid.Parse("a0000000-0000-0000-0000-000000000004");
            var stormOfLightsBane = Guid.Parse("a0000000-0000-0000-0000-000000000005");
            var satanist = Guid.Parse("a0000000-0000-0000-0000-000000000006");

            return new List<AlbumBand>
            {
                new AlbumBand { AlbumId = transilvanianHunger, BandId = darkthrone },
                new AlbumBand { AlbumId = filosofem, BandId = burzum },
                new AlbumBand { AlbumId = nightsideEclipse, BandId = emperor },
                new AlbumBand { AlbumId = deMysteriis, BandId = mayhem },
                new AlbumBand { AlbumId = stormOfLightsBane, BandId = dissection },
                new AlbumBand { AlbumId = satanist, BandId = behemoth }
            };
        }

        // Album-Genre relationships
        public static List<AlbumGenre> GetAlbumGenres()
        {
            var blackMetal = Guid.Parse("20000000-0000-0000-0000-000000000001");
            var rawBlackMetal = Guid.Parse("30000000-0000-0000-0000-000000000001");
            var atmosphericBlackMetal = Guid.Parse("30000000-0000-0000-0000-000000000003");
            var symphonicBlackMetal = Guid.Parse("30000000-0000-0000-0000-000000000004");

            var transilvanianHunger = Guid.Parse("a0000000-0000-0000-0000-000000000001");
            var filosofem = Guid.Parse("a0000000-0000-0000-0000-000000000002");
            var nightsideEclipse = Guid.Parse("a0000000-0000-0000-0000-000000000003");
            var deMysteriis = Guid.Parse("a0000000-0000-0000-0000-000000000004");
            var stormOfLightsBane = Guid.Parse("a0000000-0000-0000-0000-000000000005");
            var satanist = Guid.Parse("a0000000-0000-0000-0000-000000000006");

            return new List<AlbumGenre>
            {
                // Transilvanian Hunger - Black Metal, Raw Black Metal
                new AlbumGenre { AlbumId = transilvanianHunger, GenreId = blackMetal },
                new AlbumGenre { AlbumId = transilvanianHunger, GenreId = rawBlackMetal },
                
                // Filosofem - Black Metal, Atmospheric
                new AlbumGenre { AlbumId = filosofem, GenreId = blackMetal },
                new AlbumGenre { AlbumId = filosofem, GenreId = atmosphericBlackMetal },
                
                // In the Nightside Eclipse - Black Metal, Symphonic
                new AlbumGenre { AlbumId = nightsideEclipse, GenreId = blackMetal },
                new AlbumGenre { AlbumId = nightsideEclipse, GenreId = symphonicBlackMetal },
                
                // De Mysteriis Dom Sathanas - Black Metal
                new AlbumGenre { AlbumId = deMysteriis, GenreId = blackMetal },
                
                // Storm of the Light's Bane - Black Metal
                new AlbumGenre { AlbumId = stormOfLightsBane, GenreId = blackMetal },
                
                // The Satanist - Black Metal
                new AlbumGenre { AlbumId = satanist, GenreId = blackMetal }
            };
        }

        // Album-Country relationships
        public static List<AlbumCountry> GetAlbumCountries()
        {
            var norwayId = Guid.Parse("c0000000-0000-0000-0000-000000000001");
            var swedenId = Guid.Parse("c0000000-0000-0000-0000-000000000002");
            var polandId = Guid.Parse("c0000000-0000-0000-0000-000000000004");

            var transilvanianHunger = Guid.Parse("a0000000-0000-0000-0000-000000000001");
            var filosofem = Guid.Parse("a0000000-0000-0000-0000-000000000002");
            var nightsideEclipse = Guid.Parse("a0000000-0000-0000-0000-000000000003");
            var deMysteriis = Guid.Parse("a0000000-0000-0000-0000-000000000004");
            var stormOfLightsBane = Guid.Parse("a0000000-0000-0000-0000-000000000005");
            var satanist = Guid.Parse("a0000000-0000-0000-0000-000000000006");

            return new List<AlbumCountry>
            {
                // Norwegian albums
                new AlbumCountry { AlbumId = transilvanianHunger, CountryId = norwayId },
                new AlbumCountry { AlbumId = filosofem, CountryId = norwayId },
                new AlbumCountry { AlbumId = nightsideEclipse, CountryId = norwayId },
                new AlbumCountry { AlbumId = deMysteriis, CountryId = norwayId },
                
                // Swedish albums
                new AlbumCountry { AlbumId = stormOfLightsBane, CountryId = swedenId },
                
                // Polish albums
                new AlbumCountry { AlbumId = satanist, CountryId = polandId }
            };
        }

        // Album-Track relationships
        public static List<AlbumTrack> GetAlbumTracks()
        {
            var transilvanianHunger = Guid.Parse("a0000000-0000-0000-0000-000000000001");
            var filosofem = Guid.Parse("a0000000-0000-0000-0000-000000000002");
            var nightsideEclipse = Guid.Parse("a0000000-0000-0000-0000-000000000003");

            return new List<AlbumTrack>
            {
                // Transilvanian Hunger
                new AlbumTrack { AlbumId = transilvanianHunger, TrackId = Guid.Parse("00000000-0000-0000-0002-000000000001") },
                new AlbumTrack { AlbumId = transilvanianHunger, TrackId = Guid.Parse("00000000-0000-0000-0002-000000000002") },
                new AlbumTrack { AlbumId = transilvanianHunger, TrackId = Guid.Parse("00000000-0000-0000-0002-000000000003") },
                new AlbumTrack { AlbumId = transilvanianHunger, TrackId = Guid.Parse("00000000-0000-0000-0002-000000000004") },
                
                // Filosofem
                new AlbumTrack { AlbumId = filosofem, TrackId = Guid.Parse("00000000-0000-0000-0002-000000000005") },
                new AlbumTrack { AlbumId = filosofem, TrackId = Guid.Parse("00000000-0000-0000-0002-000000000006") },
                new AlbumTrack { AlbumId = filosofem, TrackId = Guid.Parse("00000000-0000-0000-0002-000000000007") },
                
                // In the Nightside Eclipse
                new AlbumTrack { AlbumId = nightsideEclipse, TrackId = Guid.Parse("00000000-0000-0000-0002-000000000008") },
                new AlbumTrack { AlbumId = nightsideEclipse, TrackId = Guid.Parse("00000000-0000-0000-0002-000000000009") },
                new AlbumTrack { AlbumId = nightsideEclipse, TrackId = Guid.Parse("0000000a-0000-0000-0002-000000000000") }
            };
        }
    }
}