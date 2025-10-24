using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class KapsamliPersonelYonetimi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProfilResmi",
                table: "Personeller",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CalismaLokasyonu",
                table: "Personeller",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CalismaModeli",
                table: "Personeller",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IkinciAmirPersonelId",
                table: "Personeller",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YoneticiPersonelId",
                table: "Personeller",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PersonelAcilDurum",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    IletisimTipi = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AdSoyad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    YakinlikDerecesi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Telefon1 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Telefon2 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    Adres = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelAcilDurum", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelAcilDurum_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelAile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    YakinlikDerecesi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AdSoyad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TcKimlikNo = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    DogumTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Meslek = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CalisiyorMu = table.Column<bool>(type: "boolean", nullable: false),
                    Telefon = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    OgrenciMi = table.Column<bool>(type: "boolean", nullable: false),
                    SGKBagimlisi = table.Column<bool>(type: "boolean", nullable: false),
                    Notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelAile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelAile_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelDil",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    DilAdi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Seviye = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    OkumaSeviyesi = table.Column<int>(type: "integer", nullable: false),
                    YazmaSeviyesi = table.Column<int>(type: "integer", nullable: false),
                    KonusmaSeviyesi = table.Column<int>(type: "integer", nullable: false),
                    SertifikaTuru = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SertifikaPuani = table.Column<int>(type: "integer", nullable: true),
                    SertifikaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SertifikaDosyasi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelDil", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelDil_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelDisiplin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    DisiplinTuru = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OlayTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Aciklama = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    UygulananCeza = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    KararVerenYetkiliId = table.Column<int>(type: "integer", nullable: false),
                    IlgiliDokumanlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Durum = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IptalTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IptalNedeni = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IptalEdenKullaniciId = table.Column<int>(type: "integer", nullable: true),
                    Notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelDisiplin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelDisiplin_Kullanicilar_IptalEdenKullaniciId",
                        column: x => x.IptalEdenKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelDisiplin_Kullanicilar_KararVerenYetkiliId",
                        column: x => x.KararVerenYetkiliId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelDisiplin_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelEgitim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    EgitimSeviyesi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OkulAdi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Bolum = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    BaslangicYili = table.Column<int>(type: "integer", nullable: false),
                    BitisYili = table.Column<int>(type: "integer", nullable: true),
                    MezuniyetDurumu = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    MezuniyetNotu = table.Column<decimal>(type: "numeric", nullable: true),
                    DiplomaDosyasi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelEgitim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelEgitim_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelEgitimKayit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    EgitimAdi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EgitmenKurum = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    EgitimTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EgitimSuresiSaat = table.Column<int>(type: "integer", nullable: true),
                    TamamlanmaDurumu = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    EgitimMaliyeti = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    EgitimSertifikasi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SertifikaAldiMi = table.Column<bool>(type: "boolean", nullable: false),
                    EgitimTuru = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EgitimKategorisi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EgitimIcerigi = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DegerlendirmePuani = table.Column<int>(type: "integer", nullable: true),
                    PersonelGeribildirimi = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    EkDosyalar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelEgitimKayit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelEgitimKayit_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelEkBilgi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    MedeniDurum = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    AskerlikDurumu = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    AskerlikBaslangicTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AskerlikBitisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AskerlikYeri = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AskerlikRutbesi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EhliyetSiniflari = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EhliyetAlisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EhliyetGecerlilikTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Uyruk = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IkametIli = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IkametIlce = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IkametAdresi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DogumYeri = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AnneAdi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BabaAdi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CocukSayisi = table.Column<int>(type: "integer", nullable: true),
                    HobiIlgiAlanlari = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SosyalGuvence = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SigortaliMi = table.Column<bool>(type: "boolean", nullable: false),
                    SigortaSirketi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SigortaPoliceNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelEkBilgi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelEkBilgi_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelIsDeneyimi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    SirketAdi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Pozisyon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Departman = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Sektor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BaslangicTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HalenCalisiyor = table.Column<bool>(type: "boolean", nullable: false),
                    IsAciklamasi = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Basarilar = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    AyrilmaNedeni = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ReferansKisi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ReferansTelefon = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    BelgeDosyaYolu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelIsDeneyimi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelIsDeneyimi_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelMaliBilgi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    BankaAdi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IBAN = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    HesapTuru = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    VergiNo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    VergiDairesi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SGKNo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    SGKBaslangicTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AsgariUcretMuafiyeti = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GelirVergisiOrani = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    EmekliSandigi = table.Column<bool>(type: "boolean", nullable: false),
                    OdemeYontemi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelMaliBilgi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelMaliBilgi_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelPerformans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    DegerlendirmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Donem = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DegerlendiriciKullaniciId = table.Column<int>(type: "integer", nullable: false),
                    PerformansNotu = table.Column<decimal>(type: "numeric", nullable: false),
                    NotSkalasi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Hedefler = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    HedefBasariOrani = table.Column<decimal>(type: "numeric", nullable: true),
                    GucluYonler = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    GelisimAlanlari = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Yorumlar = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    AksiyonPlanlari = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Durum = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    OnaylayanKullaniciId = table.Column<int>(type: "integer", nullable: true),
                    OnayTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EkDosyalar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelPerformans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelPerformans_Kullanicilar_DegerlendiriciKullaniciId",
                        column: x => x.DegerlendiriciKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelPerformans_Kullanicilar_OnaylayanKullaniciId",
                        column: x => x.OnaylayanKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelPerformans_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelReferans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    AdSoyad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SirketKurum = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Pozisyon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Iliski = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Telefon = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notlar = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelReferans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelReferans_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelSaglik",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    KanGrubu = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Boy = table.Column<int>(type: "integer", nullable: true),
                    Kilo = table.Column<decimal>(type: "numeric", nullable: true),
                    KronikHastaliklar = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Alerjiler = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SurekliKullanilanIlaclar = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    EngelDurumuVarMi = table.Column<bool>(type: "boolean", nullable: false),
                    EngelYuzdesi = table.Column<int>(type: "integer", nullable: true),
                    EngelAciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SaglikRaporlari = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SonPeriyodikMuayeneTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SonradakiPeriyodikMuayeneTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsGuvenligiEgitimiTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelSaglik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelSaglik_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelSertifika",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    SertifikaAdi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    VerenKurum = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AlimTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GecerlilikTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SureliMi = table.Column<bool>(type: "boolean", nullable: false),
                    SertifikaNumarasi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SertifikaDosyasi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Durum = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    HatirlatmaGonderildiMi = table.Column<bool>(type: "boolean", nullable: false),
                    HatirlatmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelSertifika", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelSertifika_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelTerfi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    TerfiTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EskiPozisyon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    YeniPozisyon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EskiUnvan = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    YeniUnvan = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EskiDepartmanId = table.Column<int>(type: "integer", nullable: true),
                    YeniDepartmanId = table.Column<int>(type: "integer", nullable: true),
                    TerfiNedeni = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    OnaylayanKullaniciId = table.Column<int>(type: "integer", nullable: false),
                    OnayTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EkDosyalar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelTerfi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelTerfi_Departmanlar_EskiDepartmanId",
                        column: x => x.EskiDepartmanId,
                        principalTable: "Departmanlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelTerfi_Departmanlar_YeniDepartmanId",
                        column: x => x.YeniDepartmanId,
                        principalTable: "Departmanlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelTerfi_Kullanicilar_OnaylayanKullaniciId",
                        column: x => x.OnaylayanKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelTerfi_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelUcretDegisiklik",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    DegisiklikTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EskiMaas = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    YeniMaas = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DegisimYuzdesi = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    FarkTutari = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DegisimNedeni = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Aciklama = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    OnaylayanKullaniciId = table.Column<int>(type: "integer", nullable: false),
                    OnayTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EkDosyalar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelUcretDegisiklik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelUcretDegisiklik_Kullanicilar_OnaylayanKullaniciId",
                        column: x => x.OnaylayanKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelUcretDegisiklik_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelYetkinlik",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    YetkinlikTipi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    YetkinlikAdi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Seviye = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    SeviyePuani = table.Column<int>(type: "integer", nullable: true),
                    SonKullanimTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SelfAssessment = table.Column<bool>(type: "boolean", nullable: false),
                    DegerlendiriciKullaniciId = table.Column<int>(type: "integer", nullable: true),
                    DegerlendirmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BelgelendirenDokumanlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelYetkinlik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelYetkinlik_Kullanicilar_DegerlendiriciKullaniciId",
                        column: x => x.DegerlendiriciKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelYetkinlik_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelZimmet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    EsyaTipi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EsyaAdi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    MarkaModel = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SeriNumarasi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ZimmetTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IadeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ZimmetDurumu = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Degeri = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    ZimmetSozlesmesi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ZimmetFotografi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Aciklama = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ZimmetVerenKullaniciId = table.Column<int>(type: "integer", nullable: false),
                    IadeTeslimAlanKullaniciId = table.Column<int>(type: "integer", nullable: true),
                    Notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelZimmet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonelZimmet_Kullanicilar_IadeTeslimAlanKullaniciId",
                        column: x => x.IadeTeslimAlanKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelZimmet_Kullanicilar_ZimmetVerenKullaniciId",
                        column: x => x.ZimmetVerenKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonelZimmet_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personeller_IkinciAmirPersonelId",
                table: "Personeller",
                column: "IkinciAmirPersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_Personeller_YoneticiPersonelId",
                table: "Personeller",
                column: "YoneticiPersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelAcilDurum_PersonelId",
                table: "PersonelAcilDurum",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelAile_PersonelId",
                table: "PersonelAile",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelDil_PersonelId",
                table: "PersonelDil",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelDisiplin_IptalEdenKullaniciId",
                table: "PersonelDisiplin",
                column: "IptalEdenKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelDisiplin_KararVerenYetkiliId",
                table: "PersonelDisiplin",
                column: "KararVerenYetkiliId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelDisiplin_PersonelId",
                table: "PersonelDisiplin",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelEgitim_PersonelId",
                table: "PersonelEgitim",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelEgitimKayit_PersonelId",
                table: "PersonelEgitimKayit",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelEkBilgi_PersonelId",
                table: "PersonelEkBilgi",
                column: "PersonelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonelIsDeneyimi_PersonelId",
                table: "PersonelIsDeneyimi",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelMaliBilgi_PersonelId",
                table: "PersonelMaliBilgi",
                column: "PersonelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonelPerformans_DegerlendiriciKullaniciId",
                table: "PersonelPerformans",
                column: "DegerlendiriciKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelPerformans_OnaylayanKullaniciId",
                table: "PersonelPerformans",
                column: "OnaylayanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelPerformans_PersonelId_DegerlendirmeTarihi",
                table: "PersonelPerformans",
                columns: new[] { "PersonelId", "DegerlendirmeTarihi" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonelReferans_PersonelId",
                table: "PersonelReferans",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelSaglik_PersonelId",
                table: "PersonelSaglik",
                column: "PersonelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonelSertifika_Durum_GecerlilikTarihi",
                table: "PersonelSertifika",
                columns: new[] { "Durum", "GecerlilikTarihi" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonelSertifika_PersonelId",
                table: "PersonelSertifika",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelSertifika_SertifikaNumarasi",
                table: "PersonelSertifika",
                column: "SertifikaNumarasi");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelTerfi_EskiDepartmanId",
                table: "PersonelTerfi",
                column: "EskiDepartmanId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelTerfi_OnaylayanKullaniciId",
                table: "PersonelTerfi",
                column: "OnaylayanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelTerfi_PersonelId_TerfiTarihi",
                table: "PersonelTerfi",
                columns: new[] { "PersonelId", "TerfiTarihi" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonelTerfi_YeniDepartmanId",
                table: "PersonelTerfi",
                column: "YeniDepartmanId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelUcretDegisiklik_OnaylayanKullaniciId",
                table: "PersonelUcretDegisiklik",
                column: "OnaylayanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelUcretDegisiklik_PersonelId_DegisiklikTarihi",
                table: "PersonelUcretDegisiklik",
                columns: new[] { "PersonelId", "DegisiklikTarihi" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonelYetkinlik_DegerlendiriciKullaniciId",
                table: "PersonelYetkinlik",
                column: "DegerlendiriciKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelYetkinlik_PersonelId",
                table: "PersonelYetkinlik",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelZimmet_IadeTeslimAlanKullaniciId",
                table: "PersonelZimmet",
                column: "IadeTeslimAlanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonelZimmet_PersonelId_ZimmetDurumu",
                table: "PersonelZimmet",
                columns: new[] { "PersonelId", "ZimmetDurumu" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonelZimmet_ZimmetVerenKullaniciId",
                table: "PersonelZimmet",
                column: "ZimmetVerenKullaniciId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personeller_Personeller_IkinciAmirPersonelId",
                table: "Personeller",
                column: "IkinciAmirPersonelId",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Personeller_Personeller_YoneticiPersonelId",
                table: "Personeller",
                column: "YoneticiPersonelId",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personeller_Personeller_IkinciAmirPersonelId",
                table: "Personeller");

            migrationBuilder.DropForeignKey(
                name: "FK_Personeller_Personeller_YoneticiPersonelId",
                table: "Personeller");

            migrationBuilder.DropTable(
                name: "PersonelAcilDurum");

            migrationBuilder.DropTable(
                name: "PersonelAile");

            migrationBuilder.DropTable(
                name: "PersonelDil");

            migrationBuilder.DropTable(
                name: "PersonelDisiplin");

            migrationBuilder.DropTable(
                name: "PersonelEgitim");

            migrationBuilder.DropTable(
                name: "PersonelEgitimKayit");

            migrationBuilder.DropTable(
                name: "PersonelEkBilgi");

            migrationBuilder.DropTable(
                name: "PersonelIsDeneyimi");

            migrationBuilder.DropTable(
                name: "PersonelMaliBilgi");

            migrationBuilder.DropTable(
                name: "PersonelPerformans");

            migrationBuilder.DropTable(
                name: "PersonelReferans");

            migrationBuilder.DropTable(
                name: "PersonelSaglik");

            migrationBuilder.DropTable(
                name: "PersonelSertifika");

            migrationBuilder.DropTable(
                name: "PersonelTerfi");

            migrationBuilder.DropTable(
                name: "PersonelUcretDegisiklik");

            migrationBuilder.DropTable(
                name: "PersonelYetkinlik");

            migrationBuilder.DropTable(
                name: "PersonelZimmet");

            migrationBuilder.DropIndex(
                name: "IX_Personeller_IkinciAmirPersonelId",
                table: "Personeller");

            migrationBuilder.DropIndex(
                name: "IX_Personeller_YoneticiPersonelId",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "CalismaLokasyonu",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "CalismaModeli",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "IkinciAmirPersonelId",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "YoneticiPersonelId",
                table: "Personeller");

            migrationBuilder.AlterColumn<string>(
                name: "ProfilResmi",
                table: "Personeller",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
