using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserContent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAlbumCardFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "band_names",
                table: "albums",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "band_slugs",
                table: "albums",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "country_codes",
                table: "albums",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "country_names",
                table: "albums",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "format",
                table: "albums",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "primary_genre_name",
                table: "albums",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "primary_genre_slug",
                table: "albums",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "slug",
                table: "albums",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "albums",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "band_names",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "band_slugs",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "country_codes",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "country_names",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "format",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "primary_genre_name",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "primary_genre_slug",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "slug",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "type",
                table: "albums");
        }
    }
}
