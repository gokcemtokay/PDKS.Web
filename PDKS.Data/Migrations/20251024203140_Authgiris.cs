using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class Authgiris : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kullanicilar_Personeller_PersonelId",
                table: "Kullanicilar");

            migrationBuilder.DropForeignKey(
                name: "FK_Kullanicilar_Roller_RolId",
                table: "Kullanicilar");

            migrationBuilder.RenameColumn(
                name: "SonGirisTarihi",
                table: "Kullanicilar",
                newName: "songiristarihi");

            migrationBuilder.RenameColumn(
                name: "SifreHash",
                table: "Kullanicilar",
                newName: "sifrehash");

            migrationBuilder.RenameColumn(
                name: "Sifre",
                table: "Kullanicilar",
                newName: "sifre");

            migrationBuilder.RenameColumn(
                name: "RolId",
                table: "Kullanicilar",
                newName: "rolid");

            migrationBuilder.RenameColumn(
                name: "PersonelId",
                table: "Kullanicilar",
                newName: "personelid");

            migrationBuilder.RenameColumn(
                name: "OlusturmaTarihi",
                table: "Kullanicilar",
                newName: "olusturmatarihi");

            migrationBuilder.RenameColumn(
                name: "KullaniciAdi",
                table: "Kullanicilar",
                newName: "kullaniciadi");

            migrationBuilder.RenameColumn(
                name: "KayitTarihi",
                table: "Kullanicilar",
                newName: "kayittarihi");

            migrationBuilder.RenameColumn(
                name: "GuncellemeTarihi",
                table: "Kullanicilar",
                newName: "guncellemeTarihi");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Kullanicilar",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Aktif",
                table: "Kullanicilar",
                newName: "aktif");

            migrationBuilder.RenameIndex(
                name: "IX_Kullanicilar_RolId",
                table: "Kullanicilar",
                newName: "IX_Kullanicilar_rolid");

            migrationBuilder.RenameIndex(
                name: "IX_Kullanicilar_PersonelId",
                table: "Kullanicilar",
                newName: "IX_Kullanicilar_personelid");

            migrationBuilder.RenameIndex(
                name: "IX_Kullanicilar_Email",
                table: "Kullanicilar",
                newName: "IX_Kullanicilar_email");

            migrationBuilder.AddColumn<string>(
                name: "IsTanimi",
                table: "PersonelIsDeneyimi",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notlar",
                table: "PersonelIsDeneyimi",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferansKisiAdi",
                table: "PersonelIsDeneyimi",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferansKisiEmail",
                table: "PersonelIsDeneyimi",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferansKisiTelefon",
                table: "PersonelIsDeneyimi",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SGKTescilliMi",
                table: "PersonelIsDeneyimi",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ad",
                table: "Kullanicilar",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "soyad",
                table: "Kullanicilar",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Kullanicilar_Personeller_personelid",
                table: "Kullanicilar",
                column: "personelid",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Kullanicilar_Roller_rolid",
                table: "Kullanicilar",
                column: "rolid",
                principalTable: "Roller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kullanicilar_Personeller_personelid",
                table: "Kullanicilar");

            migrationBuilder.DropForeignKey(
                name: "FK_Kullanicilar_Roller_rolid",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "IsTanimi",
                table: "PersonelIsDeneyimi");

            migrationBuilder.DropColumn(
                name: "Notlar",
                table: "PersonelIsDeneyimi");

            migrationBuilder.DropColumn(
                name: "ReferansKisiAdi",
                table: "PersonelIsDeneyimi");

            migrationBuilder.DropColumn(
                name: "ReferansKisiEmail",
                table: "PersonelIsDeneyimi");

            migrationBuilder.DropColumn(
                name: "ReferansKisiTelefon",
                table: "PersonelIsDeneyimi");

            migrationBuilder.DropColumn(
                name: "SGKTescilliMi",
                table: "PersonelIsDeneyimi");

            migrationBuilder.DropColumn(
                name: "ad",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "soyad",
                table: "Kullanicilar");

            migrationBuilder.RenameColumn(
                name: "songiristarihi",
                table: "Kullanicilar",
                newName: "SonGirisTarihi");

            migrationBuilder.RenameColumn(
                name: "sifrehash",
                table: "Kullanicilar",
                newName: "SifreHash");

            migrationBuilder.RenameColumn(
                name: "sifre",
                table: "Kullanicilar",
                newName: "Sifre");

            migrationBuilder.RenameColumn(
                name: "rolid",
                table: "Kullanicilar",
                newName: "RolId");

            migrationBuilder.RenameColumn(
                name: "personelid",
                table: "Kullanicilar",
                newName: "PersonelId");

            migrationBuilder.RenameColumn(
                name: "olusturmatarihi",
                table: "Kullanicilar",
                newName: "OlusturmaTarihi");

            migrationBuilder.RenameColumn(
                name: "kullaniciadi",
                table: "Kullanicilar",
                newName: "KullaniciAdi");

            migrationBuilder.RenameColumn(
                name: "kayittarihi",
                table: "Kullanicilar",
                newName: "KayitTarihi");

            migrationBuilder.RenameColumn(
                name: "guncellemeTarihi",
                table: "Kullanicilar",
                newName: "GuncellemeTarihi");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Kullanicilar",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "aktif",
                table: "Kullanicilar",
                newName: "Aktif");

            migrationBuilder.RenameIndex(
                name: "IX_Kullanicilar_rolid",
                table: "Kullanicilar",
                newName: "IX_Kullanicilar_RolId");

            migrationBuilder.RenameIndex(
                name: "IX_Kullanicilar_personelid",
                table: "Kullanicilar",
                newName: "IX_Kullanicilar_PersonelId");

            migrationBuilder.RenameIndex(
                name: "IX_Kullanicilar_email",
                table: "Kullanicilar",
                newName: "IX_Kullanicilar_Email");

            migrationBuilder.AddForeignKey(
                name: "FK_Kullanicilar_Personeller_PersonelId",
                table: "Kullanicilar",
                column: "PersonelId",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Kullanicilar_Roller_RolId",
                table: "Kullanicilar",
                column: "RolId",
                principalTable: "Roller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
