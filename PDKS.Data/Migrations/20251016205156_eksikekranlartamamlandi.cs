using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class eksikekranlartamamlandi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_avanslar_kullanicilar_onaylayan_id",
                table: "avanslar");

            migrationBuilder.DropForeignKey(
                name: "FK_avanslar_personeller_personel_id",
                table: "avanslar");

            migrationBuilder.DropForeignKey(
                name: "FK_bildirimler_kullanicilar_kullanici_id",
                table: "bildirimler");

            migrationBuilder.DropForeignKey(
                name: "FK_giris_cikislar_cihazlar_cihaz_id",
                table: "giris_cikislar");

            migrationBuilder.DropForeignKey(
                name: "FK_giris_cikislar_personeller_personel_id",
                table: "giris_cikislar");

            migrationBuilder.DropForeignKey(
                name: "FK_izinler_kullanicilar_onaylayan_id",
                table: "izinler");

            migrationBuilder.DropForeignKey(
                name: "FK_izinler_personeller_personel_id",
                table: "izinler");

            migrationBuilder.DropForeignKey(
                name: "FK_kullanicilar_personeller_personel_id",
                table: "kullanicilar");

            migrationBuilder.DropForeignKey(
                name: "FK_kullanicilar_roller_rol_id",
                table: "kullanicilar");

            migrationBuilder.DropForeignKey(
                name: "FK_loglar_kullanicilar_kullanici_id",
                table: "loglar");

            migrationBuilder.DropForeignKey(
                name: "FK_personeller_Vardiyalar_vardiya_id",
                table: "personeller");

            migrationBuilder.DropForeignKey(
                name: "FK_primler_personeller_personel_id",
                table: "primler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roller",
                table: "roller");

            migrationBuilder.DropPrimaryKey(
                name: "PK_primler",
                table: "primler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_personeller",
                table: "personeller");

            migrationBuilder.DropPrimaryKey(
                name: "PK_parametreler",
                table: "parametreler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kullanicilar",
                table: "kullanicilar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bildirimler",
                table: "bildirimler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_avanslar",
                table: "avanslar");

            migrationBuilder.DropIndex(
                name: "IX_avanslar_onaylayan_id",
                table: "avanslar");

            migrationBuilder.DropColumn(
                name: "departman",
                table: "personeller");

            migrationBuilder.RenameTable(
                name: "roller",
                newName: "Roller");

            migrationBuilder.RenameTable(
                name: "primler",
                newName: "Primler");

            migrationBuilder.RenameTable(
                name: "personeller",
                newName: "Personeller");

            migrationBuilder.RenameTable(
                name: "parametreler",
                newName: "Parametreler");

            migrationBuilder.RenameTable(
                name: "kullanicilar",
                newName: "Kullanicilar");

            migrationBuilder.RenameTable(
                name: "bildirimler",
                newName: "Bildirimler");

            migrationBuilder.RenameTable(
                name: "avanslar",
                newName: "Avanslar");

            migrationBuilder.RenameColumn(
                name: "durum",
                table: "Vardiyalar",
                newName: "Durum");

            migrationBuilder.RenameColumn(
                name: "ad",
                table: "Vardiyalar",
                newName: "Ad");

            migrationBuilder.RenameColumn(
                name: "aciklama",
                table: "Vardiyalar",
                newName: "Aciklama");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Vardiyalar",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "tolerans_suresi_dakika",
                table: "Vardiyalar",
                newName: "ToleransSuresiDakika");

            migrationBuilder.RenameColumn(
                name: "gece_vardiyasi_mi",
                table: "Vardiyalar",
                newName: "GeceVardiyasiMi");

            migrationBuilder.RenameColumn(
                name: "esnek_vardiya_mi",
                table: "Vardiyalar",
                newName: "EsnekVardiyaMi");

            migrationBuilder.RenameColumn(
                name: "bitis_saati",
                table: "Vardiyalar",
                newName: "BitisSaati");

            migrationBuilder.RenameColumn(
                name: "baslangic_saati",
                table: "Vardiyalar",
                newName: "BaslangicSaati");

            migrationBuilder.RenameColumn(
                name: "aciklama",
                table: "Roller",
                newName: "Aciklama");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Roller",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "rol_adi",
                table: "Roller",
                newName: "RolAdi");

            migrationBuilder.RenameColumn(
                name: "tutar",
                table: "Primler",
                newName: "Tutar");

            migrationBuilder.RenameColumn(
                name: "donem",
                table: "Primler",
                newName: "Donem");

            migrationBuilder.RenameColumn(
                name: "aciklama",
                table: "Primler",
                newName: "Aciklama");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Primler",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "prim_tipi",
                table: "Primler",
                newName: "PrimTipi");

            migrationBuilder.RenameColumn(
                name: "personel_id",
                table: "Primler",
                newName: "PersonelId");

            migrationBuilder.RenameColumn(
                name: "olusturma_tarihi",
                table: "Primler",
                newName: "Tarih");

            migrationBuilder.RenameIndex(
                name: "IX_primler_personel_id",
                table: "Primler",
                newName: "IX_Primler_PersonelId");

            migrationBuilder.RenameColumn(
                name: "telefon",
                table: "Personeller",
                newName: "Telefon");

            migrationBuilder.RenameColumn(
                name: "maas",
                table: "Personeller",
                newName: "Maas");

            migrationBuilder.RenameColumn(
                name: "gorev",
                table: "Personeller",
                newName: "Gorev");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Personeller",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "durum",
                table: "Personeller",
                newName: "Durum");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Personeller",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "vardiya_id",
                table: "Personeller",
                newName: "VardiyaId");

            migrationBuilder.RenameColumn(
                name: "sicil_no",
                table: "Personeller",
                newName: "SicilNo");

            migrationBuilder.RenameColumn(
                name: "olusturma_tarihi",
                table: "Personeller",
                newName: "OlusturmaTarihi");

            migrationBuilder.RenameColumn(
                name: "guncelleme_tarihi",
                table: "Personeller",
                newName: "GuncellemeTarihi");

            migrationBuilder.RenameColumn(
                name: "giris_tarihi",
                table: "Personeller",
                newName: "GirisTarihi");

            migrationBuilder.RenameColumn(
                name: "cikis_tarihi",
                table: "Personeller",
                newName: "CikisTarihi");

            migrationBuilder.RenameColumn(
                name: "avans_limiti",
                table: "Personeller",
                newName: "AvansLimiti");

            migrationBuilder.RenameColumn(
                name: "ad_soyad",
                table: "Personeller",
                newName: "AdSoyad");

            migrationBuilder.RenameIndex(
                name: "IX_personeller_email",
                table: "Personeller",
                newName: "IX_Personeller_Email");

            migrationBuilder.RenameIndex(
                name: "IX_personeller_vardiya_id",
                table: "Personeller",
                newName: "IX_Personeller_VardiyaId");

            migrationBuilder.RenameIndex(
                name: "IX_personeller_sicil_no",
                table: "Personeller",
                newName: "IX_Personeller_SicilNo");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Parametreler",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "anahtar",
                table: "Parametreler",
                newName: "Ad");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Kullanicilar",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "aktif",
                table: "Kullanicilar",
                newName: "Aktif");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Kullanicilar",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "son_giris_tarihi",
                table: "Kullanicilar",
                newName: "SonGirisTarihi");

            migrationBuilder.RenameColumn(
                name: "sifre_hash",
                table: "Kullanicilar",
                newName: "SifreHash");

            migrationBuilder.RenameColumn(
                name: "rol_id",
                table: "Kullanicilar",
                newName: "RolId");

            migrationBuilder.RenameColumn(
                name: "personel_id",
                table: "Kullanicilar",
                newName: "PersonelId");

            migrationBuilder.RenameColumn(
                name: "olusturma_tarihi",
                table: "Kullanicilar",
                newName: "OlusturmaTarihi");

            migrationBuilder.RenameIndex(
                name: "IX_kullanicilar_email",
                table: "Kullanicilar",
                newName: "IX_Kullanicilar_Email");

            migrationBuilder.RenameIndex(
                name: "IX_kullanicilar_rol_id",
                table: "Kullanicilar",
                newName: "IX_Kullanicilar_RolId");

            migrationBuilder.RenameIndex(
                name: "IX_kullanicilar_personel_id",
                table: "Kullanicilar",
                newName: "IX_Kullanicilar_PersonelId");

            migrationBuilder.RenameColumn(
                name: "tip",
                table: "Bildirimler",
                newName: "Tip");

            migrationBuilder.RenameColumn(
                name: "okundu",
                table: "Bildirimler",
                newName: "Okundu");

            migrationBuilder.RenameColumn(
                name: "mesaj",
                table: "Bildirimler",
                newName: "Mesaj");

            migrationBuilder.RenameColumn(
                name: "baslik",
                table: "Bildirimler",
                newName: "Baslik");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Bildirimler",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "olusturma_tarihi",
                table: "Bildirimler",
                newName: "OlusturmaTarihi");

            migrationBuilder.RenameColumn(
                name: "kullanici_id",
                table: "Bildirimler",
                newName: "KullaniciId");

            migrationBuilder.RenameIndex(
                name: "IX_bildirimler_kullanici_id",
                table: "Bildirimler",
                newName: "IX_Bildirimler_KullaniciId");

            migrationBuilder.RenameColumn(
                name: "tutar",
                table: "Avanslar",
                newName: "Tutar");

            migrationBuilder.RenameColumn(
                name: "durum",
                table: "Avanslar",
                newName: "Durum");

            migrationBuilder.RenameColumn(
                name: "aciklama",
                table: "Avanslar",
                newName: "Aciklama");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Avanslar",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "talep_tarihi",
                table: "Avanslar",
                newName: "TalepTarihi");

            migrationBuilder.RenameColumn(
                name: "personel_id",
                table: "Avanslar",
                newName: "PersonelId");

            migrationBuilder.RenameColumn(
                name: "odeme_tarihi",
                table: "Avanslar",
                newName: "OdemeTarihi");

            migrationBuilder.RenameColumn(
                name: "onaylayan_id",
                table: "Avanslar",
                newName: "TaksitSayisi");

            migrationBuilder.RenameIndex(
                name: "IX_avanslar_personel_id",
                table: "Avanslar",
                newName: "IX_Avanslar_PersonelId");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "Vardiyalar",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "Roller",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<decimal>(
                name: "Tutar",
                table: "Primler",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Donem",
                table: "Primler",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "Primler",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "PrimTipi",
                table: "Primler",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "Ay",
                table: "Primler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "KayitTarihi",
                table: "Primler",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "OdendiMi",
                table: "Primler",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OnayDurumu",
                table: "Primler",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "OnayTarihi",
                table: "Primler",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OnaylayanKullaniciId",
                table: "Primler",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimAdi",
                table: "Primler",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Yil",
                table: "Primler",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Telefon",
                table: "Personeller",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<decimal>(
                name: "Maas",
                table: "Personeller",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Gorev",
                table: "Personeller",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OlusturmaTarihi",
                table: "Personeller",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<decimal>(
                name: "AvansLimiti",
                table: "Personeller",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<string>(
                name: "Adres",
                table: "Personeller",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cinsiyet",
                table: "Personeller",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmanId",
                table: "Personeller",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DogumTarihi",
                table: "Personeller",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "KanGrubu",
                table: "Personeller",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "KayitTarihi",
                table: "Personeller",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notlar",
                table: "Personeller",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TcKimlikNo",
                table: "Personeller",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Unvan",
                table: "Personeller",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Deger",
                table: "Parametreler",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "Parametreler",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Birim",
                table: "Parametreler",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GuncellemeTarihi",
                table: "Parametreler",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kategori",
                table: "Parametreler",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "KayitTarihi",
                table: "Parametreler",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "SifreHash",
                table: "Kullanicilar",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "PersonelId",
                table: "Kullanicilar",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OlusturmaTarihi",
                table: "Kullanicilar",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<DateTime>(
                name: "GuncellemeTarihi",
                table: "Kullanicilar",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "KayitTarihi",
                table: "Kullanicilar",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "KullaniciAdi",
                table: "Kullanicilar",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sifre",
                table: "Kullanicilar",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CihazId1",
                table: "giris_cikislar",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CihazId1",
                table: "cihaz_loglari",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Tip",
                table: "Bildirimler",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Mesaj",
                table: "Bildirimler",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Bildirimler",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OkunmaTarihi",
                table: "Bildirimler",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Tutar",
                table: "Avanslar",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "Avanslar",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<decimal>(
                name: "KalanBorc",
                table: "Avanslar",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "KayitTarihi",
                table: "Avanslar",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "OdendiMi",
                table: "Avanslar",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OnayDurumu",
                table: "Avanslar",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "OnayTarihi",
                table: "Avanslar",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OnaylayanKullaniciId",
                table: "Avanslar",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedNedeni",
                table: "Avanslar",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Tarih",
                table: "Avanslar",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roller",
                table: "Roller",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Primler",
                table: "Primler",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Personeller",
                table: "Personeller",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Parametreler",
                table: "Parametreler",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Kullanicilar",
                table: "Kullanicilar",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bildirimler",
                table: "Bildirimler",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Avanslar",
                table: "Avanslar",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Departmanlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Kod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UstDepartmanId = table.Column<int>(type: "integer", nullable: true),
                    Durum = table.Column<bool>(type: "boolean", nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departmanlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departmanlar_Departmanlar_UstDepartmanId",
                        column: x => x.UstDepartmanId,
                        principalTable: "Departmanlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mesailer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    Tarih = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BaslangicSaati = table.Column<TimeSpan>(type: "interval", nullable: false),
                    BitisSaati = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ToplamSaat = table.Column<decimal>(type: "numeric", nullable: false),
                    FazlaMesaiSaati = table.Column<decimal>(type: "numeric", nullable: false),
                    HaftaSonuMesaiSaati = table.Column<decimal>(type: "numeric", nullable: false),
                    ResmiTatilMesaiSaati = table.Column<decimal>(type: "numeric", nullable: false),
                    MesaiTipi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OnayDurumu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OnaylayanKullaniciId = table.Column<int>(type: "integer", nullable: true),
                    OnayTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RedNedeni = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesailer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mesailer_Kullanicilar_OnaylayanKullaniciId",
                        column: x => x.OnaylayanKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mesailer_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Parametreler",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Aciklama", "Ad", "Birim", "Deger", "GuncellemeTarihi", "Kategori", "KayitTarihi" },
                values: new object[] { "Haftalık standart çalışma günü sayısı", "Haftalık Çalışma Günü", "gün", "5", null, "Çalışma Saatleri", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Parametreler",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Aciklama", "Ad", "Birim", "Deger", "GuncellemeTarihi", "Kategori", "KayitTarihi" },
                values: new object[] { "Günlük standart çalışma saati", "Günlük Çalışma Saati", "saat", "8", null, "Çalışma Saatleri", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Parametreler",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Aciklama", "Ad", "Birim", "GuncellemeTarihi", "Kategori", "KayitTarihi" },
                values: new object[] { "İşe geç kalma tolerans süresi", "Geç Kalma Toleransı", "dakika", null, "Toleranslar", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Parametreler",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Aciklama", "Ad", "Birim", "Deger", "GuncellemeTarihi", "Kategori", "KayitTarihi" },
                values: new object[] { "Erken çıkış tolerans süresi", "Erken Çıkış Toleransı", "dakika", "15", null, "Toleranslar", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Parametreler",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Aciklama", "Ad", "Birim", "Deger", "GuncellemeTarihi", "Kategori", "KayitTarihi" },
                values: new object[] { "Fazla mesai ücreti çarpanı", "Fazla Mesai Çarpanı", "kat", "1.5", null, "Mesai", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Parametreler",
                columns: new[] { "Id", "Aciklama", "Ad", "Birim", "Deger", "GuncellemeTarihi", "Kategori", "KayitTarihi" },
                values: new object[,]
                {
                    { 6, "Hafta sonu mesai ücreti çarpanı", "Hafta Sonu Mesai Çarpanı", "kat", "2", null, "Mesai", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, "Yıllık ücretli izin günü sayısı", "Yıllık İzin Günü", "gün", "14", null, "İzin", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tatiller_tarih",
                table: "tatiller",
                column: "tarih",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Primler_OnaylayanKullaniciId",
                table: "Primler",
                column: "OnaylayanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Personeller_DepartmanId",
                table: "Personeller",
                column: "DepartmanId");

            migrationBuilder.CreateIndex(
                name: "IX_Parametreler_Ad",
                table: "Parametreler",
                column: "Ad",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_giris_cikislar_CihazId1",
                table: "giris_cikislar",
                column: "CihazId1");

            migrationBuilder.CreateIndex(
                name: "IX_cihaz_loglari_CihazId1",
                table: "cihaz_loglari",
                column: "CihazId1");

            migrationBuilder.CreateIndex(
                name: "IX_Avanslar_OnaylayanKullaniciId",
                table: "Avanslar",
                column: "OnaylayanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Departmanlar_UstDepartmanId",
                table: "Departmanlar",
                column: "UstDepartmanId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesailer_OnaylayanKullaniciId",
                table: "Mesailer",
                column: "OnaylayanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Mesailer_PersonelId",
                table: "Mesailer",
                column: "PersonelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Avanslar_Kullanicilar_OnaylayanKullaniciId",
                table: "Avanslar",
                column: "OnaylayanKullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Avanslar_Personeller_PersonelId",
                table: "Avanslar",
                column: "PersonelId",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bildirimler_Kullanicilar_KullaniciId",
                table: "Bildirimler",
                column: "KullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cihaz_loglari_cihazlar_CihazId1",
                table: "cihaz_loglari",
                column: "CihazId1",
                principalTable: "cihazlar",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_giris_cikislar_Personeller_personel_id",
                table: "giris_cikislar",
                column: "personel_id",
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

            migrationBuilder.AddForeignKey(
                name: "FK_loglar_Kullanicilar_kullanici_id",
                table: "loglar",
                column: "kullanici_id",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Personeller_Departmanlar_DepartmanId",
                table: "Personeller",
                column: "DepartmanId",
                principalTable: "Departmanlar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Personeller_Vardiyalar_VardiyaId",
                table: "Personeller",
                column: "VardiyaId",
                principalTable: "Vardiyalar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Primler_Kullanicilar_OnaylayanKullaniciId",
                table: "Primler",
                column: "OnaylayanKullaniciId",
                principalTable: "Kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Primler_Personeller_PersonelId",
                table: "Primler",
                column: "PersonelId",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avanslar_Kullanicilar_OnaylayanKullaniciId",
                table: "Avanslar");

            migrationBuilder.DropForeignKey(
                name: "FK_Avanslar_Personeller_PersonelId",
                table: "Avanslar");

            migrationBuilder.DropForeignKey(
                name: "FK_Bildirimler_Kullanicilar_KullaniciId",
                table: "Bildirimler");

            migrationBuilder.DropForeignKey(
                name: "FK_cihaz_loglari_cihazlar_CihazId1",
                table: "cihaz_loglari");

            migrationBuilder.DropForeignKey(
                name: "FK_giris_cikislar_Personeller_personel_id",
                table: "giris_cikislar");

            migrationBuilder.DropForeignKey(
                name: "FK_giris_cikislar_cihazlar_CihazId1",
                table: "giris_cikislar");

            migrationBuilder.DropForeignKey(
                name: "FK_giris_cikislar_cihazlar_cihaz_id",
                table: "giris_cikislar");

            migrationBuilder.DropForeignKey(
                name: "FK_izinler_Kullanicilar_onaylayan_id",
                table: "izinler");

            migrationBuilder.DropForeignKey(
                name: "FK_izinler_Personeller_personel_id",
                table: "izinler");

            migrationBuilder.DropForeignKey(
                name: "FK_Kullanicilar_Personeller_PersonelId",
                table: "Kullanicilar");

            migrationBuilder.DropForeignKey(
                name: "FK_Kullanicilar_Roller_RolId",
                table: "Kullanicilar");

            migrationBuilder.DropForeignKey(
                name: "FK_loglar_Kullanicilar_kullanici_id",
                table: "loglar");

            migrationBuilder.DropForeignKey(
                name: "FK_Personeller_Departmanlar_DepartmanId",
                table: "Personeller");

            migrationBuilder.DropForeignKey(
                name: "FK_Personeller_Vardiyalar_VardiyaId",
                table: "Personeller");

            migrationBuilder.DropForeignKey(
                name: "FK_Primler_Kullanicilar_OnaylayanKullaniciId",
                table: "Primler");

            migrationBuilder.DropForeignKey(
                name: "FK_Primler_Personeller_PersonelId",
                table: "Primler");

            migrationBuilder.DropTable(
                name: "Departmanlar");

            migrationBuilder.DropTable(
                name: "Mesailer");

            migrationBuilder.DropIndex(
                name: "IX_tatiller_tarih",
                table: "tatiller");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roller",
                table: "Roller");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Primler",
                table: "Primler");

            migrationBuilder.DropIndex(
                name: "IX_Primler_OnaylayanKullaniciId",
                table: "Primler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Personeller",
                table: "Personeller");

            migrationBuilder.DropIndex(
                name: "IX_Personeller_DepartmanId",
                table: "Personeller");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Parametreler",
                table: "Parametreler");

            migrationBuilder.DropIndex(
                name: "IX_Parametreler_Ad",
                table: "Parametreler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Kullanicilar",
                table: "Kullanicilar");

            migrationBuilder.DropIndex(
                name: "IX_giris_cikislar_CihazId1",
                table: "giris_cikislar");

            migrationBuilder.DropIndex(
                name: "IX_cihaz_loglari_CihazId1",
                table: "cihaz_loglari");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bildirimler",
                table: "Bildirimler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Avanslar",
                table: "Avanslar");

            migrationBuilder.DropIndex(
                name: "IX_Avanslar_OnaylayanKullaniciId",
                table: "Avanslar");

            migrationBuilder.DeleteData(
                table: "Parametreler",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Parametreler",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "Ay",
                table: "Primler");

            migrationBuilder.DropColumn(
                name: "KayitTarihi",
                table: "Primler");

            migrationBuilder.DropColumn(
                name: "OdendiMi",
                table: "Primler");

            migrationBuilder.DropColumn(
                name: "OnayDurumu",
                table: "Primler");

            migrationBuilder.DropColumn(
                name: "OnayTarihi",
                table: "Primler");

            migrationBuilder.DropColumn(
                name: "OnaylayanKullaniciId",
                table: "Primler");

            migrationBuilder.DropColumn(
                name: "PrimAdi",
                table: "Primler");

            migrationBuilder.DropColumn(
                name: "Yil",
                table: "Primler");

            migrationBuilder.DropColumn(
                name: "Adres",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "Cinsiyet",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "DepartmanId",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "DogumTarihi",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "KanGrubu",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "KayitTarihi",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "Notlar",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "TcKimlikNo",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "Unvan",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "Birim",
                table: "Parametreler");

            migrationBuilder.DropColumn(
                name: "GuncellemeTarihi",
                table: "Parametreler");

            migrationBuilder.DropColumn(
                name: "Kategori",
                table: "Parametreler");

            migrationBuilder.DropColumn(
                name: "KayitTarihi",
                table: "Parametreler");

            migrationBuilder.DropColumn(
                name: "GuncellemeTarihi",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "KayitTarihi",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "KullaniciAdi",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "Sifre",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "CihazId1",
                table: "giris_cikislar");

            migrationBuilder.DropColumn(
                name: "CihazId1",
                table: "cihaz_loglari");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Bildirimler");

            migrationBuilder.DropColumn(
                name: "OkunmaTarihi",
                table: "Bildirimler");

            migrationBuilder.DropColumn(
                name: "KalanBorc",
                table: "Avanslar");

            migrationBuilder.DropColumn(
                name: "KayitTarihi",
                table: "Avanslar");

            migrationBuilder.DropColumn(
                name: "OdendiMi",
                table: "Avanslar");

            migrationBuilder.DropColumn(
                name: "OnayDurumu",
                table: "Avanslar");

            migrationBuilder.DropColumn(
                name: "OnayTarihi",
                table: "Avanslar");

            migrationBuilder.DropColumn(
                name: "OnaylayanKullaniciId",
                table: "Avanslar");

            migrationBuilder.DropColumn(
                name: "RedNedeni",
                table: "Avanslar");

            migrationBuilder.DropColumn(
                name: "Tarih",
                table: "Avanslar");

            migrationBuilder.RenameTable(
                name: "Roller",
                newName: "roller");

            migrationBuilder.RenameTable(
                name: "Primler",
                newName: "primler");

            migrationBuilder.RenameTable(
                name: "Personeller",
                newName: "personeller");

            migrationBuilder.RenameTable(
                name: "Parametreler",
                newName: "parametreler");

            migrationBuilder.RenameTable(
                name: "Kullanicilar",
                newName: "kullanicilar");

            migrationBuilder.RenameTable(
                name: "Bildirimler",
                newName: "bildirimler");

            migrationBuilder.RenameTable(
                name: "Avanslar",
                newName: "avanslar");

            migrationBuilder.RenameColumn(
                name: "Durum",
                table: "Vardiyalar",
                newName: "durum");

            migrationBuilder.RenameColumn(
                name: "Ad",
                table: "Vardiyalar",
                newName: "ad");

            migrationBuilder.RenameColumn(
                name: "Aciklama",
                table: "Vardiyalar",
                newName: "aciklama");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Vardiyalar",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ToleransSuresiDakika",
                table: "Vardiyalar",
                newName: "tolerans_suresi_dakika");

            migrationBuilder.RenameColumn(
                name: "GeceVardiyasiMi",
                table: "Vardiyalar",
                newName: "gece_vardiyasi_mi");

            migrationBuilder.RenameColumn(
                name: "EsnekVardiyaMi",
                table: "Vardiyalar",
                newName: "esnek_vardiya_mi");

            migrationBuilder.RenameColumn(
                name: "BitisSaati",
                table: "Vardiyalar",
                newName: "bitis_saati");

            migrationBuilder.RenameColumn(
                name: "BaslangicSaati",
                table: "Vardiyalar",
                newName: "baslangic_saati");

            migrationBuilder.RenameColumn(
                name: "Aciklama",
                table: "roller",
                newName: "aciklama");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "roller",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RolAdi",
                table: "roller",
                newName: "rol_adi");

            migrationBuilder.RenameColumn(
                name: "Tutar",
                table: "primler",
                newName: "tutar");

            migrationBuilder.RenameColumn(
                name: "Donem",
                table: "primler",
                newName: "donem");

            migrationBuilder.RenameColumn(
                name: "Aciklama",
                table: "primler",
                newName: "aciklama");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "primler",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PrimTipi",
                table: "primler",
                newName: "prim_tipi");

            migrationBuilder.RenameColumn(
                name: "PersonelId",
                table: "primler",
                newName: "personel_id");

            migrationBuilder.RenameColumn(
                name: "Tarih",
                table: "primler",
                newName: "olusturma_tarihi");

            migrationBuilder.RenameIndex(
                name: "IX_Primler_PersonelId",
                table: "primler",
                newName: "IX_primler_personel_id");

            migrationBuilder.RenameColumn(
                name: "Telefon",
                table: "personeller",
                newName: "telefon");

            migrationBuilder.RenameColumn(
                name: "Maas",
                table: "personeller",
                newName: "maas");

            migrationBuilder.RenameColumn(
                name: "Gorev",
                table: "personeller",
                newName: "gorev");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "personeller",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Durum",
                table: "personeller",
                newName: "durum");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "personeller",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VardiyaId",
                table: "personeller",
                newName: "vardiya_id");

            migrationBuilder.RenameColumn(
                name: "SicilNo",
                table: "personeller",
                newName: "sicil_no");

            migrationBuilder.RenameColumn(
                name: "OlusturmaTarihi",
                table: "personeller",
                newName: "olusturma_tarihi");

            migrationBuilder.RenameColumn(
                name: "GuncellemeTarihi",
                table: "personeller",
                newName: "guncelleme_tarihi");

            migrationBuilder.RenameColumn(
                name: "GirisTarihi",
                table: "personeller",
                newName: "giris_tarihi");

            migrationBuilder.RenameColumn(
                name: "CikisTarihi",
                table: "personeller",
                newName: "cikis_tarihi");

            migrationBuilder.RenameColumn(
                name: "AvansLimiti",
                table: "personeller",
                newName: "avans_limiti");

            migrationBuilder.RenameColumn(
                name: "AdSoyad",
                table: "personeller",
                newName: "ad_soyad");

            migrationBuilder.RenameIndex(
                name: "IX_Personeller_Email",
                table: "personeller",
                newName: "IX_personeller_email");

            migrationBuilder.RenameIndex(
                name: "IX_Personeller_VardiyaId",
                table: "personeller",
                newName: "IX_personeller_vardiya_id");

            migrationBuilder.RenameIndex(
                name: "IX_Personeller_SicilNo",
                table: "personeller",
                newName: "IX_personeller_sicil_no");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "parametreler",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Ad",
                table: "parametreler",
                newName: "anahtar");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "kullanicilar",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Aktif",
                table: "kullanicilar",
                newName: "aktif");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "kullanicilar",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SonGirisTarihi",
                table: "kullanicilar",
                newName: "son_giris_tarihi");

            migrationBuilder.RenameColumn(
                name: "SifreHash",
                table: "kullanicilar",
                newName: "sifre_hash");

            migrationBuilder.RenameColumn(
                name: "RolId",
                table: "kullanicilar",
                newName: "rol_id");

            migrationBuilder.RenameColumn(
                name: "PersonelId",
                table: "kullanicilar",
                newName: "personel_id");

            migrationBuilder.RenameColumn(
                name: "OlusturmaTarihi",
                table: "kullanicilar",
                newName: "olusturma_tarihi");

            migrationBuilder.RenameIndex(
                name: "IX_Kullanicilar_Email",
                table: "kullanicilar",
                newName: "IX_kullanicilar_email");

            migrationBuilder.RenameIndex(
                name: "IX_Kullanicilar_RolId",
                table: "kullanicilar",
                newName: "IX_kullanicilar_rol_id");

            migrationBuilder.RenameIndex(
                name: "IX_Kullanicilar_PersonelId",
                table: "kullanicilar",
                newName: "IX_kullanicilar_personel_id");

            migrationBuilder.RenameColumn(
                name: "Tip",
                table: "bildirimler",
                newName: "tip");

            migrationBuilder.RenameColumn(
                name: "Okundu",
                table: "bildirimler",
                newName: "okundu");

            migrationBuilder.RenameColumn(
                name: "Mesaj",
                table: "bildirimler",
                newName: "mesaj");

            migrationBuilder.RenameColumn(
                name: "Baslik",
                table: "bildirimler",
                newName: "baslik");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "bildirimler",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "OlusturmaTarihi",
                table: "bildirimler",
                newName: "olusturma_tarihi");

            migrationBuilder.RenameColumn(
                name: "KullaniciId",
                table: "bildirimler",
                newName: "kullanici_id");

            migrationBuilder.RenameIndex(
                name: "IX_Bildirimler_KullaniciId",
                table: "bildirimler",
                newName: "IX_bildirimler_kullanici_id");

            migrationBuilder.RenameColumn(
                name: "Tutar",
                table: "avanslar",
                newName: "tutar");

            migrationBuilder.RenameColumn(
                name: "Durum",
                table: "avanslar",
                newName: "durum");

            migrationBuilder.RenameColumn(
                name: "Aciklama",
                table: "avanslar",
                newName: "aciklama");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "avanslar",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TalepTarihi",
                table: "avanslar",
                newName: "talep_tarihi");

            migrationBuilder.RenameColumn(
                name: "PersonelId",
                table: "avanslar",
                newName: "personel_id");

            migrationBuilder.RenameColumn(
                name: "OdemeTarihi",
                table: "avanslar",
                newName: "odeme_tarihi");

            migrationBuilder.RenameColumn(
                name: "TaksitSayisi",
                table: "avanslar",
                newName: "onaylayan_id");

            migrationBuilder.RenameIndex(
                name: "IX_Avanslar_PersonelId",
                table: "avanslar",
                newName: "IX_avanslar_personel_id");

            migrationBuilder.AlterColumn<string>(
                name: "aciklama",
                table: "Vardiyalar",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "aciklama",
                table: "roller",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "tutar",
                table: "primler",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "donem",
                table: "primler",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "aciklama",
                table: "primler",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "prim_tipi",
                table: "primler",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "telefon",
                table: "personeller",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "maas",
                table: "personeller",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gorev",
                table: "personeller",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "olusturma_tarihi",
                table: "personeller",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "avans_limiti",
                table: "personeller",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "departman",
                table: "personeller",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Deger",
                table: "parametreler",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "parametreler",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "sifre_hash",
                table: "kullanicilar",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "personel_id",
                table: "kullanicilar",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "olusturma_tarihi",
                table: "kullanicilar",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "tip",
                table: "bildirimler",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mesaj",
                table: "bildirimler",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "tutar",
                table: "avanslar",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "aciklama",
                table: "avanslar",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_roller",
                table: "roller",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_primler",
                table: "primler",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_personeller",
                table: "personeller",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_parametreler",
                table: "parametreler",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_kullanicilar",
                table: "kullanicilar",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bildirimler",
                table: "bildirimler",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_avanslar",
                table: "avanslar",
                column: "id");

            migrationBuilder.UpdateData(
                table: "parametreler",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "Aciklama", "anahtar", "Deger" },
                values: new object[] { "Fazla mesai ücret oranı", "FazlaMesaiOrani", "1.5" });

            migrationBuilder.UpdateData(
                table: "parametreler",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "Aciklama", "anahtar", "Deger" },
                values: new object[] { "Geç kalma tolerans süresi (dakika)", "GecKalmaToleransDakika", "15" });

            migrationBuilder.UpdateData(
                table: "parametreler",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "Aciklama", "anahtar" },
                values: new object[] { "Erken çıkış tolerans süresi (dakika)", "ErkenCikisToLeransDakika" });

            migrationBuilder.UpdateData(
                table: "parametreler",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "Aciklama", "anahtar", "Deger" },
                values: new object[] { "Haftalık çalışma günü sayısı", "HaftalikcalismaGun", "5" });

            migrationBuilder.UpdateData(
                table: "parametreler",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "Aciklama", "anahtar", "Deger" },
                values: new object[] { "Günlük çalışma saati", "GunlukCalismaSaat", "8" });

            migrationBuilder.CreateIndex(
                name: "IX_avanslar_onaylayan_id",
                table: "avanslar",
                column: "onaylayan_id");

            migrationBuilder.AddForeignKey(
                name: "FK_avanslar_kullanicilar_onaylayan_id",
                table: "avanslar",
                column: "onaylayan_id",
                principalTable: "kullanicilar",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_avanslar_personeller_personel_id",
                table: "avanslar",
                column: "personel_id",
                principalTable: "personeller",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bildirimler_kullanicilar_kullanici_id",
                table: "bildirimler",
                column: "kullanici_id",
                principalTable: "kullanicilar",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_giris_cikislar_cihazlar_cihaz_id",
                table: "giris_cikislar",
                column: "cihaz_id",
                principalTable: "cihazlar",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_giris_cikislar_personeller_personel_id",
                table: "giris_cikislar",
                column: "personel_id",
                principalTable: "personeller",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_izinler_kullanicilar_onaylayan_id",
                table: "izinler",
                column: "onaylayan_id",
                principalTable: "kullanicilar",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_izinler_personeller_personel_id",
                table: "izinler",
                column: "personel_id",
                principalTable: "personeller",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_kullanicilar_personeller_personel_id",
                table: "kullanicilar",
                column: "personel_id",
                principalTable: "personeller",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_kullanicilar_roller_rol_id",
                table: "kullanicilar",
                column: "rol_id",
                principalTable: "roller",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_loglar_kullanicilar_kullanici_id",
                table: "loglar",
                column: "kullanici_id",
                principalTable: "kullanicilar",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_personeller_Vardiyalar_vardiya_id",
                table: "personeller",
                column: "vardiya_id",
                principalTable: "Vardiyalar",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_primler_personeller_personel_id",
                table: "primler",
                column: "personel_id",
                principalTable: "personeller",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
