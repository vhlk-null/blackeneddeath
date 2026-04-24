using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserContent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "album_comment_reactions",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    comment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_like = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album_comment_reactions", x => new { x.user_id, x.comment_id });
                    table.ForeignKey(
                        name: "FK_album_comment_reactions_album_comments_comment_id",
                        column: x => x.comment_id,
                        principalTable: "album_comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_album_comment_reactions_user_profiles_user_id",
                        column: x => x.user_id,
                        principalTable: "user_profiles",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "band_comment_reactions",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    comment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_like = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_band_comment_reactions", x => new { x.user_id, x.comment_id });
                    table.ForeignKey(
                        name: "FK_band_comment_reactions_band_comments_comment_id",
                        column: x => x.comment_id,
                        principalTable: "band_comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_band_comment_reactions_user_profiles_user_id",
                        column: x => x.user_id,
                        principalTable: "user_profiles",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_album_comment_reactions_comment_id",
                table: "album_comment_reactions",
                column: "comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_band_comment_reactions_comment_id",
                table: "band_comment_reactions",
                column: "comment_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "album_comment_reactions");

            migrationBuilder.DropTable(
                name: "band_comment_reactions");
        }
    }
}
