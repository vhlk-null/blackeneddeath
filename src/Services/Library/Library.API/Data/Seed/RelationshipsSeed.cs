using Library.API.Models.JoinTables;
using Library.Domain.Models.JoinTables;

namespace Library.API.Data.Seed;

public static class RelationshipsSeed
{
    // Album-Band relationships
    public static List<AlbumBand> GetAlbumBands()
    {
        return new List<AlbumBand>
        {
            new AlbumBand { AlbumId = SeedConstants.Albums.TransilvanianHunger, BandId = SeedConstants.Bands.Darkthrone },
            new AlbumBand { AlbumId = SeedConstants.Albums.Filosofem, BandId = SeedConstants.Bands.Burzum },
            new AlbumBand { AlbumId = SeedConstants.Albums.NightsideEclipse, BandId = SeedConstants.Bands.Emperor },
            new AlbumBand { AlbumId = SeedConstants.Albums.DeMysteriis, BandId = SeedConstants.Bands.Mayhem },
            new AlbumBand { AlbumId = SeedConstants.Albums.StormOfLightsBane, BandId = SeedConstants.Bands.Dissection },
            new AlbumBand { AlbumId = SeedConstants.Albums.TheSatanist, BandId = SeedConstants.Bands.Behemoth }
        };
    }

    // Album-Genre relationships
    public static List<AlbumGenre> GetAlbumGenres()
    {
        return new List<AlbumGenre>
        {
            // Transilvanian Hunger - Black Metal, Raw Black Metal
            new AlbumGenre { AlbumId = SeedConstants.Albums.TransilvanianHunger, GenreId = SeedConstants.Genres.BlackMetal },
            new AlbumGenre { AlbumId = SeedConstants.Albums.TransilvanianHunger, GenreId = SeedConstants.Genres.RawBlackMetal },

            // Filosofem - Black Metal, Atmospheric
            new AlbumGenre { AlbumId = SeedConstants.Albums.Filosofem, GenreId = SeedConstants.Genres.BlackMetal },
            new AlbumGenre { AlbumId = SeedConstants.Albums.Filosofem, GenreId = SeedConstants.Genres.AtmosphericBlackMetal },

            // In the Nightside Eclipse - Black Metal, Symphonic
            new AlbumGenre { AlbumId = SeedConstants.Albums.NightsideEclipse, GenreId = SeedConstants.Genres.BlackMetal },
            new AlbumGenre { AlbumId = SeedConstants.Albums.NightsideEclipse, GenreId = SeedConstants.Genres.SymphonicBlackMetal },

            // De Mysteriis Dom Sathanas - Black Metal
            new AlbumGenre { AlbumId = SeedConstants.Albums.DeMysteriis, GenreId = SeedConstants.Genres.BlackMetal },

            // Storm of the Light's Bane - Black Metal
            new AlbumGenre { AlbumId = SeedConstants.Albums.StormOfLightsBane, GenreId = SeedConstants.Genres.BlackMetal },

            // The Satanist - Black Metal
            new AlbumGenre { AlbumId = SeedConstants.Albums.TheSatanist, GenreId = SeedConstants.Genres.BlackMetal }
        };
    }

    // Album-Country relationships
    public static List<AlbumCountry> GetAlbumCountries()
    {
        return new List<AlbumCountry>
        {
            // Norwegian albums
            new AlbumCountry { AlbumId = SeedConstants.Albums.TransilvanianHunger, CountryId = SeedConstants.Countries.Norway },
            new AlbumCountry { AlbumId = SeedConstants.Albums.Filosofem, CountryId = SeedConstants.Countries.Norway },
            new AlbumCountry { AlbumId = SeedConstants.Albums.NightsideEclipse, CountryId = SeedConstants.Countries.Norway },
            new AlbumCountry { AlbumId = SeedConstants.Albums.DeMysteriis, CountryId = SeedConstants.Countries.Norway },

            // Swedish albums
            new AlbumCountry { AlbumId = SeedConstants.Albums.StormOfLightsBane, CountryId = SeedConstants.Countries.Sweden },

            // Polish albums
            new AlbumCountry { AlbumId = SeedConstants.Albums.TheSatanist, CountryId = SeedConstants.Countries.Poland }
        };
    }

    // Band-Genre relationships
    public static List<BandGenre> GetBandGenres()
    {
        return new List<BandGenre>
        {
            // Darkthrone - Black Metal, Raw Black Metal
            new BandGenre { BandId = SeedConstants.Bands.Darkthrone, GenreId = SeedConstants.Genres.BlackMetal },
            new BandGenre { BandId = SeedConstants.Bands.Darkthrone, GenreId = SeedConstants.Genres.RawBlackMetal },

            // Burzum - Black Metal, Atmospheric Black Metal
            new BandGenre { BandId = SeedConstants.Bands.Burzum, GenreId = SeedConstants.Genres.BlackMetal },
            new BandGenre { BandId = SeedConstants.Bands.Burzum, GenreId = SeedConstants.Genres.AtmosphericBlackMetal },

            // Emperor - Black Metal, Symphonic Black Metal
            new BandGenre { BandId = SeedConstants.Bands.Emperor, GenreId = SeedConstants.Genres.BlackMetal },
            new BandGenre { BandId = SeedConstants.Bands.Emperor, GenreId = SeedConstants.Genres.SymphonicBlackMetal },

            // Mayhem - Black Metal
            new BandGenre { BandId = SeedConstants.Bands.Mayhem, GenreId = SeedConstants.Genres.BlackMetal },

            // Dissection - Black Metal, Melodic Black Metal
            new BandGenre { BandId = SeedConstants.Bands.Dissection, GenreId = SeedConstants.Genres.BlackMetal },
            new BandGenre { BandId = SeedConstants.Bands.Dissection, GenreId = SeedConstants.Genres.MelodicBlackMetal },

            // Behemoth - Black Metal, Death Metal
            new BandGenre { BandId = SeedConstants.Bands.Behemoth, GenreId = SeedConstants.Genres.BlackMetal },
            new BandGenre { BandId = SeedConstants.Bands.Behemoth, GenreId = SeedConstants.Genres.DeathMetal }
        };
    }

    // Album-Track relationships
    public static List<AlbumTrack> GetAlbumTracks()
    {
        return new List<AlbumTrack>
        {
            // Transilvanian Hunger
            new AlbumTrack { AlbumId = SeedConstants.Albums.TransilvanianHunger, TrackId = SeedConstants.Tracks.TransilvanianHunger1 },
            new AlbumTrack { AlbumId = SeedConstants.Albums.TransilvanianHunger, TrackId = SeedConstants.Tracks.TransilvanianHunger2 },
            new AlbumTrack { AlbumId = SeedConstants.Albums.TransilvanianHunger, TrackId = SeedConstants.Tracks.TransilvanianHunger3 },
            new AlbumTrack { AlbumId = SeedConstants.Albums.TransilvanianHunger, TrackId = SeedConstants.Tracks.TransilvanianHunger4 },

            // Filosofem
            new AlbumTrack { AlbumId = SeedConstants.Albums.Filosofem, TrackId = SeedConstants.Tracks.Filosofem1 },
            new AlbumTrack { AlbumId = SeedConstants.Albums.Filosofem, TrackId = SeedConstants.Tracks.Filosofem2 },
            new AlbumTrack { AlbumId = SeedConstants.Albums.Filosofem, TrackId = SeedConstants.Tracks.Filosofem3 },

            // In the Nightside Eclipse
            new AlbumTrack { AlbumId = SeedConstants.Albums.NightsideEclipse, TrackId = SeedConstants.Tracks.NightsideEclipse1 },
            new AlbumTrack { AlbumId = SeedConstants.Albums.NightsideEclipse, TrackId = SeedConstants.Tracks.NightsideEclipse2 },
            new AlbumTrack { AlbumId = SeedConstants.Albums.NightsideEclipse, TrackId = SeedConstants.Tracks.NightsideEclipse3 }
        };
    }
}