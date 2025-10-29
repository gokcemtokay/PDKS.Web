using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class Multicompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cihazlar_sirketler_SirketId",
                table: "cihazlar");

            migrationBuilder.AddColumn<int>(
                name: "SirketId",
                table: "tatiller",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SirketId",
                table: "Parametreler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SirketId",
                table: "Mesailer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SirketId",
                table: "Menuler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SirketId",
                table: "Kullanicilar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SirketId",
                table: "cihazlar",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Parametreler",
                keyColumn: "Id",
                keyValue: 1,
                column: "SirketId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Parametreler",
                keyColumn: "Id",
                keyValue: 2,
                column: "SirketId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Parametreler",
                keyColumn: "Id",
                keyValue: 3,
                column: "SirketId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Parametreler",
                keyColumn: "Id",
                keyValue: 4,
                column: "SirketId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Parametreler",
                keyColumn: "Id",
                keyValue: 5,
                column: "SirketId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Parametreler",
                keyColumn: "Id",
                keyValue: 6,
                column: "SirketId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Parametreler",
                keyColumn: "Id",
                keyValue: 7,
                column: "SirketId",
                value: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_cihazlar_sirketler_SirketId",
                table: "cihazlar",
                column: "SirketId",
                principalTable: "sirketler",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cihazlar_sirketler_SirketId",
                table: "cihazlar");

            migrationBuilder.DropColumn(
                name: "SirketId",
                table: "tatiller");

            migrationBuilder.DropColumn(
                name: "SirketId",
                table: "Parametreler");

            migrationBuilder.DropColumn(
                name: "SirketId",
                table: "Mesailer");

            migrationBuilder.DropColumn(
                name: "SirketId",
                table: "Menuler");

            migrationBuilder.DropColumn(
                name: "SirketId",
                table: "Kullanicilar");

            migrationBuilder.AlterColumn<int>(
                name: "SirketId",
                table: "cihazlar",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_cihazlar_sirketler_SirketId",
                table: "cihazlar",
                column: "SirketId",
                principalTable: "sirketler",
                principalColumn: "id");
        }
    }
}
