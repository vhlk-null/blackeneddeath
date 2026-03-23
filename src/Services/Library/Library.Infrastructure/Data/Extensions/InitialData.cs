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
        public static readonly BandId Dissection        = BandId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000005"));
        public static readonly BandId Behemoth          = BandId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000006"));
        public static readonly BandId Abuser            = BandId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000007"));
        public static readonly BandId BloodCoven        = BandId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000008"));
        public static readonly BandId Deathwinds        = BandId.Of(Guid.Parse("b0000000-0000-0000-0000-000000000009"));
        public static readonly BandId MorsVoidDiscipline = BandId.Of(Guid.Parse("b0000000-0000-0000-0000-00000000000a"));
        public static readonly BandId Vider             = BandId.Of(Guid.Parse("b0000000-0000-0000-0000-00000000000b"));
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

        // Abuser (9)
        public static readonly AlbumId Abuser_1585         = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000007"));
        public static readonly AlbumId Abuser_GreatWork    = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000008"));
        public static readonly AlbumId Abuser_Merging      = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000009"));
        public static readonly AlbumId Abuser_Anneliese    = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000000a"));
        public static readonly AlbumId Abuser_BoundBySpells = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000000b"));
        public static readonly AlbumId Abuser_Conduit      = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000000c"));
        public static readonly AlbumId Abuser_DesolateDivine = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000000d"));
        public static readonly AlbumId Abuser_HisBestDeceit = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000000e"));
        public static readonly AlbumId Abuser_PsychicSecretions = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000000f"));

        // Blood Coven (6)
        public static readonly AlbumId BC_CaughtInTheUnlight   = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000010"));
        public static readonly AlbumId BC_ForTheHolyLord       = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000011"));
        public static readonly AlbumId BC_ContinuumHypothesis  = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000012"));
        public static readonly AlbumId BC_MolecularScythe      = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000013"));
        public static readonly AlbumId BC_ScreamToreSky        = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000014"));
        public static readonly AlbumId BC_WhatWillBe           = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000015"));

        // Deathwinds (8)
        public static readonly AlbumId DW_AuricGates          = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000016"));
        public static readonly AlbumId DW_BellumRegiis        = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000017"));
        public static readonly AlbumId DW_Crvsade             = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000018"));
        public static readonly AlbumId DW_Erebos              = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000019"));
        public static readonly AlbumId DW_Misanthropic        = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000001a"));
        public static readonly AlbumId DW_Rugia               = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000001b"));
        public static readonly AlbumId DW_Solarflesh          = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000001c"));
        public static readonly AlbumId DW_Tremendum           = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000001d"));

        // Mors.Void.Discipline (11)
        public static readonly AlbumId MVD_Exorkizein         = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000001e"));
        public static readonly AlbumId MVD_Malignancy         = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000001f"));
        public static readonly AlbumId MVD_OnslaughtKommand   = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000020"));
        public static readonly AlbumId MVD_PassioChristiI     = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000021"));
        public static readonly AlbumId MVD_PassioChristiII    = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000022"));
        public static readonly AlbumId MVD_Possession         = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000023"));
        public static readonly AlbumId MVD_Seidr              = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000024"));
        public static readonly AlbumId MVD_TheCall            = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000025"));
        public static readonly AlbumId MVD_MotherOfDarkness   = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000026"));
        public static readonly AlbumId MVD_ThirdAntichrist    = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000027"));
        public static readonly AlbumId MVD_WombOfLilithu      = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000028"));

        // Vider (7)
        public static readonly AlbumId Vider_Bloodhymns       = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-000000000029"));
        public static readonly AlbumId Vider_Darkside         = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000002a"));
        public static readonly AlbumId Vider_DawnOfDamned     = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000002b"));
        public static readonly AlbumId Vider_DeathToAll       = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000002c"));
        public static readonly AlbumId Vider_InTheTwilightGrey = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000002d"));
        public static readonly AlbumId Vider_MarkOfNecrogram  = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000002e"));
        public static readonly AlbumId Vider_NocturnalSilence = AlbumId.Of(Guid.Parse("a0000000-0000-0000-0000-00000000002f"));
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

            var abuser = Band.Create(
                "Abuser",
                "American black metal outfit known for hypnotic, dissonant riffing and occult lyricism. Their discography spans from raw early recordings to a more expansive atmospheric sound.",
                "Abuser_ua465w",
                BandActivity.Of(2004, null),
                BandStatus.Active,
                BandIds.Abuser);
            abuser.AddGenre(GenreIds.BlackMetal, isPrimary: true);
            abuser.AddGenre(GenreIds.DeathMetal, isPrimary: false);
            abuser.AddCountry(CountryIds.UnitedStates);

            var bloodCoven = Band.Create(
                "Blood Coven",
                "American black metal band rooted in orthodox darkness and ritualistic atmospheres. Their albums blend cold tremolo-picked aggression with dense, ceremonial structures.",
                "Blood_Coven_dsbu7i",
                BandActivity.Of(2001, null),
                BandStatus.Active,
                BandIds.BloodCoven);
            bloodCoven.AddGenre(GenreIds.BlackMetal, isPrimary: true);
            bloodCoven.AddCountry(CountryIds.UnitedStates);

            var deathwinds = Band.Create(
                "Deathwinds",
                "Polish black metal force drawing on Slavic mythology and martial themes. Their sound is relentless and austere, combining raw aggression with an unmistakable Eastern European coldness.",
                "Deathwinds_bee5xe",
                BandActivity.Of(2005, null),
                BandStatus.Active,
                BandIds.Deathwinds);
            deathwinds.AddGenre(GenreIds.BlackMetal, isPrimary: true);
            deathwinds.AddGenre(GenreIds.AtmosphericBlackMetal, isPrimary: false);
            deathwinds.AddCountry(CountryIds.Poland);

            var morsVoidDiscipline = Band.Create(
                "Mors.Void.Discipline",
                "French black metal entity steeped in occult theology and anti-cosmic philosophy. Operating under a doctrine of absolute negation, their works are dense and uncompromising ritual pieces.",
                "Mors.Void.Discipline_to2isd",
                BandActivity.Of(1998, null),
                BandStatus.Active,
                BandIds.MorsVoidDiscipline);
            morsVoidDiscipline.AddGenre(GenreIds.BlackMetal, isPrimary: true);
            morsVoidDiscipline.AddCountry(CountryIds.France);

            var vider = Band.Create(
                "Vider",
                "Swedish black metal band who helped define the Scandinavian second wave sound. Known for icy guitar textures, minimalist song structures, and an unwavering devotion to darkness.",
                "Vider_dvzm6c",
                BandActivity.Of(1992, null),
                BandStatus.Active,
                BandIds.Vider);
            vider.AddGenre(GenreIds.BlackMetal, isPrimary: true);
            vider.AddGenre(GenreIds.RawBlackMetal, isPrimary: false);
            vider.AddCountry(CountryIds.Sweden);

            return [darkthrone, burzum, emperor, mayhem, dissection, behemoth,
                    abuser, bloodCoven, deathwinds, morsVoidDiscipline, vider];
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

            // ── Abuser ────────────────────────────────────────────────────────────
            var a1585 = Album.Create("1585-1646", AlbumType.FullLength, AlbumRelease.Of(2009, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.Abuser_1585);
            a1585.AddBand(BandIds.Abuser); a1585.AddGenre(GenreIds.BlackMetal, isPrimary: true); a1585.AddCountry(CountryIds.UnitedStates);

            var aGreatWork = Album.Create("A Great Work of Ages", AlbumType.FullLength, AlbumRelease.Of(2019, AlbumFormat.CD), "His_Best_Deceit_divtip", null, AlbumIds.Abuser_GreatWork);
            aGreatWork.AddBand(BandIds.Abuser); aGreatWork.AddGenre(GenreIds.BlackMetal, isPrimary: true); aGreatWork.AddCountry(CountryIds.UnitedStates);

            var aMerging = Album.Create("A Merging to the Boundless", AlbumType.FullLength, AlbumRelease.Of(2013, AlbumFormat.CD), "Desolate_Divine_yu1kin", null, AlbumIds.Abuser_Merging);
            aMerging.AddBand(BandIds.Abuser); aMerging.AddGenre(GenreIds.BlackMetal, isPrimary: true); aMerging.AddCountry(CountryIds.UnitedStates);

            var anneliese = Album.Create("Anneliese", AlbumType.FullLength, AlbumRelease.Of(2011, AlbumFormat.CD), "Conduit_pe9plu", null, AlbumIds.Abuser_Anneliese);
            anneliese.AddBand(BandIds.Abuser); anneliese.AddGenre(GenreIds.BlackMetal, isPrimary: true); anneliese.AddCountry(CountryIds.UnitedStates);

            var boundBySpells = Album.Create("Bound by Spells", AlbumType.FullLength, AlbumRelease.Of(2016, AlbumFormat.CD), "Bound_By_Spells_pwiu6e", null, AlbumIds.Abuser_BoundBySpells);
            boundBySpells.AddBand(BandIds.Abuser); boundBySpells.AddGenre(GenreIds.BlackMetal, isPrimary: true); boundBySpells.AddCountry(CountryIds.UnitedStates);

            var conduit = Album.Create("Conduit", AlbumType.FullLength, AlbumRelease.Of(2021, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.Abuser_Conduit);
            conduit.AddBand(BandIds.Abuser); conduit.AddGenre(GenreIds.BlackMetal, isPrimary: true); conduit.AddCountry(CountryIds.UnitedStates);

            var desolateDivine = Album.Create("Desolate Divine", AlbumType.FullLength, AlbumRelease.Of(2018, AlbumFormat.CD), "His_Best_Deceit_divtip", null, AlbumIds.Abuser_DesolateDivine);
            desolateDivine.AddBand(BandIds.Abuser); desolateDivine.AddGenre(GenreIds.BlackMetal, isPrimary: true); desolateDivine.AddCountry(CountryIds.UnitedStates);

            var hisBestDeceit = Album.Create("His Best Deceit", AlbumType.FullLength, AlbumRelease.Of(2007, AlbumFormat.CD), "Desolate_Divine_yu1kin", null, AlbumIds.Abuser_HisBestDeceit);
            hisBestDeceit.AddBand(BandIds.Abuser); hisBestDeceit.AddGenre(GenreIds.BlackMetal, isPrimary: true); hisBestDeceit.AddCountry(CountryIds.UnitedStates);

            var psychicSecretions = Album.Create("Psychic Secretions", AlbumType.FullLength, AlbumRelease.Of(2022, AlbumFormat.CD), "Conduit_pe9plu", null, AlbumIds.Abuser_PsychicSecretions);
            psychicSecretions.AddBand(BandIds.Abuser); psychicSecretions.AddGenre(GenreIds.BlackMetal, isPrimary: true); psychicSecretions.AddCountry(CountryIds.UnitedStates);

            // ── Blood Coven ───────────────────────────────────────────────────────
            var caughtInUnlight = Album.Create("Caught in the Unlight!", AlbumType.FullLength, AlbumRelease.Of(2003, AlbumFormat.CD), "Bound_By_Spells_pwiu6e", null, AlbumIds.BC_CaughtInTheUnlight);
            caughtInUnlight.AddBand(BandIds.BloodCoven); caughtInUnlight.AddGenre(GenreIds.BlackMetal, isPrimary: true); caughtInUnlight.AddCountry(CountryIds.UnitedStates);

            var forTheHolyLord = Album.Create("For the Holy Lord", AlbumType.FullLength, AlbumRelease.Of(2007, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.BC_ForTheHolyLord);
            forTheHolyLord.AddBand(BandIds.BloodCoven); forTheHolyLord.AddGenre(GenreIds.BlackMetal, isPrimary: true); forTheHolyLord.AddCountry(CountryIds.UnitedStates);

            var continuumHypothesis = Album.Create("The Continuum Hypothesis", AlbumType.FullLength, AlbumRelease.Of(2012, AlbumFormat.CD), "His_Best_Deceit_divtip", null, AlbumIds.BC_ContinuumHypothesis);
            continuumHypothesis.AddBand(BandIds.BloodCoven); continuumHypothesis.AddGenre(GenreIds.BlackMetal, isPrimary: true); continuumHypothesis.AddCountry(CountryIds.UnitedStates);

            var molecularScythe = Album.Create("The Molecular Scythe", AlbumType.FullLength, AlbumRelease.Of(2015, AlbumFormat.CD), "Desolate_Divine_yu1kin", null, AlbumIds.BC_MolecularScythe);
            molecularScythe.AddBand(BandIds.BloodCoven); molecularScythe.AddGenre(GenreIds.BlackMetal, isPrimary: true); molecularScythe.AddCountry(CountryIds.UnitedStates);

            var screamToreSky = Album.Create("The Scream That Tore the Sky", AlbumType.FullLength, AlbumRelease.Of(2009, AlbumFormat.CD), "Conduit_pe9plu", null, AlbumIds.BC_ScreamToreSky);
            screamToreSky.AddBand(BandIds.BloodCoven); screamToreSky.AddGenre(GenreIds.BlackMetal, isPrimary: true); screamToreSky.AddCountry(CountryIds.UnitedStates);

            var whatWillBe = Album.Create("What Will Be Has Been", AlbumType.FullLength, AlbumRelease.Of(2018, AlbumFormat.CD), "Bound_By_Spells_pwiu6e", null, AlbumIds.BC_WhatWillBe);
            whatWillBe.AddBand(BandIds.BloodCoven); whatWillBe.AddGenre(GenreIds.BlackMetal, isPrimary: true); whatWillBe.AddCountry(CountryIds.UnitedStates);

            // ── Deathwinds ────────────────────────────────────────────────────────
            var auricGates = Album.Create("Auric Gates of Veles", AlbumType.FullLength, AlbumRelease.Of(2015, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.DW_AuricGates);
            auricGates.AddBand(BandIds.Deathwinds); auricGates.AddGenre(GenreIds.BlackMetal, isPrimary: true); auricGates.AddCountry(CountryIds.Poland);

            var bellumRegiis = Album.Create("Bellum Regiis", AlbumType.FullLength, AlbumRelease.Of(2012, AlbumFormat.CD), "His_Best_Deceit_divtip", null, AlbumIds.DW_BellumRegiis);
            bellumRegiis.AddBand(BandIds.Deathwinds); bellumRegiis.AddGenre(GenreIds.BlackMetal, isPrimary: true); bellumRegiis.AddCountry(CountryIds.Poland);

            var crvsade = Album.Create("Crvsade - Zero", AlbumType.FullLength, AlbumRelease.Of(2017, AlbumFormat.CD), "Desolate_Divine_yu1kin", null, AlbumIds.DW_Crvsade);
            crvsade.AddBand(BandIds.Deathwinds); crvsade.AddGenre(GenreIds.BlackMetal, isPrimary: true); crvsade.AddCountry(CountryIds.Poland);

            var erebos = Album.Create("Erebos", AlbumType.FullLength, AlbumRelease.Of(2008, AlbumFormat.CD), "Conduit_pe9plu", null, AlbumIds.DW_Erebos);
            erebos.AddBand(BandIds.Deathwinds); erebos.AddGenre(GenreIds.BlackMetal, isPrimary: true); erebos.AddCountry(CountryIds.Poland);

            var misanthropicPath = Album.Create("Misanthropic Path of Carnal Deliverance", AlbumType.FullLength, AlbumRelease.Of(2006, AlbumFormat.CD), "Bound_By_Spells_pwiu6e", null, AlbumIds.DW_Misanthropic);
            misanthropicPath.AddBand(BandIds.Deathwinds); misanthropicPath.AddGenre(GenreIds.BlackMetal, isPrimary: true); misanthropicPath.AddCountry(CountryIds.Poland);

            var rugia = Album.Create("Rugia", AlbumType.FullLength, AlbumRelease.Of(2019, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.DW_Rugia);
            rugia.AddBand(BandIds.Deathwinds); rugia.AddGenre(GenreIds.BlackMetal, isPrimary: true); rugia.AddCountry(CountryIds.Poland);

            var solarflesh = Album.Create("Solarflesh", AlbumType.FullLength, AlbumRelease.Of(2013, AlbumFormat.CD), "His_Best_Deceit_divtip", null, AlbumIds.DW_Solarflesh);
            solarflesh.AddBand(BandIds.Deathwinds); solarflesh.AddGenre(GenreIds.BlackMetal, isPrimary: true); solarflesh.AddCountry(CountryIds.Poland);

            var tremendum = Album.Create("Tremendum", AlbumType.FullLength, AlbumRelease.Of(2010, AlbumFormat.CD), "Desolate_Divine_yu1kin", null, AlbumIds.DW_Tremendum);
            tremendum.AddBand(BandIds.Deathwinds); tremendum.AddGenre(GenreIds.BlackMetal, isPrimary: true); tremendum.AddCountry(CountryIds.Poland);

            // ── Mors.Void.Discipline ──────────────────────────────────────────────
            var exorkizein = Album.Create("Exorkizein", AlbumType.FullLength, AlbumRelease.Of(2013, AlbumFormat.CD), "Conduit_pe9plu", null, AlbumIds.MVD_Exorkizein);
            exorkizein.AddBand(BandIds.MorsVoidDiscipline); exorkizein.AddGenre(GenreIds.BlackMetal, isPrimary: true); exorkizein.AddCountry(CountryIds.France);

            var malignancy = Album.Create("Malignancy", AlbumType.FullLength, AlbumRelease.Of(2006, AlbumFormat.CD), "Bound_By_Spells_pwiu6e", null, AlbumIds.MVD_Malignancy);
            malignancy.AddBand(BandIds.MorsVoidDiscipline); malignancy.AddGenre(GenreIds.BlackMetal, isPrimary: true); malignancy.AddCountry(CountryIds.France);

            var onslaughtKommand = Album.Create("Onslaught Kommand", AlbumType.FullLength, AlbumRelease.Of(2009, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.MVD_OnslaughtKommand);
            onslaughtKommand.AddBand(BandIds.MorsVoidDiscipline); onslaughtKommand.AddGenre(GenreIds.BlackMetal, isPrimary: true); onslaughtKommand.AddCountry(CountryIds.France);

            var passioChristiI = Album.Create("Passio Christi Part I", AlbumType.FullLength, AlbumRelease.Of(2015, AlbumFormat.CD), "His_Best_Deceit_divtip", null, AlbumIds.MVD_PassioChristiI);
            passioChristiI.AddBand(BandIds.MorsVoidDiscipline); passioChristiI.AddGenre(GenreIds.BlackMetal, isPrimary: true); passioChristiI.AddCountry(CountryIds.France);

            var passioChristiII = Album.Create("Passio Christi Part II", AlbumType.FullLength, AlbumRelease.Of(2016, AlbumFormat.CD), "Desolate_Divine_yu1kin", null, AlbumIds.MVD_PassioChristiII);
            passioChristiII.AddBand(BandIds.MorsVoidDiscipline); passioChristiII.AddGenre(GenreIds.BlackMetal, isPrimary: true); passioChristiII.AddCountry(CountryIds.France);

            var possession = Album.Create("Possession", AlbumType.FullLength, AlbumRelease.Of(2004, AlbumFormat.CD), "Conduit_pe9plu", null, AlbumIds.MVD_Possession);
            possession.AddBand(BandIds.MorsVoidDiscipline); possession.AddGenre(GenreIds.BlackMetal, isPrimary: true); possession.AddCountry(CountryIds.France);

            var seidr = Album.Create("Seiðr", AlbumType.FullLength, AlbumRelease.Of(2011, AlbumFormat.CD), "Bound_By_Spells_pwiu6e", null, AlbumIds.MVD_Seidr);
            seidr.AddBand(BandIds.MorsVoidDiscipline); seidr.AddGenre(GenreIds.BlackMetal, isPrimary: true); seidr.AddCountry(CountryIds.France);

            var theCall = Album.Create("The Call", AlbumType.FullLength, AlbumRelease.Of(2018, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.MVD_TheCall);
            theCall.AddBand(BandIds.MorsVoidDiscipline); theCall.AddGenre(GenreIds.BlackMetal, isPrimary: true); theCall.AddCountry(CountryIds.France);

            var motherOfDarkness = Album.Create("The Mother of Darkness", AlbumType.FullLength, AlbumRelease.Of(2008, AlbumFormat.CD), "His_Best_Deceit_divtip", null, AlbumIds.MVD_MotherOfDarkness);
            motherOfDarkness.AddBand(BandIds.MorsVoidDiscipline); motherOfDarkness.AddGenre(GenreIds.BlackMetal, isPrimary: true); motherOfDarkness.AddCountry(CountryIds.France);

            var thirdAntichrist = Album.Create("The Third Antichrist", AlbumType.FullLength, AlbumRelease.Of(2020, AlbumFormat.CD), "Desolate_Divine_yu1kin", null, AlbumIds.MVD_ThirdAntichrist);
            thirdAntichrist.AddBand(BandIds.MorsVoidDiscipline); thirdAntichrist.AddGenre(GenreIds.BlackMetal, isPrimary: true); thirdAntichrist.AddCountry(CountryIds.France);

            var wombOfLilithu = Album.Create("Womb of Lilithu", AlbumType.FullLength, AlbumRelease.Of(2002, AlbumFormat.CD), "Conduit_pe9plu", null, AlbumIds.MVD_WombOfLilithu);
            wombOfLilithu.AddBand(BandIds.MorsVoidDiscipline); wombOfLilithu.AddGenre(GenreIds.BlackMetal, isPrimary: true); wombOfLilithu.AddCountry(CountryIds.France);

            // ── Vider ─────────────────────────────────────────────────────────────
            var bloodhymns = Album.Create("Bloodhymns", AlbumType.FullLength, AlbumRelease.Of(2002, AlbumFormat.CD), "Bound_By_Spells_pwiu6e", null, AlbumIds.Vider_Bloodhymns);
            bloodhymns.AddBand(BandIds.Vider); bloodhymns.AddGenre(GenreIds.BlackMetal, isPrimary: true); bloodhymns.AddCountry(CountryIds.Sweden);

            var darkside = Album.Create("Darkside", AlbumType.FullLength, AlbumRelease.Of(1997, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.Vider_Darkside);
            darkside.AddBand(BandIds.Vider); darkside.AddGenre(GenreIds.BlackMetal, isPrimary: true); darkside.AddCountry(CountryIds.Sweden);

            var dawnOfDamned = Album.Create("Dawn of the Damned", AlbumType.FullLength, AlbumRelease.Of(2004, AlbumFormat.CD), "His_Best_Deceit_divtip", null, AlbumIds.Vider_DawnOfDamned);
            dawnOfDamned.AddBand(BandIds.Vider); dawnOfDamned.AddGenre(GenreIds.BlackMetal, isPrimary: true); dawnOfDamned.AddCountry(CountryIds.Sweden);

            var deathToAll = Album.Create("Death to All", AlbumType.FullLength, AlbumRelease.Of(2012, AlbumFormat.CD), "Desolate_Divine_yu1kin", null, AlbumIds.Vider_DeathToAll);
            deathToAll.AddBand(BandIds.Vider); deathToAll.AddGenre(GenreIds.BlackMetal, isPrimary: true); deathToAll.AddCountry(CountryIds.Sweden);

            var inTheTwilightGrey = Album.Create("In the Twilight Grey", AlbumType.FullLength, AlbumRelease.Of(1999, AlbumFormat.CD), "Conduit_pe9plu", null, AlbumIds.Vider_InTheTwilightGrey);
            inTheTwilightGrey.AddBand(BandIds.Vider); inTheTwilightGrey.AddGenre(GenreIds.BlackMetal, isPrimary: true); inTheTwilightGrey.AddCountry(CountryIds.Sweden);

            var markOfNecrogram = Album.Create("Mark of the Necrogram", AlbumType.FullLength, AlbumRelease.Of(2018, AlbumFormat.CD), "Bound_By_Spells_pwiu6e", null, AlbumIds.Vider_MarkOfNecrogram);
            markOfNecrogram.AddBand(BandIds.Vider); markOfNecrogram.AddGenre(GenreIds.BlackMetal, isPrimary: true); markOfNecrogram.AddCountry(CountryIds.Sweden);

            var nocturnalSilence = Album.Create("The Nocturnal Silence", AlbumType.FullLength, AlbumRelease.Of(1994, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.Vider_NocturnalSilence);
            nocturnalSilence.AddBand(BandIds.Vider); nocturnalSilence.AddGenre(GenreIds.BlackMetal, isPrimary: true); nocturnalSilence.AddCountry(CountryIds.Sweden);

            return [transilvanianHunger, filosofem, nightsideEclipse, deMysteriis, stormOfLightsBane, theSatanist,
                    a1585, aGreatWork, aMerging, anneliese, boundBySpells, conduit, desolateDivine, hisBestDeceit, psychicSecretions,
                    caughtInUnlight, forTheHolyLord, continuumHypothesis, molecularScythe, screamToreSky, whatWillBe,
                    auricGates, bellumRegiis, crvsade, erebos, misanthropicPath, rugia, solarflesh, tremendum,
                    exorkizein, malignancy, onslaughtKommand, passioChristiI, passioChristiII, possession, seidr, theCall, motherOfDarkness, thirdAntichrist, wombOfLilithu,
                    bloodhymns, darkside, dawnOfDamned, deathToAll, inTheTwilightGrey, markOfNecrogram, nocturnalSilence];
        }
    }
}
