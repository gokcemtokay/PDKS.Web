using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class eksikler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cihaz_loglari_cihazlar_CihazId1",
                table: "cihaz_loglari");

            migrationBuilder.DropForeignKey(
                name: "FK_cihaz_loglari_cihazlar_cihaz_id",
                table: "cihaz_loglari");

            migrationBuilder.DropForeignKey(
                name: "FK_loglar_Kullanicilar_kullanici_id",
                table: "loglar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_loglar",
                table: "loglar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cihaz_loglari",
                table: "cihaz_loglari");

            migrationBuilder.DropColumn(
                name: "ip_adres",
                table: "loglar");

            migrationBuilder.DropColumn(
                name: "modul",
                table: "loglar");

            migrationBuilder.DropColumn(
                name: "basarili",
                table: "cihaz_loglari");

            migrationBuilder.DropColumn(
                name: "islem",
                table: "cihaz_loglari");

            migrationBuilder.RenameTable(
                name: "loglar",
                newName: "Loglar");

            migrationBuilder.RenameTable(
                name: "cihaz_loglari",
                newName: "CihazLoglari");

            migrationBuilder.RenameColumn(
                name: "tarih",
                table: "Loglar",
                newName: "Tarih");

            migrationBuilder.RenameColumn(
                name: "islem",
                table: "Loglar",
                newName: "Islem");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Loglar",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "kullanici_id",
                table: "Loglar",
                newName: "KullaniciId");

            migrationBuilder.RenameColumn(
                name: "detay",
                table: "Loglar",
                newName: "LogLevel");

            migrationBuilder.RenameIndex(
                name: "IX_loglar_kullanici_id",
                table: "Loglar",
                newName: "IX_Loglar_KullaniciId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "CihazLoglari",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "tarih",
                table: "CihazLoglari",
                newName: "log_tarihi");

            migrationBuilder.RenameColumn(
                name: "cihaz_id",
                table: "CihazLoglari",
                newName: "CihazId");

            migrationBuilder.RenameColumn(
                name: "detay",
                table: "CihazLoglari",
                newName: "log_tipi");

            migrationBuilder.RenameIndex(
                name: "IX_cihaz_loglari_CihazId1",
                table: "CihazLoglari",
                newName: "IX_CihazLoglari_CihazId1");

            migrationBuilder.RenameIndex(
                name: "IX_cihaz_loglari_cihaz_id",
                table: "CihazLoglari",
                newName: "IX_CihazLoglari_CihazId");

            migrationBuilder.AlterColumn<string>(
                name: "Islem",
                table: "Loglar",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Aciklama",
                table: "Loglar",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IpAdresi",
                table: "Loglar",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "log_mesaji",
                table: "CihazLoglari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Loglar",
                table: "Loglar",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CihazLoglari",
                table: "CihazLoglari",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CihazLoglari_cihazlar_CihazId",
                table: "CihazLoglari",
                column: "CihazId",
                principalTable: "cihazlar",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CihazLoglari_cihazlar_CihazId1",
                table: "CihazLoglari",
                column: "CihazId1",
                principalTable: "cihazlar",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Loglar_Kullanicilar_KullaniciId",
                table: "Loglar",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CihazLoglari_cihazlar_CihazId",
                table: "CihazLoglari");

            migrationBuilder.DropForeignKey(
                name: "FK_CihazLoglari_cihazlar_CihazId1",
                table: "CihazLoglari");

            migrationBuilder.DropForeignKey(
                name: "FK_Loglar_Kullanicilar_KullaniciId",
                table: "Loglar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Loglar",
                table: "Loglar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CihazLoglari",
                table: "CihazLoglari");

            migrationBuilder.DropColumn(
                name: "Aciklama",
                table: "Loglar");

            migrationBuilder.DropColumn(
                name: "IpAdresi",
                table: "Loglar");

            migrationBuilder.DropColumn(
                name: "log_mesaji",
                table: "CihazLoglari");

            migrationBuilder.RenameTable(
                name: "Loglar",
                newName: "loglar");

            migrationBuilder.RenameTable(
                name: "CihazLoglari",
                newName: "cihaz_loglari");

            migrationBuilder.RenameColumn(
                name: "Tarih",
                table: "loglar",
                newName: "tarih");

            migrationBuilder.RenameColumn(
                name: "Islem",
                table: "loglar",
                newName: "islem");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "loglar",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "KullaniciId",
                table: "loglar",
                newName: "kullanici_id");

            migrationBuilder.RenameColumn(
                name: "LogLevel",
                table: "loglar",
                newName: "detay");

            migrationBuilder.RenameIndex(
                name: "IX_Loglar_KullaniciId",
                table: "loglar",
                newName: "IX_loglar_kullanici_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "cihaz_loglari",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "log_tarihi",
                table: "cihaz_loglari",
                newName: "tarih");

            migrationBuilder.RenameColumn(
                name: "CihazId",
                table: "cihaz_loglari",
                newName: "cihaz_id");

            migrationBuilder.RenameColumn(
                name: "log_tipi",
                table: "cihaz_loglari",
                newName: "detay");

            migrationBuilder.RenameIndex(
                name: "IX_CihazLoglari_CihazId1",
                table: "cihaz_loglari",
                newName: "IX_cihaz_loglari_CihazId1");

            migrationBuilder.RenameIndex(
                name: "IX_CihazLoglari_CihazId",
                table: "cihaz_loglari",
                newName: "IX_cihaz_loglari_cihaz_id");

            migrationBuilder.AlterColumn<string>(
                name: "islem",
                table: "loglar",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ip_adres",
                table: "loglar",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "modul",
                table: "loglar",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "basarili",
                table: "cihaz_loglari",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "islem",
                table: "cihaz_loglari",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_loglar",
                table: "loglar",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cihaz_loglari",
                table: "cihaz_loglari",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_cihaz_loglari_cihazlar_CihazId1",
                table: "cihaz_loglari",
                column: "CihazId1",
                principalTable: "cihazlar",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_cihaz_loglari_cihazlar_cihaz_id",
                table: "cihaz_loglari",
                column: "cihaz_id",
                principalTable: "cihazlar",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_loglar_Kullanicilar_kullanici_id",
                table: "loglar",
                column: "kullanici_id",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
