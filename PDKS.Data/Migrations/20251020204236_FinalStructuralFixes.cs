using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinalStructuralFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CihazLoglari_cihazlar_CihazId1",
                table: "CihazLoglari");

            migrationBuilder.DropForeignKey(
                name: "FK_GirisCikislar_Personeller_CihazId",
                table: "GirisCikislar");

            migrationBuilder.DropForeignKey(
                name: "FK_GirisCikislar_cihazlar_CihazId1",
                table: "GirisCikislar");

            migrationBuilder.DropIndex(
                name: "IX_GirisCikislar_CihazId1",
                table: "GirisCikislar");

            migrationBuilder.DropIndex(
                name: "IX_CihazLoglari_CihazId1",
                table: "CihazLoglari");

            migrationBuilder.DropColumn(
                name: "CihazId1",
                table: "GirisCikislar");

            migrationBuilder.DropColumn(
                name: "CihazId1",
                table: "CihazLoglari");

            migrationBuilder.CreateTable(
                name: "kullanici_sirketler",
                columns: table => new
                {
                    kullanici_id = table.Column<int>(type: "integer", nullable: false),
                    sirket_id = table.Column<int>(type: "integer", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false),
                    varsayilan = table.Column<bool>(type: "boolean", nullable: false),
                    aktif = table.Column<bool>(type: "boolean", nullable: false),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kullanici_sirketler", x => new { x.kullanici_id, x.sirket_id });
                    table.ForeignKey(
                        name: "FK_kullanici_sirketler_Kullanicilar_kullanici_id",
                        column: x => x.kullanici_id,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_kullanici_sirketler_sirketler_sirket_id",
                        column: x => x.sirket_id,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_kullanici_sirketler_sirket_id",
                table: "kullanici_sirketler",
                column: "sirket_id");

            migrationBuilder.AddForeignKey(
                name: "FK_GirisCikislar_Personeller_PersonelId",
                table: "GirisCikislar",
                column: "PersonelId",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GirisCikislar_Personeller_PersonelId",
                table: "GirisCikislar");

            migrationBuilder.DropTable(
                name: "kullanici_sirketler");

            migrationBuilder.AddColumn<int>(
                name: "CihazId1",
                table: "GirisCikislar",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CihazId1",
                table: "CihazLoglari",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GirisCikislar_CihazId1",
                table: "GirisCikislar",
                column: "CihazId1");

            migrationBuilder.CreateIndex(
                name: "IX_CihazLoglari_CihazId1",
                table: "CihazLoglari",
                column: "CihazId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CihazLoglari_cihazlar_CihazId1",
                table: "CihazLoglari",
                column: "CihazId1",
                principalTable: "cihazlar",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_GirisCikislar_Personeller_CihazId",
                table: "GirisCikislar",
                column: "CihazId",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GirisCikislar_cihazlar_CihazId1",
                table: "GirisCikislar",
                column: "CihazId1",
                principalTable: "cihazlar",
                principalColumn: "id");
        }
    }
}
