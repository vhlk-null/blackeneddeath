using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserContent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGradeFromReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "grade",
                table: "band_reviews");

            migrationBuilder.DropColumn(
                name: "grade",
                table: "album_reviews");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "grade",
                table: "band_reviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "grade",
                table: "album_reviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
