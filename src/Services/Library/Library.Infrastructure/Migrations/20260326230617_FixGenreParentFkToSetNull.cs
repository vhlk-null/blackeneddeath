using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixGenreParentFkToSetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_genres_genres_parent_genre_id",
                table: "genres");

            migrationBuilder.AddForeignKey(
                name: "FK_genres_genres_parent_genre_id",
                table: "genres",
                column: "parent_genre_id",
                principalTable: "genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_genres_genres_parent_genre_id",
                table: "genres");

            migrationBuilder.AddForeignKey(
                name: "FK_genres_genres_parent_genre_id",
                table: "genres",
                column: "parent_genre_id",
                principalTable: "genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
