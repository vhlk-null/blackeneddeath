using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Archive.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "albums",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    release_date = table.Column<int>(type: "integer", nullable: false),
                    cover_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    format = table.Column<int>(type: "integer", nullable: false),
                    label = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_albums", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    parent_genre_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.id);
                    table.ForeignKey(
                        name: "FK_genres_genres_parent_genre_id",
                        column: x => x.parent_genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tracks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tracks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "streaming_links",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    platform = table.Column<int>(type: "integer", nullable: false),
                    embed_code = table.Column<string>(type: "text", nullable: false),
                    AddedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    album_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streaming_links", x => x.id);
                    table.ForeignKey(
                        name: "FK_streaming_links_albums_album_id",
                        column: x => x.album_id,
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "album_countries",
                columns: table => new
                {
                    album_id = table.Column<Guid>(type: "uuid", nullable: false),
                    country_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album_countries", x => new { x.album_id, x.country_id });
                    table.ForeignKey(
                        name: "FK_album_countries_albums_album_id",
                        column: x => x.album_id,
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_album_countries_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bands",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    bio = table.Column<string>(type: "text", nullable: true),
                    country_id = table.Column<Guid>(type: "uuid", nullable: true),
                    logo_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    formed_year = table.Column<int>(type: "integer", nullable: true),
                    disbanded_year = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bands", x => x.id);
                    table.ForeignKey(
                        name: "FK_bands_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "album_genres",
                columns: table => new
                {
                    album_id = table.Column<Guid>(type: "uuid", nullable: false),
                    genre_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album_genres", x => new { x.album_id, x.genre_id });
                    table.ForeignKey(
                        name: "FK_album_genres_albums_album_id",
                        column: x => x.album_id,
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_album_genres_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "album_tracks",
                columns: table => new
                {
                    album_id = table.Column<Guid>(type: "uuid", nullable: false),
                    track_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album_tracks", x => new { x.album_id, x.track_id });
                    table.ForeignKey(
                        name: "FK_album_tracks_albums_album_id",
                        column: x => x.album_id,
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_album_tracks_tracks_track_id",
                        column: x => x.track_id,
                        principalTable: "tracks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "album_bands",
                columns: table => new
                {
                    album_id = table.Column<Guid>(type: "uuid", nullable: false),
                    band_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album_bands", x => new { x.album_id, x.band_id });
                    table.ForeignKey(
                        name: "FK_album_bands_albums_album_id",
                        column: x => x.album_id,
                        principalTable: "albums",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_album_bands_bands_band_id",
                        column: x => x.band_id,
                        principalTable: "bands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "band_genres",
                columns: table => new
                {
                    band_id = table.Column<Guid>(type: "uuid", nullable: false),
                    genre_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_band_genres", x => new { x.band_id, x.genre_id });
                    table.ForeignKey(
                        name: "FK_band_genres_bands_band_id",
                        column: x => x.band_id,
                        principalTable: "bands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_band_genres_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_album_bands_band_id",
                table: "album_bands",
                column: "band_id");

            migrationBuilder.CreateIndex(
                name: "IX_album_countries_country_id",
                table: "album_countries",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_album_genres_genre_id",
                table: "album_genres",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_album_tracks_track_id",
                table: "album_tracks",
                column: "track_id");

            migrationBuilder.CreateIndex(
                name: "IX_band_genres_genre_id",
                table: "band_genres",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_bands_country_id",
                table: "bands",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_countries_code",
                table: "countries",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_genres_name",
                table: "genres",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_genres_parent_genre_id",
                table: "genres",
                column: "parent_genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_streaming_links_album_id",
                table: "streaming_links",
                column: "album_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "album_bands");

            migrationBuilder.DropTable(
                name: "album_countries");

            migrationBuilder.DropTable(
                name: "album_genres");

            migrationBuilder.DropTable(
                name: "album_tracks");

            migrationBuilder.DropTable(
                name: "band_genres");

            migrationBuilder.DropTable(
                name: "streaming_links");

            migrationBuilder.DropTable(
                name: "tracks");

            migrationBuilder.DropTable(
                name: "bands");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "albums");

            migrationBuilder.DropTable(
                name: "countries");
        }
    }
}
