using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Domain.ValueObjects;
using Library.Domain.ValueObjects.Ids;

namespace Library.Infrastructure.Data.Extensions;

internal static class InitialData
{
    // ── Country IDs ────────────────────────────────────────────────────────────
    private static class CountryIds
    {
        public static readonly CountryId Norway        = CountryId.Of(Guid.Parse("c0000000-0000-0000-0000-000000000001"));
        public static readonly CountryId Sweden        = CountryId.Of(Guid.Parse("c0000000-0000-0000-0000-000000000002"));
        public static readonly CountryId Finland       = CountryId.Of(Guid.Parse("c0000000-0000-0000-0000-000000000003"));
        public static readonly CountryId Poland        = CountryId.Of(Guid.Parse("c0000000-0000-0000-0000-000000000004"));
        public static readonly CountryId Ukraine       = CountryId.Of(Guid.Parse("c0000000-0000-0000-0000-000000000005"));
        public static readonly CountryId UnitedStates  = CountryId.Of(Guid.Parse("c0000000-0000-0000-0000-000000000006"));
        public static readonly CountryId UnitedKingdom = CountryId.Of(Guid.Parse("c0000000-0000-0000-0000-000000000007"));
        public static readonly CountryId Germany       = CountryId.Of(Guid.Parse("c0000000-0000-0000-0000-000000000008"));
        public static readonly CountryId France        = CountryId.Of(Guid.Parse("c0000000-0000-0000-0000-000000000009"));
    }

    // ── Band IDs ───────────────────────────────────────────────────────────────
    private static class BandIds
    {
        public static readonly BandId Darkthrone = BandId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000001"));
        public static readonly BandId Burzum     = BandId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000002"));
        public static readonly BandId Emperor    = BandId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000003"));
        public static readonly BandId Mayhem     = BandId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000004"));
        public static readonly BandId Dissection = BandId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000005"));
        public static readonly BandId Behemoth   = BandId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000006"));
    }

    // ── Album IDs ──────────────────────────────────────────────────────────────
    private static class AlbumIds
    {
        public static readonly AlbumId TransilvanianHunger = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000001"));
        public static readonly AlbumId Filosofem           = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000002"));
        public static readonly AlbumId NightsideEclipse    = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000003"));
        public static readonly AlbumId DeMysteriis         = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000004"));
        public static readonly AlbumId StormOfLightsBane   = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000005"));
        public static readonly AlbumId TheSatanist         = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000006"));
    }

    // ── Genre IDs ──────────────────────────────────────────────────────────────
    private static class GenreIds
    {
        public static readonly GenreId Metal = GenreId.Of(Guid.Parse("10000000-0000-0000-0000-000000000001"));

        // Level 1
        public static readonly GenreId BlackMetal  = GenreId.Of(Guid.Parse("20000000-0000-0000-0000-000000000001"));
        public static readonly GenreId DeathMetal  = GenreId.Of(Guid.Parse("20000000-0000-0000-0000-000000000002"));
        public static readonly GenreId DoomMetal   = GenreId.Of(Guid.Parse("20000000-0000-0000-0000-000000000003"));
        public static readonly GenreId ThrashMetal = GenreId.Of(Guid.Parse("20000000-0000-0000-0000-000000000004"));

        // Level 2 — Black Metal
        public static readonly GenreId RawBlackMetal         = GenreId.Of(Guid.Parse("30000000-0000-0000-0000-000000000001"));
        public static readonly GenreId MelodicBlackMetal     = GenreId.Of(Guid.Parse("30000000-0000-0000-0000-000000000002"));
        public static readonly GenreId AtmosphericBlackMetal = GenreId.Of(Guid.Parse("30000000-0000-0000-0000-000000000003"));
        public static readonly GenreId SymphonicBlackMetal   = GenreId.Of(Guid.Parse("30000000-0000-0000-0000-000000000004"));
        public static readonly GenreId DepressiveBlackMetal  = GenreId.Of(Guid.Parse("30000000-0000-0000-0000-000000000005"));

        // Level 2 — Death Metal
        public static readonly GenreId MelodicDeathMetal   = GenreId.Of(Guid.Parse("30000000-0000-0000-0000-000000000006"));
        public static readonly GenreId TechnicalDeathMetal = GenreId.Of(Guid.Parse("30000000-0000-0000-0000-000000000007"));
        public static readonly GenreId BrutalDeathMetal    = GenreId.Of(Guid.Parse("30000000-0000-0000-0000-000000000008"));

        // Level 2 — Doom Metal
        public static readonly GenreId FuneralDoom = GenreId.Of(Guid.Parse("30000000-0000-0000-0000-000000000009"));
        public static readonly GenreId StonerDoom  = GenreId.Of(Guid.Parse("3000000a-0000-0000-0000-000000000000"));
    }

    // ── Track IDs — pattern: {album_num:8}-0000-0000-0001-{track_num:12} ───────
    private static class TrackIds
    {
        // Transilvanian Hunger (album 1)
        public static readonly TrackId TH1 = TrackId.Of(Guid.Parse("00000001-0000-0000-0001-000000000001"));
        public static readonly TrackId TH2 = TrackId.Of(Guid.Parse("00000001-0000-0000-0001-000000000002"));
        public static readonly TrackId TH3 = TrackId.Of(Guid.Parse("00000001-0000-0000-0001-000000000003"));
        public static readonly TrackId TH4 = TrackId.Of(Guid.Parse("00000001-0000-0000-0001-000000000004"));
        public static readonly TrackId TH5 = TrackId.Of(Guid.Parse("00000001-0000-0000-0001-000000000005"));
        public static readonly TrackId TH6 = TrackId.Of(Guid.Parse("00000001-0000-0000-0001-000000000006"));
        public static readonly TrackId TH7 = TrackId.Of(Guid.Parse("00000001-0000-0000-0001-000000000007"));
        public static readonly TrackId TH8 = TrackId.Of(Guid.Parse("00000001-0000-0000-0001-000000000008"));

        // Filosofem (album 2)
        public static readonly TrackId F1 = TrackId.Of(Guid.Parse("00000002-0000-0000-0001-000000000001"));
        public static readonly TrackId F2 = TrackId.Of(Guid.Parse("00000002-0000-0000-0001-000000000002"));
        public static readonly TrackId F3 = TrackId.Of(Guid.Parse("00000002-0000-0000-0001-000000000003"));
        public static readonly TrackId F4 = TrackId.Of(Guid.Parse("00000002-0000-0000-0001-000000000004"));

        // In the Nightside Eclipse (album 3)
        public static readonly TrackId NE1 = TrackId.Of(Guid.Parse("00000003-0000-0000-0001-000000000001"));
        public static readonly TrackId NE2 = TrackId.Of(Guid.Parse("00000003-0000-0000-0001-000000000002"));
        public static readonly TrackId NE3 = TrackId.Of(Guid.Parse("00000003-0000-0000-0001-000000000003"));
        public static readonly TrackId NE4 = TrackId.Of(Guid.Parse("00000003-0000-0000-0001-000000000004"));
        public static readonly TrackId NE5 = TrackId.Of(Guid.Parse("00000003-0000-0000-0001-000000000005"));
        public static readonly TrackId NE6 = TrackId.Of(Guid.Parse("00000003-0000-0000-0001-000000000006"));
        public static readonly TrackId NE7 = TrackId.Of(Guid.Parse("00000003-0000-0000-0001-000000000007"));
        public static readonly TrackId NE8 = TrackId.Of(Guid.Parse("00000003-0000-0000-0001-000000000008"));

        // De Mysteriis Dom Sathanas (album 4)
        public static readonly TrackId DM1 = TrackId.Of(Guid.Parse("00000004-0000-0000-0001-000000000001"));
        public static readonly TrackId DM2 = TrackId.Of(Guid.Parse("00000004-0000-0000-0001-000000000002"));
        public static readonly TrackId DM3 = TrackId.Of(Guid.Parse("00000004-0000-0000-0001-000000000003"));
        public static readonly TrackId DM4 = TrackId.Of(Guid.Parse("00000004-0000-0000-0001-000000000004"));
        public static readonly TrackId DM5 = TrackId.Of(Guid.Parse("00000004-0000-0000-0001-000000000005"));
        public static readonly TrackId DM6 = TrackId.Of(Guid.Parse("00000004-0000-0000-0001-000000000006"));
        public static readonly TrackId DM7 = TrackId.Of(Guid.Parse("00000004-0000-0000-0001-000000000007"));
        public static readonly TrackId DM8 = TrackId.Of(Guid.Parse("00000004-0000-0000-0001-000000000008"));

        // Storm of the Light's Bane (album 5)
        public static readonly TrackId SL1 = TrackId.Of(Guid.Parse("00000005-0000-0000-0001-000000000001"));
        public static readonly TrackId SL2 = TrackId.Of(Guid.Parse("00000005-0000-0000-0001-000000000002"));
        public static readonly TrackId SL3 = TrackId.Of(Guid.Parse("00000005-0000-0000-0001-000000000003"));
        public static readonly TrackId SL4 = TrackId.Of(Guid.Parse("00000005-0000-0000-0001-000000000004"));
        public static readonly TrackId SL5 = TrackId.Of(Guid.Parse("00000005-0000-0000-0001-000000000005"));
        public static readonly TrackId SL6 = TrackId.Of(Guid.Parse("00000005-0000-0000-0001-000000000006"));
        public static readonly TrackId SL7 = TrackId.Of(Guid.Parse("00000005-0000-0000-0001-000000000007"));
        public static readonly TrackId SL8 = TrackId.Of(Guid.Parse("00000005-0000-0000-0001-000000000008"));

        // The Satanist (album 6)
        public static readonly TrackId TS1 = TrackId.Of(Guid.Parse("00000006-0000-0000-0001-000000000001"));
        public static readonly TrackId TS2 = TrackId.Of(Guid.Parse("00000006-0000-0000-0001-000000000002"));
        public static readonly TrackId TS3 = TrackId.Of(Guid.Parse("00000006-0000-0000-0001-000000000003"));
        public static readonly TrackId TS4 = TrackId.Of(Guid.Parse("00000006-0000-0000-0001-000000000004"));
        public static readonly TrackId TS5 = TrackId.Of(Guid.Parse("00000006-0000-0000-0001-000000000005"));
        public static readonly TrackId TS6 = TrackId.Of(Guid.Parse("00000006-0000-0000-0001-000000000006"));
        public static readonly TrackId TS7 = TrackId.Of(Guid.Parse("00000006-0000-0000-0001-000000000007"));
        public static readonly TrackId TS8 = TrackId.Of(Guid.Parse("00000006-0000-0000-0001-000000000008"));
        public static readonly TrackId TS9 = TrackId.Of(Guid.Parse("00000006-0000-0000-0001-000000000009"));
    }

    // ══════════════════════════════════════════════════════════════════════════
    // Seed data
    // ══════════════════════════════════════════════════════════════════════════

    public static IReadOnlyList<Country> Countries =>
    [
        new Country { Id = CountryIds.Norway,        Name = "Norway",         Code = "NO" },
        new Country { Id = CountryIds.Sweden,        Name = "Sweden",         Code = "SE" },
        new Country { Id = CountryIds.Finland,       Name = "Finland",        Code = "FI" },
        new Country { Id = CountryIds.Poland,        Name = "Poland",         Code = "PL" },
        new Country { Id = CountryIds.Ukraine,       Name = "Ukraine",        Code = "UA" },
        new Country { Id = CountryIds.UnitedStates,  Name = "United States",  Code = "US" },
        new Country { Id = CountryIds.UnitedKingdom, Name = "United Kingdom", Code = "GB" },
        new Country { Id = CountryIds.Germany,       Name = "Germany",        Code = "DE" },
        new Country { Id = CountryIds.France,        Name = "France",         Code = "FR" },
    ];

    public static IReadOnlyList<Genre> Genres
    {
        get
        {
            var metal      = Genre.Create(GenreIds.Metal,      "Metal");
            var blackMetal = Genre.Create(GenreIds.BlackMetal, "Black Metal",  metal.Id);
            var deathMetal = Genre.Create(GenreIds.DeathMetal, "Death Metal",  metal.Id);
            var doomMetal  = Genre.Create(GenreIds.DoomMetal,  "Doom Metal",   metal.Id);
            var thrash     = Genre.Create(GenreIds.ThrashMetal,"Thrash Metal", metal.Id);

            return
            [
                metal,
                blackMetal, deathMetal, doomMetal, thrash,
                Genre.Create(GenreIds.RawBlackMetal,         "Raw Black Metal",         blackMetal.Id),
                Genre.Create(GenreIds.MelodicBlackMetal,     "Melodic Black Metal",     blackMetal.Id),
                Genre.Create(GenreIds.AtmosphericBlackMetal, "Atmospheric Black Metal", blackMetal.Id),
                Genre.Create(GenreIds.SymphonicBlackMetal,   "Symphonic Black Metal",   blackMetal.Id),
                Genre.Create(GenreIds.DepressiveBlackMetal,  "Depressive Black Metal",  blackMetal.Id),
                Genre.Create(GenreIds.MelodicDeathMetal,     "Melodic Death Metal",     deathMetal.Id),
                Genre.Create(GenreIds.TechnicalDeathMetal,   "Technical Death Metal",   deathMetal.Id),
                Genre.Create(GenreIds.BrutalDeathMetal,      "Brutal Death Metal",      deathMetal.Id),
                Genre.Create(GenreIds.FuneralDoom,           "Funeral Doom",            doomMetal.Id),
                Genre.Create(GenreIds.StonerDoom,            "Stoner Doom",             doomMetal.Id),
            ];
        }
    }

    public static IReadOnlyList<Track> Tracks =>
    [
        // Transilvanian Hunger — Darkthrone
        Track.Create(TrackIds.TH1, "Transilvanian Hunger"),
        Track.Create(TrackIds.TH2, "Over Fjell og Gjennom Torner"),
        Track.Create(TrackIds.TH3, "Skald av Satans Sol"),
        Track.Create(TrackIds.TH4, "Slottet i Det Fjerne"),
        Track.Create(TrackIds.TH5, "Graven Takeheimens Saler"),
        Track.Create(TrackIds.TH6, "I En Hall Med Flesk Og Mjod"),
        Track.Create(TrackIds.TH7, "As Flittermice As Satans Spys"),
        Track.Create(TrackIds.TH8, "En As I Dype Skogen"),

        // Filosofem — Burzum
        Track.Create(TrackIds.F1, "Dunkelheit"),
        Track.Create(TrackIds.F2, "Jesus' Tod"),
        Track.Create(TrackIds.F3, "Erblicket die Töchter des Firmaments"),
        Track.Create(TrackIds.F4, "Rundgang um die transzendentale Säule der Singularität"),

        // In the Nightside Eclipse — Emperor
        Track.Create(TrackIds.NE1, "Into the Infinity of Thoughts"),
        Track.Create(TrackIds.NE2, "The Burning Shadows of Silence"),
        Track.Create(TrackIds.NE3, "Cosmic Keys to My Creations & Times"),
        Track.Create(TrackIds.NE4, "Beyond the Great Vast Forest"),
        Track.Create(TrackIds.NE5, "Towards the Pantheon"),
        Track.Create(TrackIds.NE6, "The Majesty of the Night Sky"),
        Track.Create(TrackIds.NE7, "I Am the Black Wizards"),
        Track.Create(TrackIds.NE8, "Inno a Satana"),

        // De Mysteriis Dom Sathanas — Mayhem
        Track.Create(TrackIds.DM1, "Funeral Fog"),
        Track.Create(TrackIds.DM2, "Freezing Moon"),
        Track.Create(TrackIds.DM3, "Cursed in Eternity"),
        Track.Create(TrackIds.DM4, "Pagan Fears"),
        Track.Create(TrackIds.DM5, "Life Eternal"),
        Track.Create(TrackIds.DM6, "From the Dark Past"),
        Track.Create(TrackIds.DM7, "Buried by Time and Dust"),
        Track.Create(TrackIds.DM8, "De Mysteriis Dom Sathanas"),

        // Storm of the Light's Bane — Dissection
        Track.Create(TrackIds.SL1, "At the Fathomless Depths"),
        Track.Create(TrackIds.SL2, "Night's Blood"),
        Track.Create(TrackIds.SL3, "Unhallowed"),
        Track.Create(TrackIds.SL4, "Where Dead Angels Lie"),
        Track.Create(TrackIds.SL5, "Retribution - Storm of the Light's Bane"),
        Track.Create(TrackIds.SL6, "Thorns of Crimson Death"),
        Track.Create(TrackIds.SL7, "Soulreaper"),
        Track.Create(TrackIds.SL8, "No Dreams Breed in Breathless Sleep"),

        // The Satanist — Behemoth
        Track.Create(TrackIds.TS1, "Blow Your Trumpets Gabriel"),
        Track.Create(TrackIds.TS2, "Furor Divinus"),
        Track.Create(TrackIds.TS3, "Messe Noire"),
        Track.Create(TrackIds.TS4, "Ora Pro Nobis Lucifer"),
        Track.Create(TrackIds.TS5, "Amen"),
        Track.Create(TrackIds.TS6, "The Satanist"),
        Track.Create(TrackIds.TS7, "Ben Sahar"),
        Track.Create(TrackIds.TS8, "In the Absence Ov Light"),
        Track.Create(TrackIds.TS9, "O Father O Satan O Sun!"),
    ];

    public static IReadOnlyList<Band> Bands
    {
        get
        {
            var darkthrone = Band.Create(
                "Darkthrone",
                "Norwegian black metal band formed in 1986 in Kolbotn. One of the seminal acts of the second wave of black metal, known for their raw, lo-fi production and prolific output spanning over three decades.",
                null,
                BandActivity.Of(1986, null),
                BandStatus.Active,
                BandIds.Darkthrone);
            darkthrone.AddGenre(GenreIds.BlackMetal,    isPrimary: true);
            darkthrone.AddGenre(GenreIds.RawBlackMetal, isPrimary: false);
            darkthrone.AddCountry(CountryIds.Norway);

            var burzum = Band.Create(
                "Burzum",
                "Solo black metal project by Varg Vikernes, founded in Bergen, Norway in 1991. One of the most influential and controversial acts in black metal history, pioneering the atmospheric and ambient black metal style.",
                null,
                BandActivity.Of(1991, null),
                BandStatus.OnHold,
                BandIds.Burzum);
            burzum.AddGenre(GenreIds.BlackMetal,           isPrimary: true);
            burzum.AddGenre(GenreIds.AtmosphericBlackMetal, isPrimary: false);
            burzum.AddCountry(CountryIds.Norway);

            var emperor = Band.Create(
                "Emperor",
                "Norwegian black metal band formed in 1991 in Notodden. Pioneers of symphonic black metal, renowned for their complex compositions that merged aggressive black metal with orchestral arrangements and progressive structures.",
                null,
                BandActivity.Of(1991, 2001),
                BandStatus.SplitUp,
                BandIds.Emperor);
            emperor.AddGenre(GenreIds.BlackMetal,         isPrimary: true);
            emperor.AddGenre(GenreIds.SymphonicBlackMetal, isPrimary: false);
            emperor.AddCountry(CountryIds.Norway);

            var mayhem = Band.Create(
                "Mayhem",
                "Norwegian black metal band founded in 1984 in Oslo. One of the founders and most notorious acts of the Norwegian black metal scene, whose turbulent history and extreme ideology left an indelible mark on extreme metal.",
                null,
                BandActivity.Of(1984, null),
                BandStatus.Active,
                BandIds.Mayhem);
            mayhem.AddGenre(GenreIds.BlackMetal, isPrimary: true);
            mayhem.AddCountry(CountryIds.Norway);

            var dissection = Band.Create(
                "Dissection",
                "Swedish melodic black/death metal band formed in 1989 in Strömstad by Jon Nödtveidt. Known for their sophisticated blend of melodic death metal with black metal atmospherics, considered pioneers of the Scandinavian melodic extreme metal sound.",
                null,
                BandActivity.Of(1989, 2006),
                BandStatus.SplitUp,
                BandIds.Dissection);
            dissection.AddGenre(GenreIds.BlackMetal,        isPrimary: true);
            dissection.AddGenre(GenreIds.MelodicBlackMetal, isPrimary: false);
            dissection.AddCountry(CountryIds.Sweden);

            var behemoth = Band.Create(
                "Behemoth",
                "Polish extreme metal band formed in 1991 in Gdańsk by Nergal. Evolved from their early black metal roots into one of the world's foremost blackened death metal acts, known for their intense live performances and provocative imagery.",
                null,
                BandActivity.Of(1991, null),
                BandStatus.Active,
                BandIds.Behemoth);
            behemoth.AddGenre(GenreIds.BlackMetal, isPrimary: true);
            behemoth.AddGenre(GenreIds.DeathMetal, isPrimary: false);
            behemoth.AddCountry(CountryIds.Poland);

            return [darkthrone, burzum, emperor, mayhem, dissection, behemoth];
        }
    }

    public static IReadOnlyList<Album> Albums
    {
        get
        {
            var transilvanianHunger = Album.Create(
                "Transilvanian Hunger",
                AlbumType.FullLength,
                AlbumRelease.Of(1994, AlbumFormat.CD),
                "https://example.com/covers/transilvanian-hunger.jpg",
                LabelInfo.Of("Peaceville Records"),
                AlbumIds.TransilvanianHunger);
            transilvanianHunger.AddStreamingLink(StreamingPlatform.Spotify, "spotify:album:transilvanian-hunger-dummy");
            transilvanianHunger.AddStreamingLink(StreamingPlatform.Bandcamp, "https://darkthrone.bandcamp.com/album/transilvanian-hunger");
            transilvanianHunger.AddBand(BandIds.Darkthrone);
            transilvanianHunger.AddGenre(GenreIds.BlackMetal,    isPrimary: true);
            transilvanianHunger.AddGenre(GenreIds.RawBlackMetal, isPrimary: false);
            transilvanianHunger.AddCountry(CountryIds.Norway);
            transilvanianHunger.AddTrack(TrackIds.TH1, 1);
            transilvanianHunger.AddTrack(TrackIds.TH2, 2);
            transilvanianHunger.AddTrack(TrackIds.TH3, 3);
            transilvanianHunger.AddTrack(TrackIds.TH4, 4);
            transilvanianHunger.AddTrack(TrackIds.TH5, 5);
            transilvanianHunger.AddTrack(TrackIds.TH6, 6);
            transilvanianHunger.AddTrack(TrackIds.TH7, 7);
            transilvanianHunger.AddTrack(TrackIds.TH8, 8);

            var filosofem = Album.Create(
                "Filosofem",
                AlbumType.FullLength,
                AlbumRelease.Of(1996, AlbumFormat.CD),
                "https://example.com/covers/filosofem.jpg",
                LabelInfo.Of("Misanthropy Records"),
                AlbumIds.Filosofem);
            filosofem.AddStreamingLink(StreamingPlatform.Spotify, "spotify:album:filosofem-dummy");
            filosofem.AddStreamingLink(StreamingPlatform.YouTube, "https://www.youtube.com/playlist?list=filosofem-dummy");
            filosofem.AddBand(BandIds.Burzum);
            filosofem.AddGenre(GenreIds.BlackMetal,           isPrimary: true);
            filosofem.AddGenre(GenreIds.AtmosphericBlackMetal, isPrimary: false);
            filosofem.AddCountry(CountryIds.Norway);
            filosofem.AddTrack(TrackIds.F1, 1);
            filosofem.AddTrack(TrackIds.F2, 2);
            filosofem.AddTrack(TrackIds.F3, 3);
            filosofem.AddTrack(TrackIds.F4, 4);

            var nightsideEclipse = Album.Create(
                "In the Nightside Eclipse",
                AlbumType.FullLength,
                AlbumRelease.Of(1994, AlbumFormat.CD),
                "https://example.com/covers/in-the-nightside-eclipse.jpg",
                LabelInfo.Of("Candlelight Records"),
                AlbumIds.NightsideEclipse);
            nightsideEclipse.AddStreamingLink(StreamingPlatform.Spotify, "spotify:album:nightside-eclipse-dummy");
            nightsideEclipse.AddStreamingLink(StreamingPlatform.Tidal, "https://tidal.com/album/nightside-eclipse-dummy");
            nightsideEclipse.AddBand(BandIds.Emperor);
            nightsideEclipse.AddGenre(GenreIds.BlackMetal,         isPrimary: true);
            nightsideEclipse.AddGenre(GenreIds.SymphonicBlackMetal, isPrimary: false);
            nightsideEclipse.AddCountry(CountryIds.Norway);
            nightsideEclipse.AddTrack(TrackIds.NE1, 1);
            nightsideEclipse.AddTrack(TrackIds.NE2, 2);
            nightsideEclipse.AddTrack(TrackIds.NE3, 3);
            nightsideEclipse.AddTrack(TrackIds.NE4, 4);
            nightsideEclipse.AddTrack(TrackIds.NE5, 5);
            nightsideEclipse.AddTrack(TrackIds.NE6, 6);
            nightsideEclipse.AddTrack(TrackIds.NE7, 7);
            nightsideEclipse.AddTrack(TrackIds.NE8, 8);

            var deMysteriis = Album.Create(
                "De Mysteriis Dom Sathanas",
                AlbumType.FullLength,
                AlbumRelease.Of(1994, AlbumFormat.CD),
                "https://example.com/covers/de-mysteriis-dom-sathanas.jpg",
                LabelInfo.Of("Deathlike Silence Productions"),
                AlbumIds.DeMysteriis);
            deMysteriis.AddStreamingLink(StreamingPlatform.Spotify, "spotify:album:de-mysteriis-dummy");
            deMysteriis.AddStreamingLink(StreamingPlatform.Bandcamp, "https://mayhem.bandcamp.com/album/de-mysteriis-dom-sathanas");
            deMysteriis.AddBand(BandIds.Mayhem);
            deMysteriis.AddGenre(GenreIds.BlackMetal, isPrimary: true);
            deMysteriis.AddCountry(CountryIds.Norway);
            deMysteriis.AddTrack(TrackIds.DM1, 1);
            deMysteriis.AddTrack(TrackIds.DM2, 2);
            deMysteriis.AddTrack(TrackIds.DM3, 3);
            deMysteriis.AddTrack(TrackIds.DM4, 4);
            deMysteriis.AddTrack(TrackIds.DM5, 5);
            deMysteriis.AddTrack(TrackIds.DM6, 6);
            deMysteriis.AddTrack(TrackIds.DM7, 7);
            deMysteriis.AddTrack(TrackIds.DM8, 8);

            var stormOfLightsBane = Album.Create(
                "Storm of the Light's Bane",
                AlbumType.FullLength,
                AlbumRelease.Of(1995, AlbumFormat.CD),
                "https://example.com/covers/storm-of-the-lights-bane.jpg",
                LabelInfo.Of("Nuclear Blast"),
                AlbumIds.StormOfLightsBane);
            stormOfLightsBane.AddStreamingLink(StreamingPlatform.Spotify, "spotify:album:storm-of-lights-bane-dummy");
            stormOfLightsBane.AddStreamingLink(StreamingPlatform.AppleMusic, "https://music.apple.com/album/storm-of-lights-bane-dummy");
            stormOfLightsBane.AddBand(BandIds.Dissection);
            stormOfLightsBane.AddGenre(GenreIds.BlackMetal,        isPrimary: true);
            stormOfLightsBane.AddGenre(GenreIds.MelodicBlackMetal, isPrimary: false);
            stormOfLightsBane.AddCountry(CountryIds.Sweden);
            stormOfLightsBane.AddTrack(TrackIds.SL1, 1);
            stormOfLightsBane.AddTrack(TrackIds.SL2, 2);
            stormOfLightsBane.AddTrack(TrackIds.SL3, 3);
            stormOfLightsBane.AddTrack(TrackIds.SL4, 4);
            stormOfLightsBane.AddTrack(TrackIds.SL5, 5);
            stormOfLightsBane.AddTrack(TrackIds.SL6, 6);
            stormOfLightsBane.AddTrack(TrackIds.SL7, 7);
            stormOfLightsBane.AddTrack(TrackIds.SL8, 8);

            var theSatanist = Album.Create(
                "The Satanist",
                AlbumType.FullLength,
                AlbumRelease.Of(2014, AlbumFormat.CD),
                "https://example.com/covers/the-satanist.jpg",
                LabelInfo.Of("Nuclear Blast"),
                AlbumIds.TheSatanist);
            theSatanist.AddStreamingLink(StreamingPlatform.Spotify, "spotify:album:the-satanist-dummy");
            theSatanist.AddStreamingLink(StreamingPlatform.YouTube, "https://www.youtube.com/playlist?list=the-satanist-dummy");
            theSatanist.AddBand(BandIds.Behemoth);
            theSatanist.AddGenre(GenreIds.BlackMetal, isPrimary: true);
            theSatanist.AddGenre(GenreIds.DeathMetal, isPrimary: false);
            theSatanist.AddCountry(CountryIds.Poland);
            theSatanist.AddTrack(TrackIds.TS1, 1);
            theSatanist.AddTrack(TrackIds.TS2, 2);
            theSatanist.AddTrack(TrackIds.TS3, 3);
            theSatanist.AddTrack(TrackIds.TS4, 4);
            theSatanist.AddTrack(TrackIds.TS5, 5);
            theSatanist.AddTrack(TrackIds.TS6, 6);
            theSatanist.AddTrack(TrackIds.TS7, 7);
            theSatanist.AddTrack(TrackIds.TS8, 8);
            theSatanist.AddTrack(TrackIds.TS9, 9);

            return [transilvanianHunger, filosofem, nightsideEclipse, deMysteriis, stormOfLightsBane, theSatanist];
        }
    }
}
