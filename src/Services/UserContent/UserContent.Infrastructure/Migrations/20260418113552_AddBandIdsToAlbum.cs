using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserContent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBandIdsToAlbum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "band_ids",
                table: "albums",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "band_ids",
                table: "albums");
        }
    }
}
