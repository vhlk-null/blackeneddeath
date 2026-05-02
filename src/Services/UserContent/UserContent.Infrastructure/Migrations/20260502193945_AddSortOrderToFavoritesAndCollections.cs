using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserContent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSortOrderToFavoritesAndCollections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "favorite_bands",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "favorite_albums",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "collection_bands",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "sort_order",
                table: "collection_albums",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "favorite_bands");

            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "favorite_albums");

            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "collection_bands");

            migrationBuilder.DropColumn(
                name: "sort_order",
                table: "collection_albums");
        }
    }
}
