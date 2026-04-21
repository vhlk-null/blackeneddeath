using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAlbumReleaseMonthDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "release_day",
                table: "albums",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "release_month",
                table: "albums",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "release_day",
                table: "albums");

            migrationBuilder.DropColumn(
                name: "release_month",
                table: "albums");
        }
    }
}
