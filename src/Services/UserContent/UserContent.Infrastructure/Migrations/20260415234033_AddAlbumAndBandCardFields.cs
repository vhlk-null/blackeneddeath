using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserContent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAlbumAndBandCardFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "release_date",
                table: "bands",
                newName: "status");

            migrationBuilder.AddColumn<string>(
                name: "country_codes",
                table: "bands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "country_names",
                table: "bands",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "disbanded_year",
                table: "bands",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "formed_year",
                table: "bands",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "primary_genre_name",
                table: "bands",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "primary_genre_slug",
                table: "bands",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "slug",
                table: "bands",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "country_codes",
                table: "bands");

            migrationBuilder.DropColumn(
                name: "country_names",
                table: "bands");

            migrationBuilder.DropColumn(
                name: "disbanded_year",
                table: "bands");

            migrationBuilder.DropColumn(
                name: "formed_year",
                table: "bands");

            migrationBuilder.DropColumn(
                name: "primary_genre_name",
                table: "bands");

            migrationBuilder.DropColumn(
                name: "primary_genre_slug",
                table: "bands");

            migrationBuilder.DropColumn(
                name: "slug",
                table: "bands");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "bands",
                newName: "release_date");
        }
    }
}
