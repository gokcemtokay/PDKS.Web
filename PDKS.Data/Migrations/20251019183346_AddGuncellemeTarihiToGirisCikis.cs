using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGuncellemeTarihiToGirisCikis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_giris_cikislar_Personeller_cihaz_id",
                table: "giris_cikislar");

            migrationBuilder.DropForeignKey(
                name: "FK_giris_cikislar_cihazlar_CihazId1",
                table: "giris_cikislar");

            migrationBuilder.DropForeignKey(
                name: "FK_giris_cikislar_cihazlar_cihaz_id",
                table: "giris_cikislar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_giris_cikislar",
                table: "giris_cikislar");

            migrationBuilder.DropColumn(
                name: "kaynak",
                table: "giris_cikislar");

            migrationBuilder.RenameTable(
                name: "giris_cikislar",
                newName: "GirisCikislar");

            migrationBuilder.RenameColumn(
                name: "not",
                table: "GirisCikislar",
                newName: "Not");

            migrationBuilder.RenameColumn(
                name: "durum",
                table: "GirisCikislar",
                newName: "Durum");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "GirisCikislar",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "personel_id",
                table: "GirisCikislar",
                newName: "PersonelId");

            migrationBuilder.RenameColumn(
                name: "olusturma_tarihi",
                table: "GirisCikislar",
                newName: "OlusturmaTarihi");

            migrationBuilder.RenameColumn(
                name: "giris_zamani",
                table: "GirisCikislar",
                newName: "GirisZamani");

            migrationBuilder.RenameColumn(
                name: "gec_kalma_suresi",
                table: "GirisCikislar",
                newName: "GecKalmaSuresi");

            migrationBuilder.RenameColumn(
                name: "fazla_mesai_suresi",
                table: "GirisCikislar",
                newName: "FazlaMesaiSuresi");

            migrationBuilder.RenameColumn(
                name: "erken_cikis_suresi",
                table: "GirisCikislar",
                newName: "ErkenCikisSuresi");

            migrationBuilder.RenameColumn(
                name: "elle_giris",
                table: "GirisCikislar",
                newName: "ElleGiris");

            migrationBuilder.RenameColumn(
                name: "cikis_zamani",
                table: "GirisCikislar",
                newName: "CikisZamani");

            migrationBuilder.RenameColumn(
                name: "cihaz_id",
                table: "GirisCikislar",
                newName: "CihazId");

            migrationBuilder.RenameIndex(
                name: "IX_giris_cikislar_personel_id_giris_zamani",
                table: "GirisCikislar",
                newName: "IX_GirisCikislar_PersonelId_GirisZamani");

            migrationBuilder.RenameIndex(
                name: "IX_giris_cikislar_giris_zamani",
                table: "GirisCikislar",
                newName: "IX_GirisCikislar_GirisZamani");

            migrationBuilder.RenameIndex(
                name: "IX_giris_cikislar_CihazId1",
                table: "GirisCikislar",
                newName: "IX_GirisCikislar_CihazId1");

            migrationBuilder.RenameIndex(
                name: "IX_giris_cikislar_cihaz_id",
                table: "GirisCikislar",
                newName: "IX_GirisCikislar_CihazId");

            migrationBuilder.AlterColumn<string>(
                name: "Not",
                table: "GirisCikislar",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Durum",
                table: "GirisCikislar",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateTime>(
                name: "GuncellemeTarihi",
                table: "GirisCikislar",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GirisCikislar",
                table: "GirisCikislar",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GirisCikislar_Personeller_CihazId",
                table: "GirisCikislar",
                column: "CihazId",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GirisCikislar_cihazlar_CihazId",
                table: "GirisCikislar",
                column: "CihazId",
                principalTable: "cihazlar",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GirisCikislar_cihazlar_CihazId1",
                table: "GirisCikislar",
                column: "CihazId1",
                principalTable: "cihazlar",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GirisCikislar_Personeller_CihazId",
                table: "GirisCikislar");

            migrationBuilder.DropForeignKey(
                name: "FK_GirisCikislar_cihazlar_CihazId",
                table: "GirisCikislar");

            migrationBuilder.DropForeignKey(
                name: "FK_GirisCikislar_cihazlar_CihazId1",
                table: "GirisCikislar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GirisCikislar",
                table: "GirisCikislar");

            migrationBuilder.DropColumn(
                name: "GuncellemeTarihi",
                table: "GirisCikislar");

            migrationBuilder.RenameTable(
                name: "GirisCikislar",
                newName: "giris_cikislar");

            migrationBuilder.RenameColumn(
                name: "Not",
                table: "giris_cikislar",
                newName: "not");

            migrationBuilder.RenameColumn(
                name: "Durum",
                table: "giris_cikislar",
                newName: "durum");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "giris_cikislar",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PersonelId",
                table: "giris_cikislar",
                newName: "personel_id");

            migrationBuilder.RenameColumn(
                name: "OlusturmaTarihi",
                table: "giris_cikislar",
                newName: "olusturma_tarihi");

            migrationBuilder.RenameColumn(
                name: "GirisZamani",
                table: "giris_cikislar",
                newName: "giris_zamani");

            migrationBuilder.RenameColumn(
                name: "GecKalmaSuresi",
                table: "giris_cikislar",
                newName: "gec_kalma_suresi");

            migrationBuilder.RenameColumn(
                name: "FazlaMesaiSuresi",
                table: "giris_cikislar",
                newName: "fazla_mesai_suresi");

            migrationBuilder.RenameColumn(
                name: "ErkenCikisSuresi",
                table: "giris_cikislar",
                newName: "erken_cikis_suresi");

            migrationBuilder.RenameColumn(
                name: "ElleGiris",
                table: "giris_cikislar",
                newName: "elle_giris");

            migrationBuilder.RenameColumn(
                name: "CikisZamani",
                table: "giris_cikislar",
                newName: "cikis_zamani");

            migrationBuilder.RenameColumn(
                name: "CihazId",
                table: "giris_cikislar",
                newName: "cihaz_id");

            migrationBuilder.RenameIndex(
                name: "IX_GirisCikislar_PersonelId_GirisZamani",
                table: "giris_cikislar",
                newName: "IX_giris_cikislar_personel_id_giris_zamani");

            migrationBuilder.RenameIndex(
                name: "IX_GirisCikislar_GirisZamani",
                table: "giris_cikislar",
                newName: "IX_giris_cikislar_giris_zamani");

            migrationBuilder.RenameIndex(
                name: "IX_GirisCikislar_CihazId1",
                table: "giris_cikislar",
                newName: "IX_giris_cikislar_CihazId1");

            migrationBuilder.RenameIndex(
                name: "IX_GirisCikislar_CihazId",
                table: "giris_cikislar",
                newName: "IX_giris_cikislar_cihaz_id");

            migrationBuilder.AlterColumn<string>(
                name: "not",
                table: "giris_cikislar",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "durum",
                table: "giris_cikislar",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "kaynak",
                table: "giris_cikislar",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_giris_cikislar",
                table: "giris_cikislar",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_giris_cikislar_Personeller_cihaz_id",
                table: "giris_cikislar",
                column: "cihaz_id",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_giris_cikislar_cihazlar_CihazId1",
                table: "giris_cikislar",
                column: "CihazId1",
                principalTable: "cihazlar",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_giris_cikislar_cihazlar_cihaz_id",
                table: "giris_cikislar",
                column: "cihaz_id",
                principalTable: "cihazlar",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
