using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGenreCardOrderNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "order_number",
                table: "genre_cards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("""
                UPDATE genre_cards SET order_number = CASE name
                    WHEN 'Classic'        THEN 1
                    WHEN 'War Metal'      THEN 2
                    WHEN 'Cavernous'      THEN 3
                    WHEN 'Blackened Death' THEN 4
                    WHEN 'Melodic'        THEN 5
                    WHEN 'Symphonic'      THEN 6
                    WHEN 'Doom'           THEN 7
                    WHEN 'Thrash'         THEN 8
                    WHEN 'Dissonant'      THEN 9
                    WHEN 'Progressive'    THEN 10
                    WHEN 'Technical'      THEN 11
                    WHEN 'Avant-Garde'    THEN 12
                    WHEN 'Grind'          THEN 13
                    WHEN 'Brutal'         THEN 14
                    WHEN 'Sludge/Crust'   THEN 15
                    WHEN 'Folk/Oriental'  THEN 16
                    ELSE 0
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order_number",
                table: "genre_cards");
        }
    }
}
