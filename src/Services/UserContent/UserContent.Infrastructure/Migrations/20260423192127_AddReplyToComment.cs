using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserContent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReplyToComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "reply_to_comment_id",
                table: "band_comments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reply_to_username",
                table: "band_comments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "reply_to_comment_id",
                table: "album_comments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "reply_to_username",
                table: "album_comments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reply_to_comment_id",
                table: "band_comments");

            migrationBuilder.DropColumn(
                name: "reply_to_username",
                table: "band_comments");

            migrationBuilder.DropColumn(
                name: "reply_to_comment_id",
                table: "album_comments");

            migrationBuilder.DropColumn(
                name: "reply_to_username",
                table: "album_comments");
        }
    }
}
