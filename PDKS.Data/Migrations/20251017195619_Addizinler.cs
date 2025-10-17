using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addizinler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_giris_cikislar_Personeller_personel_id",
                table: "giris_cikislar");

            migrationBuilder.DropForeignKey(
                name: "FK_izinler_Kullanicilar_onaylayan_id",
                table: "izinler");

            migrationBuilder.DropForeignKey(
                name: "FK_izinler_Personeller_personel_id",
                table: "izinler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_izinler",
                table: "izinler");

            migrationBuilder.RenameTable(
                name: "izinler",
                newName: "Izinler");

            migrationBuilder.RenameColumn(
                name: "aciklama",
                table: "Izinler",
                newName: "Aciklama");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Izinler",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "red_nedeni",
                table: "Izinler",
                newName: "RedNedeni");

            migrationBuilder.RenameColumn(
                name: "personel_id",
                table: "Izinler",
                newName: "PersonelId");

            migrationBuilder.RenameColumn(
                name: "onay_tarihi",
                table: "Izinler",
                newName: "OnayTarihi");

            migrationBuilder.RenameColumn(
                name: "onay_durumu",
                table: "Izinler",
                newName: "OnayDurumu");

            migrationBuilder.RenameColumn(
                name: "olusturma_tarihi",
                table: "Izinler",
                newName: "OlusturmaTarihi");

            migrationBuilder.RenameColumn(
                name: "izin_tipi",
                table: "Izinler",
                newName: "IzinTipi");

            migrationBuilder.RenameColumn(
                name: "bitis_tarihi",
                table: "Izinler",
                newName: "BitisTarihi");

            migrationBuilder.RenameColumn(
                name: "baslangic_tarihi",
                table: "Izinler",
                newName: "BaslangicTarihi");

            migrationBuilder.RenameColumn(
                name: "onaylayan_id",
                table: "Izinler",
                newName: "OnaylayanKullaniciId");

            migrationBuilder.RenameColumn(
                name: "gun_sayisi",
                table: "Izinler",
                newName: "IzinGunSayisi");

            migrationBuilder.RenameIndex(
                name: "IX_izinler_personel_id",
                table: "Izinler",
                newName: "IX_Izinler_PersonelId");

            migrationBuilder.RenameIndex(
                name: "IX_izinler_onaylayan_id",
                table: "Izinler",
                newName: "IX_Izinler_OnaylayanKullaniciId");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "Izinler",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "RedNedeni",
                table: "Izinler",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<DateTime>(
                name: "TalepTarihi",
                table: "Izinler",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Izinler",
                table: "Izinler",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_giris_cikislar_Personeller_cihaz_id",
                table: "giris_cikislar",
                column: "cihaz_id",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Izinler_Kullanicilar_OnaylayanKullaniciId",
                table: "Izinler",
                column: "OnaylayanKullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Izinler_Personeller_PersonelId",
                table: "Izinler",
                column: "PersonelId",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_giris_cikislar_Personeller_cihaz_id",
                table: "giris_cikislar");

            migrationBuilder.DropForeignKey(
                name: "FK_Izinler_Kullanicilar_OnaylayanKullaniciId",
                table: "Izinler");

            migrationBuilder.DropForeignKey(
                name: "FK_Izinler_Personeller_PersonelId",
                table: "Izinler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Izinler",
                table: "Izinler");

            migrationBuilder.DropColumn(
                name: "TalepTarihi",
                table: "Izinler");

            migrationBuilder.RenameTable(
                name: "Izinler",
                newName: "izinler");

            migrationBuilder.RenameColumn(
                name: "Aciklama",
                table: "izinler",
                newName: "aciklama");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "izinler",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RedNedeni",
                table: "izinler",
                newName: "red_nedeni");

            migrationBuilder.RenameColumn(
                name: "PersonelId",
                table: "izinler",
                newName: "personel_id");

            migrationBuilder.RenameColumn(
                name: "OnayTarihi",
                table: "izinler",
                newName: "onay_tarihi");

            migrationBuilder.RenameColumn(
                name: "OnayDurumu",
                table: "izinler",
                newName: "onay_durumu");

            migrationBuilder.RenameColumn(
                name: "OlusturmaTarihi",
                table: "izinler",
                newName: "olusturma_tarihi");

            migrationBuilder.RenameColumn(
                name: "IzinTipi",
                table: "izinler",
                newName: "izin_tipi");

            migrationBuilder.RenameColumn(
                name: "BitisTarihi",
                table: "izinler",
                newName: "bitis_tarihi");

            migrationBuilder.RenameColumn(
                name: "BaslangicTarihi",
                table: "izinler",
                newName: "baslangic_tarihi");

            migrationBuilder.RenameColumn(
                name: "OnaylayanKullaniciId",
                table: "izinler",
                newName: "onaylayan_id");

            migrationBuilder.RenameColumn(
                name: "IzinGunSayisi",
                table: "izinler",
                newName: "gun_sayisi");

            migrationBuilder.RenameIndex(
                name: "IX_Izinler_PersonelId",
                table: "izinler",
                newName: "IX_izinler_personel_id");

            migrationBuilder.RenameIndex(
                name: "IX_Izinler_OnaylayanKullaniciId",
                table: "izinler",
                newName: "IX_izinler_onaylayan_id");

            migrationBuilder.AlterColumn<string>(
                name: "aciklama",
                table: "izinler",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "red_nedeni",
                table: "izinler",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_izinler",
                table: "izinler",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_giris_cikislar_Personeller_personel_id",
                table: "giris_cikislar",
                column: "personel_id",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_izinler_Kullanicilar_onaylayan_id",
                table: "izinler",
                column: "onaylayan_id",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_izinler_Personeller_personel_id",
                table: "izinler",
                column: "personel_id",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
