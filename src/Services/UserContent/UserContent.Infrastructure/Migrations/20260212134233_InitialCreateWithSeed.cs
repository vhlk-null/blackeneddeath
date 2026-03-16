using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserContent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "albums",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cover_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    release_date = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_albums", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bands",
                columns: table => new
                {
                    band_id = table.Column<Guid>(type: "uuid", nullable: false),
                    band_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    logo_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    release_date = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bands", x => x.band_id);
                });

            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    avatar_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    registered_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_login_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    bio = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_profiles", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "favorite_albums",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    album_id = table.Column<Guid>(type: "uuid", nullable: false),
                    added_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_rating = table.Column<int>(type: "integer", nullable: true),
                    user_review = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorite_albums", x => new { x.user_id, x.album_id });
                    table.ForeignKey(
                        name: "FK_favorite_albums_albums_album_id",
                        column: x => x.album_id,
                        principalTable: "albums",
                        principalColumn: "id",
<<<<<<<< HEAD:src/Services/UserContent/UserContent.Infrastructure/Migrations/20260212134233_InitialCreateWithSeed.cs
                        onDelete: ReferentialAction.Cascade);
========
                        onDelete: ReferentialAction.Restrict);
>>>>>>>> origin/develop:src/Services/UserContent/UserContent.Infrastructure/Migrations/20260306162031_MigrationName.cs
                    table.ForeignKey(
                        name: "FK_favorite_albums_user_profiles_user_id",
                        column: x => x.user_id,
                        principalTable: "user_profiles",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "favorite_bands",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    band_id = table.Column<Guid>(type: "uuid", nullable: false),
                    added_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_following = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorite_bands", x => new { x.user_id, x.band_id });
                    table.ForeignKey(
                        name: "FK_favorite_bands_bands_band_id",
                        column: x => x.band_id,
                        principalTable: "bands",
                        principalColumn: "band_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_favorite_bands_user_profiles_user_id",
                        column: x => x.user_id,
                        principalTable: "user_profiles",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_favorite_albums_album_id",
                table: "favorite_albums",
                column: "album_id");

            migrationBuilder.CreateIndex(
                name: "IX_favorite_bands_band_id",
                table: "favorite_bands",
                column: "band_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_profiles_email",
                table: "user_profiles",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_profiles_username",
                table: "user_profiles",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "favorite_albums");

            migrationBuilder.DropTable(
                name: "favorite_bands");

            migrationBuilder.DropTable(
                name: "albums");

            migrationBuilder.DropTable(
                name: "bands");

            migrationBuilder.DropTable(
                name: "user_profiles");
        }
    }
}
