using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAlbumAndBandRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "average_rating",
                table: "bands",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ratings_count",
                table: "bands",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "average_rating",
                table: "albums",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ratings_count",
                table: "albums",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "album_ratings",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    album_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    rated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album_ratings", x => new { x.user_id, x.album_id });
                    table.ForeignKey(
                        name: "FK_album_ratings_albums_album_id",
                        column: x => x.album_id,
                        principalTable: "albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "band_ratings",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    band_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    rated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_band_ratings", x => new { x.user_id, x.band_id });
                    table.ForeignKey(
                        name: "FK_band_ratings_bands_band_id",
                        column: x => x.band_id,
                        principalTable: "bands",
                        principalColumn: "Id",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "album_ratings");

            migrationBuilder.DropTable(
                name: "band_ratings");

            migrationBuilder.DropColumn(
                name: "average_rating",
                table: "bands");

            migrationBuilder.DropColumn(
                name: "ratings_count",
                table: "bands");

            migrationBuilder.DropColumn(
                name: "average_rating",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "ratings_count",
                table: "albums");
        }
    }
}
