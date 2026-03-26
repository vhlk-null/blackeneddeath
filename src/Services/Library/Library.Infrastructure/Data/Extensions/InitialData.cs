using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Domain.ValueObjects;
using Library.Domain.ValueObjects.Ids;

namespace Library.Infrastructure.Data.Extensions;

internal static class InitialData
{
    // ── Label IDs ──────────────────────────────────────────────────────────────
    private static class LabelIds
    {
        public static readonly LabelId Peaceville         = LabelId.Of(Guid.Parse("1a000000-0000-0000-0000-000000000001"));
        public static readonly LabelId Misanthropy         = LabelId.Of(Guid.Parse("1a000000-0000-0000-0000-000000000002"));
        public static readonly LabelId Candlelight         = LabelId.Of(Guid.Parse("1a000000-0000-0000-0000-000000000003"));
        public static readonly LabelId DeathlikeSilence    = LabelId.Of(Guid.Parse("1a000000-0000-0000-0000-000000000004"));
        public static readonly LabelId NuclearBlast        = LabelId.Of(Guid.Parse("1a000000-0000-0000-0000-000000000005"));
    }

    // ── Tag IDs ────────────────────────────────────────────────────────────────
    private static class TagIds
    {
        public static readonly TagId Atmospheric  = TagId.Of(Guid.Parse("7a000000-0000-0000-0000-000000000001"));
        public static readonly TagId Raw          = TagId.Of(Guid.Parse("7a000000-0000-0000-0000-000000000002"));
        public static readonly TagId Melancholic  = TagId.Of(Guid.Parse("7a000000-0000-0000-0000-000000000003"));
        public static readonly TagId Epic         = TagId.Of(Guid.Parse("7a000000-0000-0000-0000-000000000004"));
        public static readonly TagId Ambient      = TagId.Of(Guid.Parse("7a000000-0000-0000-0000-000000000005"));
        public static readonly TagId Progressive  = TagId.Of(Guid.Parse("7a000000-0000-0000-0000-000000000006"));
        public static readonly TagId OldSchool    = TagId.Of(Guid.Parse("7a000000-0000-0000-0000-000000000007"));
        public static readonly TagId Instrumental = TagId.Of(Guid.Parse("7a000000-0000-0000-0000-000000000008"));
        public static readonly TagId Experimental = TagId.Of(Guid.Parse("7a000000-0000-0000-0000-000000000009"));
        public static readonly TagId Kvlt         = TagId.Of(Guid.Parse("7a000000-0000-0000-0000-00000000000a"));
    }

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

        // Abuser — 1585-1646 (album 7)
        public static readonly TrackId A1_1 = TrackId.Of(Guid.Parse("00000007-0000-0000-0001-000000000001"));
        public static readonly TrackId A1_2 = TrackId.Of(Guid.Parse("00000007-0000-0000-0001-000000000002"));
        public static readonly TrackId A1_3 = TrackId.Of(Guid.Parse("00000007-0000-0000-0001-000000000003"));
        public static readonly TrackId A1_4 = TrackId.Of(Guid.Parse("00000007-0000-0000-0001-000000000004"));

        // Abuser — A Great Work of Ages (album 8)
        public static readonly TrackId A2_1 = TrackId.Of(Guid.Parse("00000008-0000-0000-0001-000000000001"));
        public static readonly TrackId A2_2 = TrackId.Of(Guid.Parse("00000008-0000-0000-0001-000000000002"));
        public static readonly TrackId A2_3 = TrackId.Of(Guid.Parse("00000008-0000-0000-0001-000000000003"));
        public static readonly TrackId A2_4 = TrackId.Of(Guid.Parse("00000008-0000-0000-0001-000000000004"));

        // Abuser — A Merging to the Boundless (album 9)
        public static readonly TrackId A3_1 = TrackId.Of(Guid.Parse("00000009-0000-0000-0001-000000000001"));
        public static readonly TrackId A3_2 = TrackId.Of(Guid.Parse("00000009-0000-0000-0001-000000000002"));
        public static readonly TrackId A3_3 = TrackId.Of(Guid.Parse("00000009-0000-0000-0001-000000000003"));
        public static readonly TrackId A3_4 = TrackId.Of(Guid.Parse("00000009-0000-0000-0001-000000000004"));

        // Abuser — Anneliese (album a)
        public static readonly TrackId A4_1 = TrackId.Of(Guid.Parse("0000000a-0000-0000-0001-000000000001"));
        public static readonly TrackId A4_2 = TrackId.Of(Guid.Parse("0000000a-0000-0000-0001-000000000002"));
        public static readonly TrackId A4_3 = TrackId.Of(Guid.Parse("0000000a-0000-0000-0001-000000000003"));
        public static readonly TrackId A4_4 = TrackId.Of(Guid.Parse("0000000a-0000-0000-0001-000000000004"));

        // Abuser — Bound by Spells (album b)
        public static readonly TrackId A5_1 = TrackId.Of(Guid.Parse("0000000b-0000-0000-0001-000000000001"));
        public static readonly TrackId A5_2 = TrackId.Of(Guid.Parse("0000000b-0000-0000-0001-000000000002"));
        public static readonly TrackId A5_3 = TrackId.Of(Guid.Parse("0000000b-0000-0000-0001-000000000003"));
        public static readonly TrackId A5_4 = TrackId.Of(Guid.Parse("0000000b-0000-0000-0001-000000000004"));

        // Abuser — Conduit (album c)
        public static readonly TrackId A6_1 = TrackId.Of(Guid.Parse("0000000c-0000-0000-0001-000000000001"));
        public static readonly TrackId A6_2 = TrackId.Of(Guid.Parse("0000000c-0000-0000-0001-000000000002"));
        public static readonly TrackId A6_3 = TrackId.Of(Guid.Parse("0000000c-0000-0000-0001-000000000003"));
        public static readonly TrackId A6_4 = TrackId.Of(Guid.Parse("0000000c-0000-0000-0001-000000000004"));

        // Abuser — Desolate Divine (album d)
        public static readonly TrackId A7_1 = TrackId.Of(Guid.Parse("0000000d-0000-0000-0001-000000000001"));
        public static readonly TrackId A7_2 = TrackId.Of(Guid.Parse("0000000d-0000-0000-0001-000000000002"));
        public static readonly TrackId A7_3 = TrackId.Of(Guid.Parse("0000000d-0000-0000-0001-000000000003"));
        public static readonly TrackId A7_4 = TrackId.Of(Guid.Parse("0000000d-0000-0000-0001-000000000004"));

        // Abuser — His Best Deceit (album e)
        public static readonly TrackId A8_1 = TrackId.Of(Guid.Parse("0000000e-0000-0000-0001-000000000001"));
        public static readonly TrackId A8_2 = TrackId.Of(Guid.Parse("0000000e-0000-0000-0001-000000000002"));
        public static readonly TrackId A8_3 = TrackId.Of(Guid.Parse("0000000e-0000-0000-0001-000000000003"));
        public static readonly TrackId A8_4 = TrackId.Of(Guid.Parse("0000000e-0000-0000-0001-000000000004"));

        // Abuser — Psychic Secretions (album f)
        public static readonly TrackId A9_1 = TrackId.Of(Guid.Parse("0000000f-0000-0000-0001-000000000001"));
        public static readonly TrackId A9_2 = TrackId.Of(Guid.Parse("0000000f-0000-0000-0001-000000000002"));
        public static readonly TrackId A9_3 = TrackId.Of(Guid.Parse("0000000f-0000-0000-0001-000000000003"));
        public static readonly TrackId A9_4 = TrackId.Of(Guid.Parse("0000000f-0000-0000-0001-000000000004"));

        // Blood Coven — Caught in the Unlight! (album 10)
        public static readonly TrackId BC1_1 = TrackId.Of(Guid.Parse("00000010-0000-0000-0001-000000000001"));
        public static readonly TrackId BC1_2 = TrackId.Of(Guid.Parse("00000010-0000-0000-0001-000000000002"));
        public static readonly TrackId BC1_3 = TrackId.Of(Guid.Parse("00000010-0000-0000-0001-000000000003"));
        public static readonly TrackId BC1_4 = TrackId.Of(Guid.Parse("00000010-0000-0000-0001-000000000004"));

        // Blood Coven — For the Holy Lord (album 11)
        public static readonly TrackId BC2_1 = TrackId.Of(Guid.Parse("00000011-0000-0000-0001-000000000001"));
        public static readonly TrackId BC2_2 = TrackId.Of(Guid.Parse("00000011-0000-0000-0001-000000000002"));
        public static readonly TrackId BC2_3 = TrackId.Of(Guid.Parse("00000011-0000-0000-0001-000000000003"));
        public static readonly TrackId BC2_4 = TrackId.Of(Guid.Parse("00000011-0000-0000-0001-000000000004"));

        // Blood Coven — The Continuum Hypothesis (album 12)
        public static readonly TrackId BC3_1 = TrackId.Of(Guid.Parse("00000012-0000-0000-0001-000000000001"));
        public static readonly TrackId BC3_2 = TrackId.Of(Guid.Parse("00000012-0000-0000-0001-000000000002"));
        public static readonly TrackId BC3_3 = TrackId.Of(Guid.Parse("00000012-0000-0000-0001-000000000003"));
        public static readonly TrackId BC3_4 = TrackId.Of(Guid.Parse("00000012-0000-0000-0001-000000000004"));

        // Blood Coven — The Molecular Scythe (album 13)
        public static readonly TrackId BC4_1 = TrackId.Of(Guid.Parse("00000013-0000-0000-0001-000000000001"));
        public static readonly TrackId BC4_2 = TrackId.Of(Guid.Parse("00000013-0000-0000-0001-000000000002"));
        public static readonly TrackId BC4_3 = TrackId.Of(Guid.Parse("00000013-0000-0000-0001-000000000003"));
        public static readonly TrackId BC4_4 = TrackId.Of(Guid.Parse("00000013-0000-0000-0001-000000000004"));

        // Blood Coven — The Scream That Tore the Sky (album 14)
        public static readonly TrackId BC5_1 = TrackId.Of(Guid.Parse("00000014-0000-0000-0001-000000000001"));
        public static readonly TrackId BC5_2 = TrackId.Of(Guid.Parse("00000014-0000-0000-0001-000000000002"));
        public static readonly TrackId BC5_3 = TrackId.Of(Guid.Parse("00000014-0000-0000-0001-000000000003"));
        public static readonly TrackId BC5_4 = TrackId.Of(Guid.Parse("00000014-0000-0000-0001-000000000004"));

        // Blood Coven — What Will Be Has Been (album 15)
        public static readonly TrackId BC6_1 = TrackId.Of(Guid.Parse("00000015-0000-0000-0001-000000000001"));
        public static readonly TrackId BC6_2 = TrackId.Of(Guid.Parse("00000015-0000-0000-0001-000000000002"));
        public static readonly TrackId BC6_3 = TrackId.Of(Guid.Parse("00000015-0000-0000-0001-000000000003"));
        public static readonly TrackId BC6_4 = TrackId.Of(Guid.Parse("00000015-0000-0000-0001-000000000004"));

        // Deathwinds — Auric Gates of Veles (album 16)
        public static readonly TrackId DW1_1 = TrackId.Of(Guid.Parse("00000016-0000-0000-0001-000000000001"));
        public static readonly TrackId DW1_2 = TrackId.Of(Guid.Parse("00000016-0000-0000-0001-000000000002"));
        public static readonly TrackId DW1_3 = TrackId.Of(Guid.Parse("00000016-0000-0000-0001-000000000003"));
        public static readonly TrackId DW1_4 = TrackId.Of(Guid.Parse("00000016-0000-0000-0001-000000000004"));

        // Deathwinds — Bellum Regiis (album 17)
        public static readonly TrackId DW2_1 = TrackId.Of(Guid.Parse("00000017-0000-0000-0001-000000000001"));
        public static readonly TrackId DW2_2 = TrackId.Of(Guid.Parse("00000017-0000-0000-0001-000000000002"));
        public static readonly TrackId DW2_3 = TrackId.Of(Guid.Parse("00000017-0000-0000-0001-000000000003"));
        public static readonly TrackId DW2_4 = TrackId.Of(Guid.Parse("00000017-0000-0000-0001-000000000004"));

        // Deathwinds — Crvsade - Zero (album 18)
        public static readonly TrackId DW3_1 = TrackId.Of(Guid.Parse("00000018-0000-0000-0001-000000000001"));
        public static readonly TrackId DW3_2 = TrackId.Of(Guid.Parse("00000018-0000-0000-0001-000000000002"));
        public static readonly TrackId DW3_3 = TrackId.Of(Guid.Parse("00000018-0000-0000-0001-000000000003"));
        public static readonly TrackId DW3_4 = TrackId.Of(Guid.Parse("00000018-0000-0000-0001-000000000004"));

        // Deathwinds — Erebos (album 19)
        public static readonly TrackId DW4_1 = TrackId.Of(Guid.Parse("00000019-0000-0000-0001-000000000001"));
        public static readonly TrackId DW4_2 = TrackId.Of(Guid.Parse("00000019-0000-0000-0001-000000000002"));
        public static readonly TrackId DW4_3 = TrackId.Of(Guid.Parse("00000019-0000-0000-0001-000000000003"));
        public static readonly TrackId DW4_4 = TrackId.Of(Guid.Parse("00000019-0000-0000-0001-000000000004"));

        // Deathwinds — Misanthropic Path of Carnal Deliverance (album 1a)
        public static readonly TrackId DW5_1 = TrackId.Of(Guid.Parse("0000001a-0000-0000-0001-000000000001"));
        public static readonly TrackId DW5_2 = TrackId.Of(Guid.Parse("0000001a-0000-0000-0001-000000000002"));
        public static readonly TrackId DW5_3 = TrackId.Of(Guid.Parse("0000001a-0000-0000-0001-000000000003"));
        public static readonly TrackId DW5_4 = TrackId.Of(Guid.Parse("0000001a-0000-0000-0001-000000000004"));

        // Deathwinds — Rugia (album 1b)
        public static readonly TrackId DW6_1 = TrackId.Of(Guid.Parse("0000001b-0000-0000-0001-000000000001"));
        public static readonly TrackId DW6_2 = TrackId.Of(Guid.Parse("0000001b-0000-0000-0001-000000000002"));
        public static readonly TrackId DW6_3 = TrackId.Of(Guid.Parse("0000001b-0000-0000-0001-000000000003"));
        public static readonly TrackId DW6_4 = TrackId.Of(Guid.Parse("0000001b-0000-0000-0001-000000000004"));

        // Deathwinds — Solarflesh (album 1c)
        public static readonly TrackId DW7_1 = TrackId.Of(Guid.Parse("0000001c-0000-0000-0001-000000000001"));
        public static readonly TrackId DW7_2 = TrackId.Of(Guid.Parse("0000001c-0000-0000-0001-000000000002"));
        public static readonly TrackId DW7_3 = TrackId.Of(Guid.Parse("0000001c-0000-0000-0001-000000000003"));
        public static readonly TrackId DW7_4 = TrackId.Of(Guid.Parse("0000001c-0000-0000-0001-000000000004"));

        // Deathwinds — Tremendum (album 1d)
        public static readonly TrackId DW8_1 = TrackId.Of(Guid.Parse("0000001d-0000-0000-0001-000000000001"));
        public static readonly TrackId DW8_2 = TrackId.Of(Guid.Parse("0000001d-0000-0000-0001-000000000002"));
        public static readonly TrackId DW8_3 = TrackId.Of(Guid.Parse("0000001d-0000-0000-0001-000000000003"));
        public static readonly TrackId DW8_4 = TrackId.Of(Guid.Parse("0000001d-0000-0000-0001-000000000004"));

        // MVD — Exorkizein (album 1e)
        public static readonly TrackId M1_1 = TrackId.Of(Guid.Parse("0000001e-0000-0000-0001-000000000001"));
        public static readonly TrackId M1_2 = TrackId.Of(Guid.Parse("0000001e-0000-0000-0001-000000000002"));
        public static readonly TrackId M1_3 = TrackId.Of(Guid.Parse("0000001e-0000-0000-0001-000000000003"));
        public static readonly TrackId M1_4 = TrackId.Of(Guid.Parse("0000001e-0000-0000-0001-000000000004"));

        // MVD — Malignancy (album 1f)
        public static readonly TrackId M2_1 = TrackId.Of(Guid.Parse("0000001f-0000-0000-0001-000000000001"));
        public static readonly TrackId M2_2 = TrackId.Of(Guid.Parse("0000001f-0000-0000-0001-000000000002"));
        public static readonly TrackId M2_3 = TrackId.Of(Guid.Parse("0000001f-0000-0000-0001-000000000003"));
        public static readonly TrackId M2_4 = TrackId.Of(Guid.Parse("0000001f-0000-0000-0001-000000000004"));

        // MVD — Onslaught Kommand (album 20)
        public static readonly TrackId M3_1 = TrackId.Of(Guid.Parse("00000020-0000-0000-0001-000000000001"));
        public static readonly TrackId M3_2 = TrackId.Of(Guid.Parse("00000020-0000-0000-0001-000000000002"));
        public static readonly TrackId M3_3 = TrackId.Of(Guid.Parse("00000020-0000-0000-0001-000000000003"));
        public static readonly TrackId M3_4 = TrackId.Of(Guid.Parse("00000020-0000-0000-0001-000000000004"));

        // MVD — Passio Christi Part I (album 21)
        public static readonly TrackId M4_1 = TrackId.Of(Guid.Parse("00000021-0000-0000-0001-000000000001"));
        public static readonly TrackId M4_2 = TrackId.Of(Guid.Parse("00000021-0000-0000-0001-000000000002"));
        public static readonly TrackId M4_3 = TrackId.Of(Guid.Parse("00000021-0000-0000-0001-000000000003"));
        public static readonly TrackId M4_4 = TrackId.Of(Guid.Parse("00000021-0000-0000-0001-000000000004"));

        // MVD — Passio Christi Part II (album 22)
        public static readonly TrackId M5_1 = TrackId.Of(Guid.Parse("00000022-0000-0000-0001-000000000001"));
        public static readonly TrackId M5_2 = TrackId.Of(Guid.Parse("00000022-0000-0000-0001-000000000002"));
        public static readonly TrackId M5_3 = TrackId.Of(Guid.Parse("00000022-0000-0000-0001-000000000003"));
        public static readonly TrackId M5_4 = TrackId.Of(Guid.Parse("00000022-0000-0000-0001-000000000004"));

        // MVD — Possession (album 23)
        public static readonly TrackId M6_1 = TrackId.Of(Guid.Parse("00000023-0000-0000-0001-000000000001"));
        public static readonly TrackId M6_2 = TrackId.Of(Guid.Parse("00000023-0000-0000-0001-000000000002"));
        public static readonly TrackId M6_3 = TrackId.Of(Guid.Parse("00000023-0000-0000-0001-000000000003"));
        public static readonly TrackId M6_4 = TrackId.Of(Guid.Parse("00000023-0000-0000-0001-000000000004"));

        // MVD — Seiðr (album 24)
        public static readonly TrackId M7_1 = TrackId.Of(Guid.Parse("00000024-0000-0000-0001-000000000001"));
        public static readonly TrackId M7_2 = TrackId.Of(Guid.Parse("00000024-0000-0000-0001-000000000002"));
        public static readonly TrackId M7_3 = TrackId.Of(Guid.Parse("00000024-0000-0000-0001-000000000003"));
        public static readonly TrackId M7_4 = TrackId.Of(Guid.Parse("00000024-0000-0000-0001-000000000004"));

        // MVD — The Call (album 25)
        public static readonly TrackId M8_1 = TrackId.Of(Guid.Parse("00000025-0000-0000-0001-000000000001"));
        public static readonly TrackId M8_2 = TrackId.Of(Guid.Parse("00000025-0000-0000-0001-000000000002"));
        public static readonly TrackId M8_3 = TrackId.Of(Guid.Parse("00000025-0000-0000-0001-000000000003"));
        public static readonly TrackId M8_4 = TrackId.Of(Guid.Parse("00000025-0000-0000-0001-000000000004"));

        // MVD — The Mother of Darkness (album 26)
        public static readonly TrackId M9_1 = TrackId.Of(Guid.Parse("00000026-0000-0000-0001-000000000001"));
        public static readonly TrackId M9_2 = TrackId.Of(Guid.Parse("00000026-0000-0000-0001-000000000002"));
        public static readonly TrackId M9_3 = TrackId.Of(Guid.Parse("00000026-0000-0000-0001-000000000003"));
        public static readonly TrackId M9_4 = TrackId.Of(Guid.Parse("00000026-0000-0000-0001-000000000004"));

        // MVD — The Third Antichrist (album 27)
        public static readonly TrackId M10_1 = TrackId.Of(Guid.Parse("00000027-0000-0000-0001-000000000001"));
        public static readonly TrackId M10_2 = TrackId.Of(Guid.Parse("00000027-0000-0000-0001-000000000002"));
        public static readonly TrackId M10_3 = TrackId.Of(Guid.Parse("00000027-0000-0000-0001-000000000003"));
        public static readonly TrackId M10_4 = TrackId.Of(Guid.Parse("00000027-0000-0000-0001-000000000004"));

        // MVD — Womb of Lilithu (album 28)
        public static readonly TrackId M11_1 = TrackId.Of(Guid.Parse("00000028-0000-0000-0001-000000000001"));
        public static readonly TrackId M11_2 = TrackId.Of(Guid.Parse("00000028-0000-0000-0001-000000000002"));
        public static readonly TrackId M11_3 = TrackId.Of(Guid.Parse("00000028-0000-0000-0001-000000000003"));
        public static readonly TrackId M11_4 = TrackId.Of(Guid.Parse("00000028-0000-0000-0001-000000000004"));

        // Vider — Bloodhymns (album 29)
        public static readonly TrackId V1_1 = TrackId.Of(Guid.Parse("00000029-0000-0000-0001-000000000001"));
        public static readonly TrackId V1_2 = TrackId.Of(Guid.Parse("00000029-0000-0000-0001-000000000002"));
        public static readonly TrackId V1_3 = TrackId.Of(Guid.Parse("00000029-0000-0000-0001-000000000003"));
        public static readonly TrackId V1_4 = TrackId.Of(Guid.Parse("00000029-0000-0000-0001-000000000004"));

        // Vider — Darkside (album 2a)
        public static readonly TrackId V2_1 = TrackId.Of(Guid.Parse("0000002a-0000-0000-0001-000000000001"));
        public static readonly TrackId V2_2 = TrackId.Of(Guid.Parse("0000002a-0000-0000-0001-000000000002"));
        public static readonly TrackId V2_3 = TrackId.Of(Guid.Parse("0000002a-0000-0000-0001-000000000003"));
        public static readonly TrackId V2_4 = TrackId.Of(Guid.Parse("0000002a-0000-0000-0001-000000000004"));

        // Vider — Dawn of the Damned (album 2b)
        public static readonly TrackId V3_1 = TrackId.Of(Guid.Parse("0000002b-0000-0000-0001-000000000001"));
        public static readonly TrackId V3_2 = TrackId.Of(Guid.Parse("0000002b-0000-0000-0001-000000000002"));
        public static readonly TrackId V3_3 = TrackId.Of(Guid.Parse("0000002b-0000-0000-0001-000000000003"));
        public static readonly TrackId V3_4 = TrackId.Of(Guid.Parse("0000002b-0000-0000-0001-000000000004"));

        // Vider — Death to All (album 2c)
        public static readonly TrackId V4_1 = TrackId.Of(Guid.Parse("0000002c-0000-0000-0001-000000000001"));
        public static readonly TrackId V4_2 = TrackId.Of(Guid.Parse("0000002c-0000-0000-0001-000000000002"));
        public static readonly TrackId V4_3 = TrackId.Of(Guid.Parse("0000002c-0000-0000-0001-000000000003"));
        public static readonly TrackId V4_4 = TrackId.Of(Guid.Parse("0000002c-0000-0000-0001-000000000004"));

        // Vider — In the Twilight Grey (album 2d)
        public static readonly TrackId V5_1 = TrackId.Of(Guid.Parse("0000002d-0000-0000-0001-000000000001"));
        public static readonly TrackId V5_2 = TrackId.Of(Guid.Parse("0000002d-0000-0000-0001-000000000002"));
        public static readonly TrackId V5_3 = TrackId.Of(Guid.Parse("0000002d-0000-0000-0001-000000000003"));
        public static readonly TrackId V5_4 = TrackId.Of(Guid.Parse("0000002d-0000-0000-0001-000000000004"));

        // Vider — Mark of the Necrogram (album 2e)
        public static readonly TrackId V6_1 = TrackId.Of(Guid.Parse("0000002e-0000-0000-0001-000000000001"));
        public static readonly TrackId V6_2 = TrackId.Of(Guid.Parse("0000002e-0000-0000-0001-000000000002"));
        public static readonly TrackId V6_3 = TrackId.Of(Guid.Parse("0000002e-0000-0000-0001-000000000003"));
        public static readonly TrackId V6_4 = TrackId.Of(Guid.Parse("0000002e-0000-0000-0001-000000000004"));

        // Vider — The Nocturnal Silence (album 2f)
        public static readonly TrackId V7_1 = TrackId.Of(Guid.Parse("0000002f-0000-0000-0001-000000000001"));
        public static readonly TrackId V7_2 = TrackId.Of(Guid.Parse("0000002f-0000-0000-0001-000000000002"));
        public static readonly TrackId V7_3 = TrackId.Of(Guid.Parse("0000002f-0000-0000-0001-000000000003"));
        public static readonly TrackId V7_4 = TrackId.Of(Guid.Parse("0000002f-0000-0000-0001-000000000004"));
    }

    // ══════════════════════════════════════════════════════════════════════════
    // Seed data
    // ══════════════════════════════════════════════════════════════════════════

    public static IReadOnlyList<Country> Countries =>
    [
        Country.Create(CountryIds.Norway,        "Norway",         "NO"),
        Country.Create(CountryIds.Sweden,        "Sweden",         "SE"),
        Country.Create(CountryIds.Finland,       "Finland",        "FI"),
        Country.Create(CountryIds.Poland,        "Poland",         "PL"),
        Country.Create(CountryIds.Ukraine,       "Ukraine",        "UA"),
        Country.Create(CountryIds.UnitedStates,  "United States",  "US"),
        Country.Create(CountryIds.UnitedKingdom, "United Kingdom", "GB"),
        Country.Create(CountryIds.Germany,       "Germany",        "DE"),
        Country.Create(CountryIds.France,        "France",         "FR"),
    ];

    public static IReadOnlyList<Tag> Tags =>
    [
        Tag.Create(TagIds.Atmospheric,  "Atmospheric"),
        Tag.Create(TagIds.Raw,          "Raw"),
        Tag.Create(TagIds.Melancholic,  "Melancholic"),
        Tag.Create(TagIds.Epic,         "Epic"),
        Tag.Create(TagIds.Ambient,      "Ambient"),
        Tag.Create(TagIds.Progressive,  "Progressive"),
        Tag.Create(TagIds.OldSchool,    "Old School"),
        Tag.Create(TagIds.Instrumental, "Instrumental"),
        Tag.Create(TagIds.Experimental, "Experimental"),
        Tag.Create(TagIds.Kvlt,         "Kvlt"),
    ];

    public static IReadOnlyList<Label> Labels =>
    [
        Label.Create(LabelIds.Peaceville,      "Peaceville Records"),
        Label.Create(LabelIds.Misanthropy,     "Misanthropy Records"),
        Label.Create(LabelIds.Candlelight,     "Candlelight Records"),
        Label.Create(LabelIds.DeathlikeSilence,"Deathlike Silence Productions"),
        Label.Create(LabelIds.NuclearBlast,    "Nuclear Blast"),
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

        // 1585-1646 — Abuser
        Track.Create(TrackIds.A1_1, "Hex of the Burning Dark"),
        Track.Create(TrackIds.A1_2, "Summoning the Void"),
        Track.Create(TrackIds.A1_3, "Abyssal Rites"),
        Track.Create(TrackIds.A1_4, "Black Cathedral"),

        // A Great Work of Ages — Abuser
        Track.Create(TrackIds.A2_1, "The Ancient Calling"),
        Track.Create(TrackIds.A2_2, "Void Manifestation"),
        Track.Create(TrackIds.A2_3, "Crimson Rites"),
        Track.Create(TrackIds.A2_4, "Into the Abyss"),

        // A Merging to the Boundless — Abuser
        Track.Create(TrackIds.A3_1, "Beyond the Gates"),
        Track.Create(TrackIds.A3_2, "Boundless Darkness"),
        Track.Create(TrackIds.A3_3, "Spectral Ascent"),
        Track.Create(TrackIds.A3_4, "Void Walker"),

        // Anneliese — Abuser
        Track.Create(TrackIds.A4_1, "Possession"),
        Track.Create(TrackIds.A4_2, "The Exorcism"),
        Track.Create(TrackIds.A4_3, "Dark Invocation"),
        Track.Create(TrackIds.A4_4, "Anneliese"),

        // Bound by Spells — Abuser
        Track.Create(TrackIds.A5_1, "Spellbound"),
        Track.Create(TrackIds.A5_2, "Chains of the Dark"),
        Track.Create(TrackIds.A5_3, "Occult Binding"),
        Track.Create(TrackIds.A5_4, "Forbidden Rites"),

        // Conduit — Abuser
        Track.Create(TrackIds.A6_1, "Through the Veil"),
        Track.Create(TrackIds.A6_2, "Dark Conduit"),
        Track.Create(TrackIds.A6_3, "Channeling"),
        Track.Create(TrackIds.A6_4, "Void Current"),

        // Desolate Divine — Abuser
        Track.Create(TrackIds.A7_1, "Forsaken God"),
        Track.Create(TrackIds.A7_2, "Desolate Altar"),
        Track.Create(TrackIds.A7_3, "The Fallen"),
        Track.Create(TrackIds.A7_4, "Sacred Ruin"),

        // His Best Deceit — Abuser
        Track.Create(TrackIds.A8_1, "Father of Lies"),
        Track.Create(TrackIds.A8_2, "Serpent's Tongue"),
        Track.Create(TrackIds.A8_3, "Deception Rites"),
        Track.Create(TrackIds.A8_4, "His Greatest Lie"),

        // Psychic Secretions — Abuser
        Track.Create(TrackIds.A9_1, "Mind Virus"),
        Track.Create(TrackIds.A9_2, "Psychic Leakage"),
        Track.Create(TrackIds.A9_3, "Neural Decay"),
        Track.Create(TrackIds.A9_4, "Cerebral Void"),

        // Caught in the Unlight! — Blood Coven
        Track.Create(TrackIds.BC1_1, "Unlight"),
        Track.Create(TrackIds.BC1_2, "Shroud of Darkness"),
        Track.Create(TrackIds.BC1_3, "The Coven Gathers"),
        Track.Create(TrackIds.BC1_4, "Sacrificial Night"),

        // For the Holy Lord — Blood Coven
        Track.Create(TrackIds.BC2_1, "Blasphemous Praise"),
        Track.Create(TrackIds.BC2_2, "Dark Liturgy"),
        Track.Create(TrackIds.BC2_3, "The Holy Lies"),
        Track.Create(TrackIds.BC2_4, "Antichrist Rising"),

        // The Continuum Hypothesis — Blood Coven
        Track.Create(TrackIds.BC3_1, "Infinite Regression"),
        Track.Create(TrackIds.BC3_2, "The Dark Equation"),
        Track.Create(TrackIds.BC3_3, "Beyond Reality"),
        Track.Create(TrackIds.BC3_4, "Temporal Collapse"),

        // The Molecular Scythe — Blood Coven
        Track.Create(TrackIds.BC4_1, "Harvesting Souls"),
        Track.Create(TrackIds.BC4_2, "Atomic Damnation"),
        Track.Create(TrackIds.BC4_3, "The Reaping"),
        Track.Create(TrackIds.BC4_4, "Scythe of Night"),

        // The Scream That Tore the Sky — Blood Coven
        Track.Create(TrackIds.BC5_1, "Heavens Rupture"),
        Track.Create(TrackIds.BC5_2, "The Great Wail"),
        Track.Create(TrackIds.BC5_3, "Sky Splitting Agony"),
        Track.Create(TrackIds.BC5_4, "Celestial Terror"),

        // What Will Be Has Been — Blood Coven
        Track.Create(TrackIds.BC6_1, "Predestined Darkness"),
        Track.Create(TrackIds.BC6_2, "The Eternal Return"),
        Track.Create(TrackIds.BC6_3, "Fated to Fall"),
        Track.Create(TrackIds.BC6_4, "Ordained by Night"),

        // Auric Gates of Veles — Deathwinds
        Track.Create(TrackIds.DW1_1, "Through the Golden Gates"),
        Track.Create(TrackIds.DW1_2, "Slavic Darkness"),
        Track.Create(TrackIds.DW1_3, "Death God's Domain"),
        Track.Create(TrackIds.DW1_4, "Into Veles' Kingdom"),

        // Bellum Regiis — Deathwinds
        Track.Create(TrackIds.DW2_1, "War of Kings"),
        Track.Create(TrackIds.DW2_2, "Royal Bloodshed"),
        Track.Create(TrackIds.DW2_3, "The King Falls"),
        Track.Create(TrackIds.DW2_4, "Throne of Ash"),

        // Crvsade - Zero — Deathwinds
        Track.Create(TrackIds.DW3_1, "March of the Damned"),
        Track.Create(TrackIds.DW3_2, "Siege of Heaven"),
        Track.Create(TrackIds.DW3_3, "Crusaders Fall"),
        Track.Create(TrackIds.DW3_4, "Zero Hour"),

        // Erebos — Deathwinds
        Track.Create(TrackIds.DW4_1, "Son of Night"),
        Track.Create(TrackIds.DW4_2, "Primordial Dark"),
        Track.Create(TrackIds.DW4_3, "Before the Dawn"),
        Track.Create(TrackIds.DW4_4, "The Abyss Eternal"),

        // Misanthropic Path of Carnal Deliverance — Deathwinds
        Track.Create(TrackIds.DW5_1, "Hatred Manifest"),
        Track.Create(TrackIds.DW5_2, "Flesh Transcended"),
        Track.Create(TrackIds.DW5_3, "Path of Scorn"),
        Track.Create(TrackIds.DW5_4, "Carnal Ascent"),

        // Rugia — Deathwinds
        Track.Create(TrackIds.DW6_1, "Island of the Slavs"),
        Track.Create(TrackIds.DW6_2, "Ancient Blood"),
        Track.Create(TrackIds.DW6_3, "Pagan Shores"),
        Track.Create(TrackIds.DW6_4, "Return to Rügen"),

        // Solarflesh — Deathwinds
        Track.Create(TrackIds.DW7_1, "Consumed by the Sun"),
        Track.Create(TrackIds.DW7_2, "Solar Wound"),
        Track.Create(TrackIds.DW7_3, "Radiation of Darkness"),
        Track.Create(TrackIds.DW7_4, "The Burning"),

        // Tremendum — Deathwinds
        Track.Create(TrackIds.DW8_1, "The Great Terror"),
        Track.Create(TrackIds.DW8_2, "Awe and Dread"),
        Track.Create(TrackIds.DW8_3, "Sublime Darkness"),
        Track.Create(TrackIds.DW8_4, "Vast Emptiness"),

        // Exorkizein — Mors.Void.Discipline
        Track.Create(TrackIds.M1_1, "Banishment Rites"),
        Track.Create(TrackIds.M1_2, "The Expulsion"),
        Track.Create(TrackIds.M1_3, "Void Exorcism"),
        Track.Create(TrackIds.M1_4, "Rite of Undoing"),

        // Malignancy — Mors.Void.Discipline
        Track.Create(TrackIds.M2_1, "Diseased Spirit"),
        Track.Create(TrackIds.M2_2, "Malignant Growth"),
        Track.Create(TrackIds.M2_3, "The Spreading Dark"),
        Track.Create(TrackIds.M2_4, "Terminal Void"),

        // Onslaught Kommand — Mors.Void.Discipline
        Track.Create(TrackIds.M3_1, "Assault Absolute"),
        Track.Create(TrackIds.M3_2, "Command of Destruction"),
        Track.Create(TrackIds.M3_3, "Storm of Negation"),
        Track.Create(TrackIds.M3_4, "The Kommand"),

        // Passio Christi Part I — Mors.Void.Discipline
        Track.Create(TrackIds.M4_1, "The Suffering Begins"),
        Track.Create(TrackIds.M4_2, "Crown of Thorns"),
        Track.Create(TrackIds.M4_3, "Gethsemane"),
        Track.Create(TrackIds.M4_4, "The Betrayal"),

        // Passio Christi Part II — Mors.Void.Discipline
        Track.Create(TrackIds.M5_1, "Golgotha"),
        Track.Create(TrackIds.M5_2, "The Crucifixion"),
        Track.Create(TrackIds.M5_3, "Harrowing of Hell"),
        Track.Create(TrackIds.M5_4, "Via Dolorosa"),

        // Possession — Mors.Void.Discipline
        Track.Create(TrackIds.M6_1, "Invoked"),
        Track.Create(TrackIds.M6_2, "The Entry"),
        Track.Create(TrackIds.M6_3, "Host Devoured"),
        Track.Create(TrackIds.M6_4, "Final Exorcism"),

        // Seiðr — Mors.Void.Discipline
        Track.Create(TrackIds.M7_1, "The Seeing"),
        Track.Create(TrackIds.M7_2, "Völva's Vision"),
        Track.Create(TrackIds.M7_3, "Shamanic Trance"),
        Track.Create(TrackIds.M7_4, "Runic Gnosis"),

        // The Call — Mors.Void.Discipline
        Track.Create(TrackIds.M8_1, "Answered by Darkness"),
        Track.Create(TrackIds.M8_2, "The Calling"),
        Track.Create(TrackIds.M8_3, "Summoned"),
        Track.Create(TrackIds.M8_4, "The Response"),

        // The Mother of Darkness — Mors.Void.Discipline
        Track.Create(TrackIds.M9_1, "Dark Womb"),
        Track.Create(TrackIds.M9_2, "The Black Mother"),
        Track.Create(TrackIds.M9_3, "Void Birthright"),
        Track.Create(TrackIds.M9_4, "Dark Genesis"),

        // The Third Antichrist — Mors.Void.Discipline
        Track.Create(TrackIds.M10_1, "The Beast Reborn"),
        Track.Create(TrackIds.M10_2, "False Prophet"),
        Track.Create(TrackIds.M10_3, "Mark of the Third"),
        Track.Create(TrackIds.M10_4, "Antichrist Ascending"),

        // Womb of Lilithu — Mors.Void.Discipline
        Track.Create(TrackIds.M11_1, "Lilith Awakens"),
        Track.Create(TrackIds.M11_2, "The Dark Womb"),
        Track.Create(TrackIds.M11_3, "Daughters of Night"),
        Track.Create(TrackIds.M11_4, "Lilithu's Embrace"),

        // Bloodhymns — Vider
        Track.Create(TrackIds.V1_1, "Hymn to the Blood"),
        Track.Create(TrackIds.V1_2, "Sanguine Rites"),
        Track.Create(TrackIds.V1_3, "The Red Liturgy"),
        Track.Create(TrackIds.V1_4, "Bloodsoaked Altar"),

        // Darkside — Vider
        Track.Create(TrackIds.V2_1, "Into the Dark"),
        Track.Create(TrackIds.V2_2, "Hidden Face"),
        Track.Create(TrackIds.V2_3, "Lunar Eclipse"),
        Track.Create(TrackIds.V2_4, "The Unseen"),

        // Dawn of the Damned — Vider
        Track.Create(TrackIds.V3_1, "First Light of Doom"),
        Track.Create(TrackIds.V3_2, "The Damned Rise"),
        Track.Create(TrackIds.V3_3, "Black Dawn"),
        Track.Create(TrackIds.V3_4, "Eternal Night"),

        // Death to All — Vider
        Track.Create(TrackIds.V4_1, "Total Annihilation"),
        Track.Create(TrackIds.V4_2, "The Final War"),
        Track.Create(TrackIds.V4_3, "Void Conquest"),
        Track.Create(TrackIds.V4_4, "All Must Fall"),

        // In the Twilight Grey — Vider
        Track.Create(TrackIds.V5_1, "Between Light and Dark"),
        Track.Create(TrackIds.V5_2, "Grey Dusk"),
        Track.Create(TrackIds.V5_3, "Twilight Rites"),
        Track.Create(TrackIds.V5_4, "The Liminal Zone"),

        // Mark of the Necrogram — Vider
        Track.Create(TrackIds.V6_1, "Cursed Script"),
        Track.Create(TrackIds.V6_2, "The Dark Writing"),
        Track.Create(TrackIds.V6_3, "Branded by Death"),
        Track.Create(TrackIds.V6_4, "The Necrogram"),

        // The Nocturnal Silence — Vider
        Track.Create(TrackIds.V7_1, "Silence of the Night"),
        Track.Create(TrackIds.V7_2, "Nocturnal Rites"),
        Track.Create(TrackIds.V7_3, "After Midnight"),
        Track.Create(TrackIds.V7_4, "Deafening Quiet"),
    ];

    public static IReadOnlyList<Band> Bands
    {
        get
        {
            var darkthrone = Band.Create(
                "Darkthrone",
                "Norwegian black metal band formed in 1986 in Kolbotn. One of the seminal acts of the second wave of black metal, known for their raw, lo-fi production and prolific output spanning over three decades.",
                "Vider_dvzm6c",
                BandActivity.Of(1986, null),
                BandStatus.Active,
                BandIds.Darkthrone);
            darkthrone.AddGenre(GenreIds.BlackMetal,    isPrimary: true);
            darkthrone.AddGenre(GenreIds.RawBlackMetal, isPrimary: false);
            darkthrone.AddCountry(CountryIds.Norway);

            var burzum = Band.Create(
                "Burzum",
                "Solo black metal project by Varg Vikernes, founded in Bergen, Norway in 1991. One of the most influential and controversial acts in black metal history, pioneering the atmospheric and ambient black metal style.",
                "Deathwinds_bee5xe",
                BandActivity.Of(1991, null),
                BandStatus.OnHold,
                BandIds.Burzum);
            burzum.AddGenre(GenreIds.BlackMetal,           isPrimary: true);
            burzum.AddGenre(GenreIds.AtmosphericBlackMetal, isPrimary: false);
            burzum.AddCountry(CountryIds.Norway);

            var emperor = Band.Create(
                "Emperor",
                "Norwegian black metal band formed in 1991 in Notodden. Pioneers of symphonic black metal, renowned for their complex compositions that merged aggressive black metal with orchestral arrangements and progressive structures.",
                "Blood_Coven_dsbu7i",
                BandActivity.Of(1991, 2001),
                BandStatus.SplitUp,
                BandIds.Emperor);
            emperor.AddGenre(GenreIds.BlackMetal,         isPrimary: true);
            emperor.AddGenre(GenreIds.SymphonicBlackMetal, isPrimary: false);
            emperor.AddCountry(CountryIds.Norway);

            var mayhem = Band.Create(
                "Mayhem",
                "Norwegian black metal band founded in 1984 in Oslo. One of the founders and most notorious acts of the Norwegian black metal scene, whose turbulent history and extreme ideology left an indelible mark on extreme metal.",
                "Abuser_ua465w",
                BandActivity.Of(1984, null),
                BandStatus.Active,
                BandIds.Mayhem);
            mayhem.AddGenre(GenreIds.BlackMetal, isPrimary: true);
            mayhem.AddCountry(CountryIds.Norway);

            var dissection = Band.Create(
                "Dissection",
                "Swedish melodic black/death metal band formed in 1989 in Strömstad by Jon Nödtveidt. Known for their sophisticated blend of melodic death metal with black metal atmospherics, considered pioneers of the Scandinavian melodic extreme metal sound.",
                "Mors.Void.Discipline_to2isd",
                BandActivity.Of(1989, 2006),
                BandStatus.SplitUp,
                BandIds.Dissection);
            dissection.AddGenre(GenreIds.BlackMetal,        isPrimary: true);
            dissection.AddGenre(GenreIds.MelodicBlackMetal, isPrimary: false);
            dissection.AddCountry(CountryIds.Sweden);

            var behemoth = Band.Create(
                "Behemoth",
                "Polish extreme metal band formed in 1991 in Gdańsk by Nergal. Evolved from their early black metal roots into one of the world's foremost blackened death metal acts, known for their intense live performances and provocative imagery.",
                "Vider_dvzm6c",
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
                "Bound_By_Spells_pwiu6e",
                LabelIds.Peaceville,
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
                "Desolate_Divine_yu1kin",
                LabelIds.Misanthropy,
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
                "Conduit_pe9plu",
                LabelIds.Candlelight,
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
                "His_Best_Deceit_divtip",
                LabelIds.DeathlikeSilence,
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
                "Psychic_Secretions_c0bo9q",
                LabelIds.NuclearBlast,
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
                "Bound_By_Spells_pwiu6e",
                LabelIds.NuclearBlast,
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
            a1585.AddTrack(TrackIds.A1_1, 1); a1585.AddTrack(TrackIds.A1_2, 2); a1585.AddTrack(TrackIds.A1_3, 3); a1585.AddTrack(TrackIds.A1_4, 4);

            var aGreatWork = Album.Create("A Great Work of Ages", AlbumType.FullLength, AlbumRelease.Of(2019, AlbumFormat.CD), "His_Best_Deceit_divtip", null, AlbumIds.Abuser_GreatWork);
            aGreatWork.AddBand(BandIds.Abuser); aGreatWork.AddGenre(GenreIds.BlackMetal, isPrimary: true); aGreatWork.AddCountry(CountryIds.UnitedStates);
            aGreatWork.AddTrack(TrackIds.A2_1, 1); aGreatWork.AddTrack(TrackIds.A2_2, 2); aGreatWork.AddTrack(TrackIds.A2_3, 3); aGreatWork.AddTrack(TrackIds.A2_4, 4);

            var aMerging = Album.Create("A Merging to the Boundless", AlbumType.FullLength, AlbumRelease.Of(2013, AlbumFormat.CD), "Desolate_Divine_yu1kin", null, AlbumIds.Abuser_Merging);
            aMerging.AddBand(BandIds.Abuser); aMerging.AddGenre(GenreIds.BlackMetal, isPrimary: true); aMerging.AddCountry(CountryIds.UnitedStates);
            aMerging.AddTrack(TrackIds.A3_1, 1); aMerging.AddTrack(TrackIds.A3_2, 2); aMerging.AddTrack(TrackIds.A3_3, 3); aMerging.AddTrack(TrackIds.A3_4, 4);

            var anneliese = Album.Create("Anneliese", AlbumType.FullLength, AlbumRelease.Of(2011, AlbumFormat.CD), "Conduit_pe9plu", null, AlbumIds.Abuser_Anneliese);
            anneliese.AddBand(BandIds.Abuser); anneliese.AddGenre(GenreIds.BlackMetal, isPrimary: true); anneliese.AddCountry(CountryIds.UnitedStates);
            anneliese.AddTrack(TrackIds.A4_1, 1); anneliese.AddTrack(TrackIds.A4_2, 2); anneliese.AddTrack(TrackIds.A4_3, 3); anneliese.AddTrack(TrackIds.A4_4, 4);

            var boundBySpells = Album.Create("Bound by Spells", AlbumType.FullLength, AlbumRelease.Of(2016, AlbumFormat.CD), "Bound_By_Spells_pwiu6e", null, AlbumIds.Abuser_BoundBySpells);
            boundBySpells.AddBand(BandIds.Abuser); boundBySpells.AddGenre(GenreIds.BlackMetal, isPrimary: true); boundBySpells.AddCountry(CountryIds.UnitedStates);
            boundBySpells.AddTrack(TrackIds.A5_1, 1); boundBySpells.AddTrack(TrackIds.A5_2, 2); boundBySpells.AddTrack(TrackIds.A5_3, 3); boundBySpells.AddTrack(TrackIds.A5_4, 4);

            var conduit = Album.Create("Conduit", AlbumType.FullLength, AlbumRelease.Of(2021, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.Abuser_Conduit);
            conduit.AddBand(BandIds.Abuser); conduit.AddGenre(GenreIds.BlackMetal, isPrimary: true); conduit.AddCountry(CountryIds.UnitedStates);
            conduit.AddTrack(TrackIds.A6_1, 1); conduit.AddTrack(TrackIds.A6_2, 2); conduit.AddTrack(TrackIds.A6_3, 3); conduit.AddTrack(TrackIds.A6_4, 4);

            var desolateDivine = Album.Create("Desolate Divine", AlbumType.FullLength, AlbumRelease.Of(2018, AlbumFormat.CD), "His_Best_Deceit_divtip", null, AlbumIds.Abuser_DesolateDivine);
            desolateDivine.AddBand(BandIds.Abuser); desolateDivine.AddGenre(GenreIds.BlackMetal, isPrimary: true); desolateDivine.AddCountry(CountryIds.UnitedStates);
            desolateDivine.AddTrack(TrackIds.A7_1, 1); desolateDivine.AddTrack(TrackIds.A7_2, 2); desolateDivine.AddTrack(TrackIds.A7_3, 3); desolateDivine.AddTrack(TrackIds.A7_4, 4);

            var hisBestDeceit = Album.Create("His Best Deceit", AlbumType.FullLength, AlbumRelease.Of(2007, AlbumFormat.CD), "Desolate_Divine_yu1kin", null, AlbumIds.Abuser_HisBestDeceit);
            hisBestDeceit.AddBand(BandIds.Abuser); hisBestDeceit.AddGenre(GenreIds.BlackMetal, isPrimary: true); hisBestDeceit.AddCountry(CountryIds.UnitedStates);
            hisBestDeceit.AddTrack(TrackIds.A8_1, 1); hisBestDeceit.AddTrack(TrackIds.A8_2, 2); hisBestDeceit.AddTrack(TrackIds.A8_3, 3); hisBestDeceit.AddTrack(TrackIds.A8_4, 4);

            var psychicSecretions = Album.Create("Psychic Secretions", AlbumType.FullLength, AlbumRelease.Of(2022, AlbumFormat.CD), "Conduit_pe9plu", null, AlbumIds.Abuser_PsychicSecretions);
            psychicSecretions.AddBand(BandIds.Abuser); psychicSecretions.AddGenre(GenreIds.BlackMetal, isPrimary: true); psychicSecretions.AddCountry(CountryIds.UnitedStates);
            psychicSecretions.AddTrack(TrackIds.A9_1, 1); psychicSecretions.AddTrack(TrackIds.A9_2, 2); psychicSecretions.AddTrack(TrackIds.A9_3, 3); psychicSecretions.AddTrack(TrackIds.A9_4, 4);

            // ── Blood Coven ───────────────────────────────────────────────────────
            var caughtInUnlight = Album.Create("Caught in the Unlight!", AlbumType.FullLength, AlbumRelease.Of(2003, AlbumFormat.CD), "Bound_By_Spells_pwiu6e", null, AlbumIds.BC_CaughtInTheUnlight);
            caughtInUnlight.AddBand(BandIds.BloodCoven); caughtInUnlight.AddGenre(GenreIds.BlackMetal, isPrimary: true); caughtInUnlight.AddCountry(CountryIds.UnitedStates);
            caughtInUnlight.AddTrack(TrackIds.BC1_1, 1); caughtInUnlight.AddTrack(TrackIds.BC1_2, 2); caughtInUnlight.AddTrack(TrackIds.BC1_3, 3); caughtInUnlight.AddTrack(TrackIds.BC1_4, 4);

            var forTheHolyLord = Album.Create("For the Holy Lord", AlbumType.FullLength, AlbumRelease.Of(2007, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.BC_ForTheHolyLord);
            forTheHolyLord.AddBand(BandIds.BloodCoven); forTheHolyLord.AddGenre(GenreIds.BlackMetal, isPrimary: true); forTheHolyLord.AddCountry(CountryIds.UnitedStates);
            forTheHolyLord.AddTrack(TrackIds.BC2_1, 1); forTheHolyLord.AddTrack(TrackIds.BC2_2, 2); forTheHolyLord.AddTrack(TrackIds.BC2_3, 3); forTheHolyLord.AddTrack(TrackIds.BC2_4, 4);

            var continuumHypothesis = Album.Create("The Continuum Hypothesis", AlbumType.FullLength, AlbumRelease.Of(2012, AlbumFormat.CD), "His_Best_Deceit_divtip", null, AlbumIds.BC_ContinuumHypothesis);
            continuumHypothesis.AddBand(BandIds.BloodCoven); continuumHypothesis.AddGenre(GenreIds.BlackMetal, isPrimary: true); continuumHypothesis.AddCountry(CountryIds.UnitedStates);
            continuumHypothesis.AddTrack(TrackIds.BC3_1, 1); continuumHypothesis.AddTrack(TrackIds.BC3_2, 2); continuumHypothesis.AddTrack(TrackIds.BC3_3, 3); continuumHypothesis.AddTrack(TrackIds.BC3_4, 4);

            var molecularScythe = Album.Create("The Molecular Scythe", AlbumType.FullLength, AlbumRelease.Of(2015, AlbumFormat.CD), "Desolate_Divine_yu1kin", null, AlbumIds.BC_MolecularScythe);
            molecularScythe.AddBand(BandIds.BloodCoven); molecularScythe.AddGenre(GenreIds.BlackMetal, isPrimary: true); molecularScythe.AddCountry(CountryIds.UnitedStates);
            molecularScythe.AddTrack(TrackIds.BC4_1, 1); molecularScythe.AddTrack(TrackIds.BC4_2, 2); molecularScythe.AddTrack(TrackIds.BC4_3, 3); molecularScythe.AddTrack(TrackIds.BC4_4, 4);

            var screamToreSky = Album.Create("The Scream That Tore the Sky", AlbumType.FullLength, AlbumRelease.Of(2009, AlbumFormat.CD), "Conduit_pe9plu", null, AlbumIds.BC_ScreamToreSky);
            screamToreSky.AddBand(BandIds.BloodCoven); screamToreSky.AddGenre(GenreIds.BlackMetal, isPrimary: true); screamToreSky.AddCountry(CountryIds.UnitedStates);
            screamToreSky.AddTrack(TrackIds.BC5_1, 1); screamToreSky.AddTrack(TrackIds.BC5_2, 2); screamToreSky.AddTrack(TrackIds.BC5_3, 3); screamToreSky.AddTrack(TrackIds.BC5_4, 4);

            var whatWillBe = Album.Create("What Will Be Has Been", AlbumType.FullLength, AlbumRelease.Of(2018, AlbumFormat.CD), "Bound_By_Spells_pwiu6e", null, AlbumIds.BC_WhatWillBe);
            whatWillBe.AddBand(BandIds.BloodCoven); whatWillBe.AddGenre(GenreIds.BlackMetal, isPrimary: true); whatWillBe.AddCountry(CountryIds.UnitedStates);
            whatWillBe.AddTrack(TrackIds.BC6_1, 1); whatWillBe.AddTrack(TrackIds.BC6_2, 2); whatWillBe.AddTrack(TrackIds.BC6_3, 3); whatWillBe.AddTrack(TrackIds.BC6_4, 4);

            // ── Deathwinds ────────────────────────────────────────────────────────
            var auricGates = Album.Create("Auric Gates of Veles", AlbumType.FullLength, AlbumRelease.Of(2015, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.DW_AuricGates);
            auricGates.AddBand(BandIds.Deathwinds); auricGates.AddGenre(GenreIds.BlackMetal, isPrimary: true); auricGates.AddCountry(CountryIds.Poland);
            auricGates.AddTrack(TrackIds.DW1_1, 1); auricGates.AddTrack(TrackIds.DW1_2, 2); auricGates.AddTrack(TrackIds.DW1_3, 3); auricGates.AddTrack(TrackIds.DW1_4, 4);

            var bellumRegiis = Album.Create("Bellum Regiis", AlbumType.FullLength, AlbumRelease.Of(2012, AlbumFormat.CD), "His_Best_Deceit_divtip", null, AlbumIds.DW_BellumRegiis);
            bellumRegiis.AddBand(BandIds.Deathwinds); bellumRegiis.AddGenre(GenreIds.BlackMetal, isPrimary: true); bellumRegiis.AddCountry(CountryIds.Poland);
            bellumRegiis.AddTrack(TrackIds.DW2_1, 1); bellumRegiis.AddTrack(TrackIds.DW2_2, 2); bellumRegiis.AddTrack(TrackIds.DW2_3, 3); bellumRegiis.AddTrack(TrackIds.DW2_4, 4);

            var crvsade = Album.Create("Crvsade - Zero", AlbumType.FullLength, AlbumRelease.Of(2017, AlbumFormat.CD), "Desolate_Divine_yu1kin", null, AlbumIds.DW_Crvsade);
            crvsade.AddBand(BandIds.Deathwinds); crvsade.AddGenre(GenreIds.BlackMetal, isPrimary: true); crvsade.AddCountry(CountryIds.Poland);
            crvsade.AddTrack(TrackIds.DW3_1, 1); crvsade.AddTrack(TrackIds.DW3_2, 2); crvsade.AddTrack(TrackIds.DW3_3, 3); crvsade.AddTrack(TrackIds.DW3_4, 4);

            var erebos = Album.Create("Erebos", AlbumType.FullLength, AlbumRelease.Of(2008, AlbumFormat.CD), "Conduit_pe9plu", null, AlbumIds.DW_Erebos);
            erebos.AddBand(BandIds.Deathwinds); erebos.AddGenre(GenreIds.BlackMetal, isPrimary: true); erebos.AddCountry(CountryIds.Poland);
            erebos.AddTrack(TrackIds.DW4_1, 1); erebos.AddTrack(TrackIds.DW4_2, 2); erebos.AddTrack(TrackIds.DW4_3, 3); erebos.AddTrack(TrackIds.DW4_4, 4);

            var misanthropicPath = Album.Create("Misanthropic Path of Carnal Deliverance", AlbumType.FullLength, AlbumRelease.Of(2006, AlbumFormat.CD), "Bound_By_Spells_pwiu6e", null, AlbumIds.DW_Misanthropic);
            misanthropicPath.AddBand(BandIds.Deathwinds); misanthropicPath.AddGenre(GenreIds.BlackMetal, isPrimary: true); misanthropicPath.AddCountry(CountryIds.Poland);
            misanthropicPath.AddTrack(TrackIds.DW5_1, 1); misanthropicPath.AddTrack(TrackIds.DW5_2, 2); misanthropicPath.AddTrack(TrackIds.DW5_3, 3); misanthropicPath.AddTrack(TrackIds.DW5_4, 4);

            var rugia = Album.Create("Rugia", AlbumType.FullLength, AlbumRelease.Of(2019, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.DW_Rugia);
            rugia.AddBand(BandIds.Deathwinds); rugia.AddGenre(GenreIds.BlackMetal, isPrimary: true); rugia.AddCountry(CountryIds.Poland);
            rugia.AddTrack(TrackIds.DW6_1, 1); rugia.AddTrack(TrackIds.DW6_2, 2); rugia.AddTrack(TrackIds.DW6_3, 3); rugia.AddTrack(TrackIds.DW6_4, 4);

            var solarflesh = Album.Create("Solarflesh", AlbumType.FullLength, AlbumRelease.Of(2013, AlbumFormat.CD), "His_Best_Deceit_divtip", null, AlbumIds.DW_Solarflesh);
            solarflesh.AddBand(BandIds.Deathwinds); solarflesh.AddGenre(GenreIds.BlackMetal, isPrimary: true); solarflesh.AddCountry(CountryIds.Poland);
            solarflesh.AddTrack(TrackIds.DW7_1, 1); solarflesh.AddTrack(TrackIds.DW7_2, 2); solarflesh.AddTrack(TrackIds.DW7_3, 3); solarflesh.AddTrack(TrackIds.DW7_4, 4);

            var tremendum = Album.Create("Tremendum", AlbumType.FullLength, AlbumRelease.Of(2010, AlbumFormat.CD), "Desolate_Divine_yu1kin", null, AlbumIds.DW_Tremendum);
            tremendum.AddBand(BandIds.Deathwinds); tremendum.AddGenre(GenreIds.BlackMetal, isPrimary: true); tremendum.AddCountry(CountryIds.Poland);
            tremendum.AddTrack(TrackIds.DW8_1, 1); tremendum.AddTrack(TrackIds.DW8_2, 2); tremendum.AddTrack(TrackIds.DW8_3, 3); tremendum.AddTrack(TrackIds.DW8_4, 4);

            // ── Mors.Void.Discipline ──────────────────────────────────────────────
            var exorkizein = Album.Create("Exorkizein", AlbumType.FullLength, AlbumRelease.Of(2013, AlbumFormat.CD), "Conduit_pe9plu", null, AlbumIds.MVD_Exorkizein);
            exorkizein.AddBand(BandIds.MorsVoidDiscipline); exorkizein.AddGenre(GenreIds.BlackMetal, isPrimary: true); exorkizein.AddCountry(CountryIds.France);
            exorkizein.AddTrack(TrackIds.M1_1, 1); exorkizein.AddTrack(TrackIds.M1_2, 2); exorkizein.AddTrack(TrackIds.M1_3, 3); exorkizein.AddTrack(TrackIds.M1_4, 4);

            var malignancy = Album.Create("Malignancy", AlbumType.FullLength, AlbumRelease.Of(2006, AlbumFormat.CD), "Bound_By_Spells_pwiu6e", null, AlbumIds.MVD_Malignancy);
            malignancy.AddBand(BandIds.MorsVoidDiscipline); malignancy.AddGenre(GenreIds.BlackMetal, isPrimary: true); malignancy.AddCountry(CountryIds.France);
            malignancy.AddTrack(TrackIds.M2_1, 1); malignancy.AddTrack(TrackIds.M2_2, 2); malignancy.AddTrack(TrackIds.M2_3, 3); malignancy.AddTrack(TrackIds.M2_4, 4);

            var onslaughtKommand = Album.Create("Onslaught Kommand", AlbumType.FullLength, AlbumRelease.Of(2009, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.MVD_OnslaughtKommand);
            onslaughtKommand.AddBand(BandIds.MorsVoidDiscipline); onslaughtKommand.AddGenre(GenreIds.BlackMetal, isPrimary: true); onslaughtKommand.AddCountry(CountryIds.France);
            onslaughtKommand.AddTrack(TrackIds.M3_1, 1); onslaughtKommand.AddTrack(TrackIds.M3_2, 2); onslaughtKommand.AddTrack(TrackIds.M3_3, 3); onslaughtKommand.AddTrack(TrackIds.M3_4, 4);

            var passioChristiI = Album.Create("Passio Christi Part I", AlbumType.FullLength, AlbumRelease.Of(2015, AlbumFormat.CD), "His_Best_Deceit_divtip", null, AlbumIds.MVD_PassioChristiI);
            passioChristiI.AddBand(BandIds.MorsVoidDiscipline); passioChristiI.AddGenre(GenreIds.BlackMetal, isPrimary: true); passioChristiI.AddCountry(CountryIds.France);
            passioChristiI.AddTrack(TrackIds.M4_1, 1); passioChristiI.AddTrack(TrackIds.M4_2, 2); passioChristiI.AddTrack(TrackIds.M4_3, 3); passioChristiI.AddTrack(TrackIds.M4_4, 4);

            var passioChristiII = Album.Create("Passio Christi Part II", AlbumType.FullLength, AlbumRelease.Of(2016, AlbumFormat.CD), "Desolate_Divine_yu1kin", null, AlbumIds.MVD_PassioChristiII);
            passioChristiII.AddBand(BandIds.MorsVoidDiscipline); passioChristiII.AddGenre(GenreIds.BlackMetal, isPrimary: true); passioChristiII.AddCountry(CountryIds.France);
            passioChristiII.AddTrack(TrackIds.M5_1, 1); passioChristiII.AddTrack(TrackIds.M5_2, 2); passioChristiII.AddTrack(TrackIds.M5_3, 3); passioChristiII.AddTrack(TrackIds.M5_4, 4);

            var possession = Album.Create("Possession", AlbumType.FullLength, AlbumRelease.Of(2004, AlbumFormat.CD), "Conduit_pe9plu", null, AlbumIds.MVD_Possession);
            possession.AddBand(BandIds.MorsVoidDiscipline); possession.AddGenre(GenreIds.BlackMetal, isPrimary: true); possession.AddCountry(CountryIds.France);
            possession.AddTrack(TrackIds.M6_1, 1); possession.AddTrack(TrackIds.M6_2, 2); possession.AddTrack(TrackIds.M6_3, 3); possession.AddTrack(TrackIds.M6_4, 4);

            var seidr = Album.Create("Seiðr", AlbumType.FullLength, AlbumRelease.Of(2011, AlbumFormat.CD), "Bound_By_Spells_pwiu6e", null, AlbumIds.MVD_Seidr);
            seidr.AddBand(BandIds.MorsVoidDiscipline); seidr.AddGenre(GenreIds.BlackMetal, isPrimary: true); seidr.AddCountry(CountryIds.France);
            seidr.AddTrack(TrackIds.M7_1, 1); seidr.AddTrack(TrackIds.M7_2, 2); seidr.AddTrack(TrackIds.M7_3, 3); seidr.AddTrack(TrackIds.M7_4, 4);

            var theCall = Album.Create("The Call", AlbumType.FullLength, AlbumRelease.Of(2018, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.MVD_TheCall);
            theCall.AddBand(BandIds.MorsVoidDiscipline); theCall.AddGenre(GenreIds.BlackMetal, isPrimary: true); theCall.AddCountry(CountryIds.France);
            theCall.AddTrack(TrackIds.M8_1, 1); theCall.AddTrack(TrackIds.M8_2, 2); theCall.AddTrack(TrackIds.M8_3, 3); theCall.AddTrack(TrackIds.M8_4, 4);

            var motherOfDarkness = Album.Create("The Mother of Darkness", AlbumType.FullLength, AlbumRelease.Of(2008, AlbumFormat.CD), "His_Best_Deceit_divtip", null, AlbumIds.MVD_MotherOfDarkness);
            motherOfDarkness.AddBand(BandIds.MorsVoidDiscipline); motherOfDarkness.AddGenre(GenreIds.BlackMetal, isPrimary: true); motherOfDarkness.AddCountry(CountryIds.France);
            motherOfDarkness.AddTrack(TrackIds.M9_1, 1); motherOfDarkness.AddTrack(TrackIds.M9_2, 2); motherOfDarkness.AddTrack(TrackIds.M9_3, 3); motherOfDarkness.AddTrack(TrackIds.M9_4, 4);

            var thirdAntichrist = Album.Create("The Third Antichrist", AlbumType.FullLength, AlbumRelease.Of(2020, AlbumFormat.CD), "Desolate_Divine_yu1kin", null, AlbumIds.MVD_ThirdAntichrist);
            thirdAntichrist.AddBand(BandIds.MorsVoidDiscipline); thirdAntichrist.AddGenre(GenreIds.BlackMetal, isPrimary: true); thirdAntichrist.AddCountry(CountryIds.France);
            thirdAntichrist.AddTrack(TrackIds.M10_1, 1); thirdAntichrist.AddTrack(TrackIds.M10_2, 2); thirdAntichrist.AddTrack(TrackIds.M10_3, 3); thirdAntichrist.AddTrack(TrackIds.M10_4, 4);

            var wombOfLilithu = Album.Create("Womb of Lilithu", AlbumType.FullLength, AlbumRelease.Of(2002, AlbumFormat.CD), "Conduit_pe9plu", null, AlbumIds.MVD_WombOfLilithu);
            wombOfLilithu.AddBand(BandIds.MorsVoidDiscipline); wombOfLilithu.AddGenre(GenreIds.BlackMetal, isPrimary: true); wombOfLilithu.AddCountry(CountryIds.France);
            wombOfLilithu.AddTrack(TrackIds.M11_1, 1); wombOfLilithu.AddTrack(TrackIds.M11_2, 2); wombOfLilithu.AddTrack(TrackIds.M11_3, 3); wombOfLilithu.AddTrack(TrackIds.M11_4, 4);

            // ── Vider ─────────────────────────────────────────────────────────────
            var bloodhymns = Album.Create("Bloodhymns", AlbumType.FullLength, AlbumRelease.Of(2002, AlbumFormat.CD), "Bound_By_Spells_pwiu6e", null, AlbumIds.Vider_Bloodhymns);
            bloodhymns.AddBand(BandIds.Vider); bloodhymns.AddGenre(GenreIds.BlackMetal, isPrimary: true); bloodhymns.AddCountry(CountryIds.Sweden);
            bloodhymns.AddTrack(TrackIds.V1_1, 1); bloodhymns.AddTrack(TrackIds.V1_2, 2); bloodhymns.AddTrack(TrackIds.V1_3, 3); bloodhymns.AddTrack(TrackIds.V1_4, 4);

            var darkside = Album.Create("Darkside", AlbumType.FullLength, AlbumRelease.Of(1997, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.Vider_Darkside);
            darkside.AddBand(BandIds.Vider); darkside.AddGenre(GenreIds.BlackMetal, isPrimary: true); darkside.AddCountry(CountryIds.Sweden);
            darkside.AddTrack(TrackIds.V2_1, 1); darkside.AddTrack(TrackIds.V2_2, 2); darkside.AddTrack(TrackIds.V2_3, 3); darkside.AddTrack(TrackIds.V2_4, 4);

            var dawnOfDamned = Album.Create("Dawn of the Damned", AlbumType.FullLength, AlbumRelease.Of(2004, AlbumFormat.CD), "His_Best_Deceit_divtip", null, AlbumIds.Vider_DawnOfDamned);
            dawnOfDamned.AddBand(BandIds.Vider); dawnOfDamned.AddGenre(GenreIds.BlackMetal, isPrimary: true); dawnOfDamned.AddCountry(CountryIds.Sweden);
            dawnOfDamned.AddTrack(TrackIds.V3_1, 1); dawnOfDamned.AddTrack(TrackIds.V3_2, 2); dawnOfDamned.AddTrack(TrackIds.V3_3, 3); dawnOfDamned.AddTrack(TrackIds.V3_4, 4);

            var deathToAll = Album.Create("Death to All", AlbumType.FullLength, AlbumRelease.Of(2012, AlbumFormat.CD), "Desolate_Divine_yu1kin", null, AlbumIds.Vider_DeathToAll);
            deathToAll.AddBand(BandIds.Vider); deathToAll.AddGenre(GenreIds.BlackMetal, isPrimary: true); deathToAll.AddCountry(CountryIds.Sweden);
            deathToAll.AddTrack(TrackIds.V4_1, 1); deathToAll.AddTrack(TrackIds.V4_2, 2); deathToAll.AddTrack(TrackIds.V4_3, 3); deathToAll.AddTrack(TrackIds.V4_4, 4);

            var inTheTwilightGrey = Album.Create("In the Twilight Grey", AlbumType.FullLength, AlbumRelease.Of(1999, AlbumFormat.CD), "Conduit_pe9plu", null, AlbumIds.Vider_InTheTwilightGrey);
            inTheTwilightGrey.AddBand(BandIds.Vider); inTheTwilightGrey.AddGenre(GenreIds.BlackMetal, isPrimary: true); inTheTwilightGrey.AddCountry(CountryIds.Sweden);
            inTheTwilightGrey.AddTrack(TrackIds.V5_1, 1); inTheTwilightGrey.AddTrack(TrackIds.V5_2, 2); inTheTwilightGrey.AddTrack(TrackIds.V5_3, 3); inTheTwilightGrey.AddTrack(TrackIds.V5_4, 4);

            var markOfNecrogram = Album.Create("Mark of the Necrogram", AlbumType.FullLength, AlbumRelease.Of(2018, AlbumFormat.CD), "Bound_By_Spells_pwiu6e", null, AlbumIds.Vider_MarkOfNecrogram);
            markOfNecrogram.AddBand(BandIds.Vider); markOfNecrogram.AddGenre(GenreIds.BlackMetal, isPrimary: true); markOfNecrogram.AddCountry(CountryIds.Sweden);
            markOfNecrogram.AddTrack(TrackIds.V6_1, 1); markOfNecrogram.AddTrack(TrackIds.V6_2, 2); markOfNecrogram.AddTrack(TrackIds.V6_3, 3); markOfNecrogram.AddTrack(TrackIds.V6_4, 4);

            var nocturnalSilence = Album.Create("The Nocturnal Silence", AlbumType.FullLength, AlbumRelease.Of(1994, AlbumFormat.CD), "Psychic_Secretions_c0bo9q", null, AlbumIds.Vider_NocturnalSilence);
            nocturnalSilence.AddBand(BandIds.Vider); nocturnalSilence.AddGenre(GenreIds.BlackMetal, isPrimary: true); nocturnalSilence.AddCountry(CountryIds.Sweden);
            nocturnalSilence.AddTrack(TrackIds.V7_1, 1); nocturnalSilence.AddTrack(TrackIds.V7_2, 2); nocturnalSilence.AddTrack(TrackIds.V7_3, 3); nocturnalSilence.AddTrack(TrackIds.V7_4, 4);

            return [transilvanianHunger, filosofem, nightsideEclipse, deMysteriis, stormOfLightsBane, theSatanist,
                    a1585, aGreatWork, aMerging, anneliese, boundBySpells, conduit, desolateDivine, hisBestDeceit, psychicSecretions,
                    caughtInUnlight, forTheHolyLord, continuumHypothesis, molecularScythe, screamToreSky, whatWillBe,
                    auricGates, bellumRegiis, crvsade, erebos, misanthropicPath, rugia, solarflesh, tremendum,
                    exorkizein, malignancy, onslaughtKommand, passioChristiI, passioChristiII, possession, seidr, theCall, motherOfDarkness, thirdAntichrist, wombOfLilithu,
                    bloodhymns, darkside, dawnOfDamned, deathToAll, inTheTwilightGrey, markOfNecrogram, nocturnalSilence];
        }
    }
}
