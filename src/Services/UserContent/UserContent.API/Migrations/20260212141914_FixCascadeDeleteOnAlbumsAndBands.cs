using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserContent.API.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeDeleteOnAlbumsAndBands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_favorite_albums_albums_album_id",
                table: "favorite_albums");

            migrationBuilder.DropForeignKey(
                name: "FK_favorite_bands_bands_band_id",
                table: "favorite_bands");

            migrationBuilder.AddForeignKey(
                name: "FK_favorite_albums_albums_album_id",
                table: "favorite_albums",
                column: "album_id",
                principalTable: "albums",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_favorite_bands_bands_band_id",
                table: "favorite_bands",
                column: "band_id",
                principalTable: "bands",
                principalColumn: "band_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_favorite_albums_albums_album_id",
                table: "favorite_albums");

            migrationBuilder.DropForeignKey(
                name: "FK_favorite_bands_bands_band_id",
                table: "favorite_bands");

            migrationBuilder.AddForeignKey(
                name: "FK_favorite_albums_albums_album_id",
                table: "favorite_albums",
                column: "album_id",
                principalTable: "albums",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_favorite_bands_bands_band_id",
                table: "favorite_bands",
                column: "band_id",
                principalTable: "bands",
                principalColumn: "band_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
