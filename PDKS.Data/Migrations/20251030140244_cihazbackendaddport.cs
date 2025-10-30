using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class cihazbackendaddport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cihaz_tipi",
                table: "cihazlar",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "port",
                table: "cihazlar",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cihaz_tipi",
                table: "cihazlar");

            migrationBuilder.DropColumn(
                name: "port",
                table: "cihazlar");
        }
    }
}
