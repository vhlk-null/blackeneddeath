using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserContent.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    album_id = table.Column<Guid>(type: "uuid", nullable: false),
                    album_title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cover_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    release_date = table.Column<int>(type: "integer", nullable: false),
                    added_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_rating = table.Column<int>(type: "integer", nullable: true),
                    user_review = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorite_albums", x => x.id);
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
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    band_id = table.Column<Guid>(type: "uuid", nullable: false),
                    band_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    logo_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    added_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReleaseDate = table.Column<int>(type: "integer", nullable: false),
                    is_following = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorite_bands", x => x.id);
                    table.ForeignKey(
                        name: "FK_favorite_bands_user_profiles_user_id",
                        column: x => x.user_id,
                        principalTable: "user_profiles",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_favorite_albums_user_id",
                table: "favorite_albums",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_favorite_albums_user_id_album_id",
                table: "favorite_albums",
                columns: new[] { "user_id", "album_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_favorite_bands_user_id",
                table: "favorite_bands",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_favorite_bands_user_id_band_id",
                table: "favorite_bands",
                columns: new[] { "user_id", "band_id" },
                unique: true);

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
                name: "user_profiles");
        }
    }
}
