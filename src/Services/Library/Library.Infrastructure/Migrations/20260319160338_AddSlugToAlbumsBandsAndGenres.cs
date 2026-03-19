using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSlugToAlbumsBandsAndGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "slug",
                table: "genres",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "slug",
                table: "bands",
                type: "character varying(220)",
                maxLength: 220,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "slug",
                table: "albums",
                type: "character varying(220)",
                maxLength: 220,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_genres_slug",
                table: "genres",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_bands_slug",
                table: "bands",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_albums_slug",
                table: "albums",
                column: "slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_genres_slug",
                table: "genres");

            migrationBuilder.DropIndex(
                name: "ix_bands_slug",
                table: "bands");

            migrationBuilder.DropIndex(
                name: "ix_albums_slug",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "slug",
                table: "genres");

            migrationBuilder.DropColumn(
                name: "slug",
                table: "bands");

            migrationBuilder.DropColumn(
                name: "slug",
                table: "albums");
        }
    }
}
