using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserContent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MergeRatingsIntoReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "album_ratings");

            migrationBuilder.DropTable(
                name: "band_ratings");

            migrationBuilder.AddColumn<DateTime>(
                name: "rated_at",
                table: "band_reviews",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "rating",
                table: "band_reviews",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "rated_at",
                table: "album_reviews",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "rating",
                table: "album_reviews",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rated_at",
                table: "band_reviews");

            migrationBuilder.DropColumn(
                name: "rating",
                table: "band_reviews");

            migrationBuilder.DropColumn(
                name: "rated_at",
                table: "album_reviews");

            migrationBuilder.DropColumn(
                name: "rating",
                table: "album_reviews");

            migrationBuilder.CreateTable(
                name: "album_ratings",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    album_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album_ratings", x => new { x.user_id, x.album_id });
                    table.ForeignKey(
                        name: "FK_album_ratings_albums_album_id",
                        column: x => x.album_id,
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_album_ratings_user_profiles_user_id",
                        column: x => x.user_id,
                        principalTable: "user_profiles",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "band_ratings",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    band_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_band_ratings", x => new { x.user_id, x.band_id });
                    table.ForeignKey(
                        name: "FK_band_ratings_bands_band_id",
                        column: x => x.band_id,
                        principalTable: "bands",
                        principalColumn: "band_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_band_ratings_user_profiles_user_id",
                        column: x => x.user_id,
                        principalTable: "user_profiles",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_album_ratings_album_id",
                table: "album_ratings",
                column: "album_id");

            migrationBuilder.CreateIndex(
                name: "IX_band_ratings_band_id",
                table: "band_ratings",
                column: "band_id");
        }
    }
}
