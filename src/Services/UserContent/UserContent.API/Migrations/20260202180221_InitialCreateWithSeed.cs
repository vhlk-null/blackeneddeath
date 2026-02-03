using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserContent.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    RegisteredDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    FavoriteBandsCount = table.Column<int>(type: "integer", nullable: false),
                    FavoriteAlbumsCount = table.Column<int>(type: "integer", nullable: false),
                    ReviewsCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteAlbums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlbumId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserRating = table.Column<int>(type: "integer", nullable: true),
                    UserReview = table.Column<string>(type: "text", nullable: true),
                    UserProfileInfoUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteAlbums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteAlbums_UserProfiles_UserProfileInfoUserId",
                        column: x => x.UserProfileInfoUserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "FavoriteBands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    BandId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsFollowing = table.Column<bool>(type: "boolean", nullable: false),
                    UserProfileInfoUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteBands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteBands_UserProfiles_UserProfileInfoUserId",
                        column: x => x.UserProfileInfoUserId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteAlbums_UserId_AlbumId",
                table: "FavoriteAlbums",
                columns: new[] { "UserId", "AlbumId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteAlbums_UserProfileInfoUserId",
                table: "FavoriteAlbums",
                column: "UserProfileInfoUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteBands_UserId_BandId",
                table: "FavoriteBands",
                columns: new[] { "UserId", "BandId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteBands_UserProfileInfoUserId",
                table: "FavoriteBands",
                column: "UserProfileInfoUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteAlbums");

            migrationBuilder.DropTable(
                name: "FavoriteBands");

            migrationBuilder.DropTable(
                name: "UserProfiles");
        }
    }
}
