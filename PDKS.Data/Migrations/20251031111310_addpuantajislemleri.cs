using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class addpuantajislemleri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PuantajDetaylar_GirisCikislar_GirisCikisId",
                table: "PuantajDetaylar");

            migrationBuilder.DropForeignKey(
                name: "FK_PuantajDetaylar_Personeller_PersonelId",
                table: "PuantajDetaylar");

            migrationBuilder.DropForeignKey(
                name: "FK_PuantajDetaylar_Vardiyalar_VardiyaId",
                table: "PuantajDetaylar");

            migrationBuilder.DropForeignKey(
                name: "FK_Puantajlar_Kullanicilar_OnaylayanKullaniciId",
                table: "Puantajlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Puantajlar_sirketler_SirketId",
                table: "Puantajlar");

            migrationBuilder.DropIndex(
                name: "IX_Puantajlar_PersonelId",
                table: "Puantajlar");

            migrationBuilder.DropIndex(
                name: "IX_PuantajDetaylar_GirisCikisId",
                table: "PuantajDetaylar");

            migrationBuilder.DropIndex(
                name: "IX_PuantajDetaylar_PersonelId",
                table: "PuantajDetaylar");

            migrationBuilder.DropIndex(
                name: "IX_PuantajDetaylar_PuantajId",
                table: "PuantajDetaylar");

            migrationBuilder.DropColumn(
                name: "GunDurumu",
                table: "PuantajDetaylar");

            migrationBuilder.DropColumn(
                name: "PersonelId",
                table: "PuantajDetaylar");

            migrationBuilder.DropColumn(
                name: "PlanlananSure",
                table: "PuantajDetaylar");

            migrationBuilder.RenameColumn(
                name: "ToplamCalismaSaati",
                table: "Puantajlar",
                newName: "UcretsizIzinGunSayisi");

            migrationBuilder.RenameColumn(
                name: "ResmiTatilGunu",
                table: "Puantajlar",
                newName: "ToplamGecKalmaSuresi");

            migrationBuilder.RenameColumn(
                name: "RaporluGun",
                table: "Puantajlar",
                newName: "ToplamErkenCikisSuresi");

            migrationBuilder.RenameColumn(
                name: "NormalMesaiSaati",
                table: "Puantajlar",
                newName: "ToplamEksikCalismaSuresi");

            migrationBuilder.RenameColumn(
                name: "IzinGunu",
                table: "Puantajlar",
                newName: "ToplamCalismaSuresi");

            migrationBuilder.RenameColumn(
                name: "HaftaTatiliGunu",
                table: "Puantajlar",
                newName: "ResmiTatilGunSayisi");

            migrationBuilder.RenameColumn(
                name: "HaftaSonuMesaiSaati",
                table: "Puantajlar",
                newName: "ResmiTatilCalismaSuresi");

            migrationBuilder.RenameColumn(
                name: "GeceMesaiSaati",
                table: "Puantajlar",
                newName: "NormalMesaiSuresi");

            migrationBuilder.RenameColumn(
                name: "GecKalmaSuresi",
                table: "Puantajlar",
                newName: "MazeretliIzinGunSayisi");

            migrationBuilder.RenameColumn(
                name: "GecKalmaGunu",
                table: "Puantajlar",
                newName: "IzinGunSayisi");

            migrationBuilder.RenameColumn(
                name: "FazlaMesaiSaati",
                table: "Puantajlar",
                newName: "HastaTatiliGunSayisi");

            migrationBuilder.RenameColumn(
                name: "ErkenCikisSuresi",
                table: "Puantajlar",
                newName: "HaftaTatiliGunSayisi");

            migrationBuilder.RenameColumn(
                name: "ErkenCikisGunu",
                table: "Puantajlar",
                newName: "HaftaTatiliCalismaSuresi");

            migrationBuilder.RenameColumn(
                name: "EksikCalismaSaati",
                table: "Puantajlar",
                newName: "GeceMesaiSuresi");

            migrationBuilder.RenameColumn(
                name: "DevamsizlikGunu",
                table: "Puantajlar",
                newName: "GecKalmaGunSayisi");

            migrationBuilder.RenameColumn(
                name: "ResmiTatilMi",
                table: "PuantajDetaylar",
                newName: "IzinliMi");

            migrationBuilder.RenameColumn(
                name: "PlanlananGiris",
                table: "PuantajDetaylar",
                newName: "VardiyaBitis");

            migrationBuilder.RenameColumn(
                name: "PlanlananCikis",
                table: "PuantajDetaylar",
                newName: "VardiyaBaslangic");

            migrationBuilder.RenameColumn(
                name: "NormalMesai",
                table: "PuantajDetaylar",
                newName: "ToplamMolaSuresi");

            migrationBuilder.RenameColumn(
                name: "HaftaSonuMu",
                table: "PuantajDetaylar",
                newName: "ElleGirildiMi");

            migrationBuilder.RenameColumn(
                name: "GirisCikisId",
                table: "PuantajDetaylar",
                newName: "ToplamCalismaSuresi");

            migrationBuilder.RenameColumn(
                name: "GerceklesenSure",
                table: "PuantajDetaylar",
                newName: "PlanlananCalismaSuresi");

            migrationBuilder.RenameColumn(
                name: "GerceklesenGiris",
                table: "PuantajDetaylar",
                newName: "SonCikis");

            migrationBuilder.RenameColumn(
                name: "GerceklesenCikis",
                table: "PuantajDetaylar",
                newName: "IlkGiris");

            migrationBuilder.RenameColumn(
                name: "GeceMesai",
                table: "PuantajDetaylar",
                newName: "MolaAdedi");

            migrationBuilder.RenameColumn(
                name: "FazlaMesai",
                table: "PuantajDetaylar",
                newName: "IzinId");

            migrationBuilder.RenameColumn(
                name: "EksikSure",
                table: "PuantajDetaylar",
                newName: "GunTipi");

            migrationBuilder.AlterColumn<string>(
                name: "Notlar",
                table: "Puantajlar",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Durum",
                table: "Puantajlar",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "DevamsizlikGunSayisi",
                table: "Puantajlar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ErkenCikisGunSayisi",
                table: "Puantajlar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FazlaMesaiSuresi",
                table: "Puantajlar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Onaylandi",
                table: "Puantajlar",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Notlar",
                table: "PuantajDetaylar",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IzinTuru",
                table: "PuantajDetaylar",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CalismaDurumu",
                table: "PuantajDetaylar",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "DuzeltmeYapildiMi",
                table: "PuantajDetaylar",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EksikCalismaSuresi",
                table: "PuantajDetaylar",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FazlaMesaiSuresi",
                table: "PuantajDetaylar",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Puantajlar_PersonelId_Yil_Ay",
                table: "Puantajlar",
                columns: new[] { "PersonelId", "Yil", "Ay" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PuantajDetaylar_IzinId",
                table: "PuantajDetaylar",
                column: "IzinId");

            migrationBuilder.CreateIndex(
                name: "IX_PuantajDetaylar_PuantajId_Tarih",
                table: "PuantajDetaylar",
                columns: new[] { "PuantajId", "Tarih" });

            migrationBuilder.AddForeignKey(
                name: "FK_PuantajDetaylar_Izinler_IzinId",
                table: "PuantajDetaylar",
                column: "IzinId",
                principalTable: "Izinler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PuantajDetaylar_Vardiyalar_VardiyaId",
                table: "PuantajDetaylar",
                column: "VardiyaId",
                principalTable: "Vardiyalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Puantajlar_Kullanicilar_OnaylayanKullaniciId",
                table: "Puantajlar",
                column: "OnaylayanKullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Puantajlar_sirketler_SirketId",
                table: "Puantajlar",
                column: "SirketId",
                principalTable: "sirketler",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PuantajDetaylar_Izinler_IzinId",
                table: "PuantajDetaylar");

            migrationBuilder.DropForeignKey(
                name: "FK_PuantajDetaylar_Vardiyalar_VardiyaId",
                table: "PuantajDetaylar");

            migrationBuilder.DropForeignKey(
                name: "FK_Puantajlar_Kullanicilar_OnaylayanKullaniciId",
                table: "Puantajlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Puantajlar_sirketler_SirketId",
                table: "Puantajlar");

            migrationBuilder.DropIndex(
                name: "IX_Puantajlar_PersonelId_Yil_Ay",
                table: "Puantajlar");

            migrationBuilder.DropIndex(
                name: "IX_PuantajDetaylar_IzinId",
                table: "PuantajDetaylar");

            migrationBuilder.DropIndex(
                name: "IX_PuantajDetaylar_PuantajId_Tarih",
                table: "PuantajDetaylar");

            migrationBuilder.DropColumn(
                name: "DevamsizlikGunSayisi",
                table: "Puantajlar");

            migrationBuilder.DropColumn(
                name: "ErkenCikisGunSayisi",
                table: "Puantajlar");

            migrationBuilder.DropColumn(
                name: "FazlaMesaiSuresi",
                table: "Puantajlar");

            migrationBuilder.DropColumn(
                name: "Onaylandi",
                table: "Puantajlar");

            migrationBuilder.DropColumn(
                name: "CalismaDurumu",
                table: "PuantajDetaylar");

            migrationBuilder.DropColumn(
                name: "DuzeltmeYapildiMi",
                table: "PuantajDetaylar");

            migrationBuilder.DropColumn(
                name: "EksikCalismaSuresi",
                table: "PuantajDetaylar");

            migrationBuilder.DropColumn(
                name: "FazlaMesaiSuresi",
                table: "PuantajDetaylar");

            migrationBuilder.RenameColumn(
                name: "UcretsizIzinGunSayisi",
                table: "Puantajlar",
                newName: "ToplamCalismaSaati");

            migrationBuilder.RenameColumn(
                name: "ToplamGecKalmaSuresi",
                table: "Puantajlar",
                newName: "ResmiTatilGunu");

            migrationBuilder.RenameColumn(
                name: "ToplamErkenCikisSuresi",
                table: "Puantajlar",
                newName: "RaporluGun");

            migrationBuilder.RenameColumn(
                name: "ToplamEksikCalismaSuresi",
                table: "Puantajlar",
                newName: "NormalMesaiSaati");

            migrationBuilder.RenameColumn(
                name: "ToplamCalismaSuresi",
                table: "Puantajlar",
                newName: "IzinGunu");

            migrationBuilder.RenameColumn(
                name: "ResmiTatilGunSayisi",
                table: "Puantajlar",
                newName: "HaftaTatiliGunu");

            migrationBuilder.RenameColumn(
                name: "ResmiTatilCalismaSuresi",
                table: "Puantajlar",
                newName: "HaftaSonuMesaiSaati");

            migrationBuilder.RenameColumn(
                name: "NormalMesaiSuresi",
                table: "Puantajlar",
                newName: "GeceMesaiSaati");

            migrationBuilder.RenameColumn(
                name: "MazeretliIzinGunSayisi",
                table: "Puantajlar",
                newName: "GecKalmaSuresi");

            migrationBuilder.RenameColumn(
                name: "IzinGunSayisi",
                table: "Puantajlar",
                newName: "GecKalmaGunu");

            migrationBuilder.RenameColumn(
                name: "HastaTatiliGunSayisi",
                table: "Puantajlar",
                newName: "FazlaMesaiSaati");

            migrationBuilder.RenameColumn(
                name: "HaftaTatiliGunSayisi",
                table: "Puantajlar",
                newName: "ErkenCikisSuresi");

            migrationBuilder.RenameColumn(
                name: "HaftaTatiliCalismaSuresi",
                table: "Puantajlar",
                newName: "ErkenCikisGunu");

            migrationBuilder.RenameColumn(
                name: "GeceMesaiSuresi",
                table: "Puantajlar",
                newName: "EksikCalismaSaati");

            migrationBuilder.RenameColumn(
                name: "GecKalmaGunSayisi",
                table: "Puantajlar",
                newName: "DevamsizlikGunu");

            migrationBuilder.RenameColumn(
                name: "VardiyaBitis",
                table: "PuantajDetaylar",
                newName: "PlanlananGiris");

            migrationBuilder.RenameColumn(
                name: "VardiyaBaslangic",
                table: "PuantajDetaylar",
                newName: "PlanlananCikis");

            migrationBuilder.RenameColumn(
                name: "ToplamMolaSuresi",
                table: "PuantajDetaylar",
                newName: "NormalMesai");

            migrationBuilder.RenameColumn(
                name: "ToplamCalismaSuresi",
                table: "PuantajDetaylar",
                newName: "GirisCikisId");

            migrationBuilder.RenameColumn(
                name: "SonCikis",
                table: "PuantajDetaylar",
                newName: "GerceklesenGiris");

            migrationBuilder.RenameColumn(
                name: "PlanlananCalismaSuresi",
                table: "PuantajDetaylar",
                newName: "GerceklesenSure");

            migrationBuilder.RenameColumn(
                name: "MolaAdedi",
                table: "PuantajDetaylar",
                newName: "GeceMesai");

            migrationBuilder.RenameColumn(
                name: "IzinliMi",
                table: "PuantajDetaylar",
                newName: "ResmiTatilMi");

            migrationBuilder.RenameColumn(
                name: "IzinId",
                table: "PuantajDetaylar",
                newName: "FazlaMesai");

            migrationBuilder.RenameColumn(
                name: "IlkGiris",
                table: "PuantajDetaylar",
                newName: "GerceklesenCikis");

            migrationBuilder.RenameColumn(
                name: "GunTipi",
                table: "PuantajDetaylar",
                newName: "EksikSure");

            migrationBuilder.RenameColumn(
                name: "ElleGirildiMi",
                table: "PuantajDetaylar",
                newName: "HaftaSonuMu");

            migrationBuilder.AlterColumn<string>(
                name: "Notlar",
                table: "Puantajlar",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Durum",
                table: "Puantajlar",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Notlar",
                table: "PuantajDetaylar",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "IzinTuru",
                table: "PuantajDetaylar",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "GunDurumu",
                table: "PuantajDetaylar",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PersonelId",
                table: "PuantajDetaylar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlanlananSure",
                table: "PuantajDetaylar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Puantajlar_PersonelId",
                table: "Puantajlar",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_PuantajDetaylar_GirisCikisId",
                table: "PuantajDetaylar",
                column: "GirisCikisId");

            migrationBuilder.CreateIndex(
                name: "IX_PuantajDetaylar_PersonelId",
                table: "PuantajDetaylar",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_PuantajDetaylar_PuantajId",
                table: "PuantajDetaylar",
                column: "PuantajId");

            migrationBuilder.AddForeignKey(
                name: "FK_PuantajDetaylar_GirisCikislar_GirisCikisId",
                table: "PuantajDetaylar",
                column: "GirisCikisId",
                principalTable: "GirisCikislar",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PuantajDetaylar_Personeller_PersonelId",
                table: "PuantajDetaylar",
                column: "PersonelId",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PuantajDetaylar_Vardiyalar_VardiyaId",
                table: "PuantajDetaylar",
                column: "VardiyaId",
                principalTable: "Vardiyalar",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Puantajlar_Kullanicilar_OnaylayanKullaniciId",
                table: "Puantajlar",
                column: "OnaylayanKullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Puantajlar_sirketler_SirketId",
                table: "Puantajlar",
                column: "SirketId",
                principalTable: "sirketler",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
