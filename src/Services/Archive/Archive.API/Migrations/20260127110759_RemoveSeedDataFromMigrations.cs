using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Archive.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeedDataFromMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "albums",
                keyColumn: "id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "albums",
                keyColumn: "id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "albums",
                keyColumn: "id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "albums",
                keyColumn: "id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "albums",
                keyColumn: "id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "albums",
                keyColumn: "id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "bands",
                keyColumn: "id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "bands",
                keyColumn: "id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "bands",
                keyColumn: "id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "bands",
                keyColumn: "id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "bands",
                keyColumn: "id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "bands",
                keyColumn: "id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "countries",
                keyColumn: "id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "countries",
                keyColumn: "id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "countries",
                keyColumn: "id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "countries",
                keyColumn: "id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "countries",
                keyColumn: "id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "countries",
                keyColumn: "id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("3000000a-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "tracks",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000001"));

            migrationBuilder.DeleteData(
                table: "tracks",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000002"));

            migrationBuilder.DeleteData(
                table: "tracks",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000003"));

            migrationBuilder.DeleteData(
                table: "tracks",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000004"));

            migrationBuilder.DeleteData(
                table: "tracks",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000005"));

            migrationBuilder.DeleteData(
                table: "tracks",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000006"));

            migrationBuilder.DeleteData(
                table: "tracks",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000007"));

            migrationBuilder.DeleteData(
                table: "tracks",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000008"));

            migrationBuilder.DeleteData(
                table: "tracks",
                keyColumn: "id",
                keyValue: new Guid("00000000-0000-0000-0002-000000000009"));

            migrationBuilder.DeleteData(
                table: "tracks",
                keyColumn: "id",
                keyValue: new Guid("0000000a-0000-0000-0002-000000000000"));

            migrationBuilder.DeleteData(
                table: "countries",
                keyColumn: "id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "countries",
                keyColumn: "id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "countries",
                keyColumn: "id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "genres",
                keyColumn: "id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "albums",
                columns: new[] { "id", "cover_url", "format", "label", "release_date", "title", "type" },
                values: new object[,]
                {
                    { new Guid("a0000000-0000-0000-0000-000000000001"), null, 0, "Peaceville Records", 1994, "Transilvanian Hunger", 0 },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), null, 0, "Misanthropy Records", 1996, "Filosofem", 0 },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), null, 0, "Candlelight Records", 1994, "In the Nightside Eclipse", 0 },
                    { new Guid("a0000000-0000-0000-0000-000000000004"), null, 0, "Deathlike Silence Productions", 1994, "De Mysteriis Dom Sathanas", 0 },
                    { new Guid("a0000000-0000-0000-0000-000000000005"), null, 0, "Nuclear Blast", 1995, "Storm of the Light's Bane", 0 },
                    { new Guid("a0000000-0000-0000-0000-000000000006"), null, 0, "Nuclear Blast", 2014, "The Satanist", 0 }
                });

            migrationBuilder.InsertData(
                table: "countries",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { new Guid("c0000000-0000-0000-0000-000000000001"), "NO", "Norway" },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), "SE", "Sweden" },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), "FI", "Finland" },
                    { new Guid("c0000000-0000-0000-0000-000000000004"), "PL", "Poland" },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), "UA", "Ukraine" },
                    { new Guid("c0000000-0000-0000-0000-000000000006"), "US", "United States" },
                    { new Guid("c0000000-0000-0000-0000-000000000007"), "GB", "United Kingdom" },
                    { new Guid("c0000000-0000-0000-0000-000000000008"), "DE", "Germany" },
                    { new Guid("c0000000-0000-0000-0000-000000000009"), "FR", "France" }
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "name", "parent_genre_id" },
                values: new object[] { new Guid("10000000-0000-0000-0000-000000000001"), "Metal", null });

            migrationBuilder.InsertData(
                table: "tracks",
                columns: new[] { "id", "title" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0002-000000000001"), "Transilvanian Hunger" },
                    { new Guid("00000000-0000-0000-0002-000000000002"), "Over Fjell og Gjennom Torner" },
                    { new Guid("00000000-0000-0000-0002-000000000003"), "Skald av Satans Sol" },
                    { new Guid("00000000-0000-0000-0002-000000000004"), "Slottet i Det Fjerne" },
                    { new Guid("00000000-0000-0000-0002-000000000005"), "Dunkelheit" },
                    { new Guid("00000000-0000-0000-0002-000000000006"), "Jesus' Tod" },
                    { new Guid("00000000-0000-0000-0002-000000000007"), "Erblicket die Töchter des Firmaments" },
                    { new Guid("00000000-0000-0000-0002-000000000008"), "Into the Infinity of Thoughts" },
                    { new Guid("00000000-0000-0000-0002-000000000009"), "The Burning Shadows of Silence" },
                    { new Guid("0000000a-0000-0000-0002-000000000000"), "Cosmic Keys to My Creations & Times" }
                });

            migrationBuilder.InsertData(
                table: "bands",
                columns: new[] { "id", "bio", "country_id", "disbanded_year", "formed_year", "logo_url", "name", "status" },
                values: new object[,]
                {
                    { new Guid("b0000000-0000-0000-0000-000000000001"), "Norwegian black metal band formed in 1986.", new Guid("c0000000-0000-0000-0000-000000000001"), null, 1986, null, "Darkthrone", 0 },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), "Norwegian black metal solo project by Varg Vikernes.", new Guid("c0000000-0000-0000-0000-000000000001"), null, 1991, null, "Burzum", 2 },
                    { new Guid("b0000000-0000-0000-0000-000000000003"), "Norwegian black metal band, pioneers of symphonic black metal.", new Guid("c0000000-0000-0000-0000-000000000001"), 2001, 1991, null, "Emperor", 1 },
                    { new Guid("b0000000-0000-0000-0000-000000000004"), "Norwegian black metal band, one of the genre's founders.", new Guid("c0000000-0000-0000-0000-000000000001"), null, 1984, null, "Mayhem", 0 },
                    { new Guid("b0000000-0000-0000-0000-000000000005"), "Swedish melodic black/death metal band.", new Guid("c0000000-0000-0000-0000-000000000002"), 2006, 1989, null, "Dissection", 1 },
                    { new Guid("b0000000-0000-0000-0000-000000000006"), "Polish blackened death metal band.", new Guid("c0000000-0000-0000-0000-000000000004"), null, 1991, null, "Behemoth", 0 }
                });

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "id", "name", "parent_genre_id" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), "Black Metal", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "Death Metal", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("20000000-0000-0000-0000-000000000003"), "Doom Metal", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("20000000-0000-0000-0000-000000000004"), "Thrash Metal", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000001"), "Raw Black Metal", new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000002"), "Melodic Black Metal", new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000003"), "Atmospheric Black Metal", new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000004"), "Symphonic Black Metal", new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000005"), "Depressive Black Metal", new Guid("20000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000006"), "Melodic Death Metal", new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("30000000-0000-0000-0000-000000000007"), "Technical Death Metal", new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("30000000-0000-0000-0000-000000000008"), "Brutal Death Metal", new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("30000000-0000-0000-0000-000000000009"), "Funeral Doom", new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("3000000a-0000-0000-0000-000000000000"), "Stoner Doom", new Guid("20000000-0000-0000-0000-000000000003") }
                });
        }
    }
}
