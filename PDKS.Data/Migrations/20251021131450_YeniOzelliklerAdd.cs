using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class YeniOzelliklerAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bildirimler_KullaniciId",
                table: "Bildirimler");

            migrationBuilder.AddColumn<int>(
                name: "ReferansId",
                table: "Bildirimler",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferansTip",
                table: "Bildirimler",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Anketler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnketBasligi = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    BaslangicTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Anonim = table.Column<bool>(type: "boolean", nullable: false),
                    Aktif = table.Column<bool>(type: "boolean", nullable: false),
                    HedefKatilimcilar = table.Column<string>(type: "text", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "integer", nullable: false),
                    SirketId = table.Column<int>(type: "integer", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anketler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anketler_Kullanicilar_OlusturanKullaniciId",
                        column: x => x.OlusturanKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Anketler_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Araclar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Plaka = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Marka = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Yil = table.Column<int>(type: "integer", nullable: true),
                    Renk = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    YakitTipi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    GuncelKm = table.Column<int>(type: "integer", nullable: false),
                    SonMuayeneTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    KaskoTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SigortaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Aktif = table.Column<bool>(type: "boolean", nullable: false),
                    TahsisliPersonelId = table.Column<int>(type: "integer", nullable: true),
                    SirketId = table.Column<int>(type: "integer", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Araclar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Araclar_Personeller_TahsisliPersonelId",
                        column: x => x.TahsisliPersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Araclar_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AvansTalepleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    Tutar = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Sebep = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TalepTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OdemeSekli = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TaksitSayisi = table.Column<int>(type: "integer", nullable: true),
                    OnayDurumu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OdemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DekontDosyasi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SirketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvansTalepleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvansTalepleri_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvansTalepleri_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DosyaTalepleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    DosyaAdi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DosyaTipi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Durum = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TalepTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    YuklenenDosya = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    YuklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OnaylayanKullaniciId = table.Column<int>(type: "integer", nullable: true),
                    OnayTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OnayNotu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SirketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosyaTalepleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DosyaTalepleri_Kullanicilar_OnaylayanKullaniciId",
                        column: x => x.OnaylayanKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DosyaTalepleri_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DosyaTalepleri_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Duyurular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Baslik = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Icerik = table.Column<string>(type: "text", nullable: false),
                    Tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BaslangicTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Aktif = table.Column<bool>(type: "boolean", nullable: false),
                    AnaSayfadaGoster = table.Column<bool>(type: "boolean", nullable: false),
                    HedefDepartmanlar = table.Column<string>(type: "text", nullable: true),
                    EkDosyalar = table.Column<string>(type: "text", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "integer", nullable: false),
                    SirketId = table.Column<int>(type: "integer", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duyurular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Duyurular_Kullanicilar_OlusturanKullaniciId",
                        column: x => x.OlusturanKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Duyurular_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Etkinlikler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EtkinlikAdi = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    EtkinlikTipi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Konum = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    KontenjanSayisi = table.Column<int>(type: "integer", nullable: true),
                    KatilimZorunlu = table.Column<bool>(type: "boolean", nullable: false),
                    HedefKatilimcilar = table.Column<string>(type: "text", nullable: true),
                    KapakResmi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DuzenleyenKullaniciId = table.Column<int>(type: "integer", nullable: false),
                    SirketId = table.Column<int>(type: "integer", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etkinlikler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Etkinlikler_Kullanicilar_DuzenleyenKullaniciId",
                        column: x => x.DuzenleyenKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Etkinlikler_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Forumlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ForumAdi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Ikon = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    Aktif = table.Column<bool>(type: "boolean", nullable: false),
                    SirketId = table.Column<int>(type: "integer", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forumlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forumlar_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gorevler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Baslik = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    AtayanPersonelId = table.Column<int>(type: "integer", nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BitisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Oncelik = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Durum = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TamamlanmaYuzdesi = table.Column<int>(type: "integer", nullable: false),
                    Etiketler = table.Column<string>(type: "text", nullable: true),
                    Dosyalar = table.Column<string>(type: "text", nullable: true),
                    UstGorevId = table.Column<int>(type: "integer", nullable: true),
                    SirketId = table.Column<int>(type: "integer", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gorevler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gorevler_Gorevler_UstGorevId",
                        column: x => x.UstGorevId,
                        principalTable: "Gorevler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gorevler_Personeller_AtayanPersonelId",
                        column: x => x.AtayanPersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gorevler_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GorevTanimlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepartmanId = table.Column<int>(type: "integer", nullable: false),
                    GorevAdi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Aciklama = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    GerekliNitelikler = table.Column<string>(type: "text", nullable: true),
                    DeneyimSeviyesi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    MaasAraligiMin = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    MaasAraligiMax = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Aktif = table.Column<bool>(type: "boolean", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SirketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GorevTanimlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GorevTanimlari_Departmanlar_DepartmanId",
                        column: x => x.DepartmanId,
                        principalTable: "Departmanlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GorevTanimlari_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IzinHaklari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    Yil = table.Column<int>(type: "integer", nullable: false),
                    YillikIzinGun = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    KullanilanIzin = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    KalanIzin = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    MazeretiIzin = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    HastalikIzin = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    UcretsizIzin = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    BabalikIzin = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    EvlilikIzin = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    OlumIzin = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    SirketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IzinHaklari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IzinHaklari_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IzinHaklari_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MalzemeTalepleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    MalzemeAdi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Miktar = table.Column<int>(type: "integer", nullable: false),
                    Birim = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OnayDurumu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TalepTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OnayTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TeslimTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TeslimEdenKullaniciId = table.Column<int>(type: "integer", nullable: true),
                    TeslimNotu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SirketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MalzemeTalepleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MalzemeTalepleri_Kullanicilar_TeslimEdenKullaniciId",
                        column: x => x.TeslimEdenKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MalzemeTalepleri_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MalzemeTalepleri_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasrafTalepleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    MasrafTipi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Tutar = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Tarih = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Faturalar = table.Column<string>(type: "text", nullable: true),
                    KdvOrani = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    KdvTutari = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    OnayDurumu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OdemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TalepTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SirketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasrafTalepleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasrafTalepleri_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasrafTalepleri_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MazeretBildirimleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    Tarih = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MazeretTipi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EkDosyalar = table.Column<string>(type: "text", nullable: true),
                    OnayDurumu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OnaylayiciPersonelId = table.Column<int>(type: "integer", nullable: true),
                    OnayTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SirketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MazeretBildirimleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MazeretBildirimleri_Personeller_OnaylayiciPersonelId",
                        column: x => x.OnaylayiciPersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MazeretBildirimleri_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MazeretBildirimleri_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Oneriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    Baslik = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: false),
                    Kategori = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Durum = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Anonim = table.Column<bool>(type: "boolean", nullable: false),
                    EkDosyalar = table.Column<string>(type: "text", nullable: true),
                    DegerlendirmePuani = table.Column<int>(type: "integer", nullable: true),
                    DegerlendirmeNotu = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DegerlendirenKullaniciId = table.Column<int>(type: "integer", nullable: true),
                    DegerlendirmeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OdulMiktari = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    OneriTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SirketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oneriler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Oneriler_Kullanicilar_DegerlendirenKullaniciId",
                        column: x => x.DegerlendirenKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Oneriler_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Oneriler_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeyahatTalepleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    SeyahatTipi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    GidisSehri = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VarisSehri = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UlkeAdi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    KalkisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DonusTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amac = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    KonaklamaGerekli = table.Column<bool>(type: "boolean", nullable: false),
                    UlasimTipi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BeklenenMaliyet = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    OnayDurumu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TalepTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UcakBileti = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OtelRezervasyon = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SirketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeyahatTalepleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeyahatTalepleri_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeyahatTalepleri_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sikayetler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    Baslik = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: false),
                    Kategori = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    OncelikSeviyesi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Durum = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Anonim = table.Column<bool>(type: "boolean", nullable: false),
                    EkDosyalar = table.Column<string>(type: "text", nullable: true),
                    AtananKullaniciId = table.Column<int>(type: "integer", nullable: true),
                    CozumAciklamasi = table.Column<string>(type: "text", nullable: true),
                    CozumTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SikayetTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SirketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sikayetler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sikayetler_Kullanicilar_AtananKullaniciId",
                        column: x => x.AtananKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sikayetler_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sikayetler_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToplantiOdalari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OdaAdi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Kapasite = table.Column<int>(type: "integer", nullable: true),
                    Ozellikler = table.Column<string>(type: "text", nullable: true),
                    Aktif = table.Column<bool>(type: "boolean", nullable: false),
                    SirketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToplantiOdalari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToplantiOdalari_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Zimmetler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    DemirbasAdi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DemirbasKodu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TeslimTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IadeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Durum = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SirketId = table.Column<int>(type: "integer", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zimmetler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zimmetler_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Zimmetler_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnketSorulari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnketId = table.Column<int>(type: "integer", nullable: false),
                    SoruMetni = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SoruTipi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Secenekler = table.Column<string>(type: "text", nullable: true),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    Zorunlu = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnketSorulari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnketSorulari_Anketler_AnketId",
                        column: x => x.AnketId,
                        principalTable: "Anketler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AracTalepleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    AracId = table.Column<int>(type: "integer", nullable: true),
                    GidisSehri = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DonusSehri = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    KalkisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KalkisSaati = table.Column<TimeSpan>(type: "interval", nullable: false),
                    DonusTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DonusSaati = table.Column<TimeSpan>(type: "interval", nullable: false),
                    YolcuSayisi = table.Column<int>(type: "integer", nullable: false),
                    Amac = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OnayDurumu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TalepTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SirketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AracTalepleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AracTalepleri_Araclar_AracId",
                        column: x => x.AracId,
                        principalTable: "Araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AracTalepleri_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AracTalepleri_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EtkinlikKatilimcilari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EtkinlikId = table.Column<int>(type: "integer", nullable: false),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    KatilimDurumu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KatilimTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Not = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtkinlikKatilimcilari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EtkinlikKatilimcilari_Etkinlikler_EtkinlikId",
                        column: x => x.EtkinlikId,
                        principalTable: "Etkinlikler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EtkinlikKatilimcilari_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumKonulari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ForumId = table.Column<int>(type: "integer", nullable: false),
                    OlusturanPersonelId = table.Column<int>(type: "integer", nullable: false),
                    Baslik = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Icerik = table.Column<string>(type: "text", nullable: false),
                    Sabitlenmis = table.Column<bool>(type: "boolean", nullable: false),
                    Kilitli = table.Column<bool>(type: "boolean", nullable: false),
                    GoruntulemeSayisi = table.Column<int>(type: "integer", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumKonulari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumKonulari_Forumlar_ForumId",
                        column: x => x.ForumId,
                        principalTable: "Forumlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumKonulari_Personeller_OlusturanPersonelId",
                        column: x => x.OlusturanPersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GorevAtamalari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GorevId = table.Column<int>(type: "integer", nullable: false),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    AtamaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Tamamlandi = table.Column<bool>(type: "boolean", nullable: false),
                    TamamlanmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GorevAtamalari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GorevAtamalari_Gorevler_GorevId",
                        column: x => x.GorevId,
                        principalTable: "Gorevler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GorevAtamalari_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GorevYorumlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GorevId = table.Column<int>(type: "integer", nullable: false),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    Yorum = table.Column<string>(type: "text", nullable: false),
                    Dosyalar = table.Column<string>(type: "text", nullable: true),
                    YorumTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GorevYorumlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GorevYorumlari_Gorevler_GorevId",
                        column: x => x.GorevId,
                        principalTable: "Gorevler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GorevYorumlari_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeyahatMasraflari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeyahatId = table.Column<int>(type: "integer", nullable: false),
                    MasrafTipi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Tutar = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Tarih = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FaturaDosyasi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KdvOrani = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeyahatMasraflari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeyahatMasraflari_SeyahatTalepleri_SeyahatId",
                        column: x => x.SeyahatId,
                        principalTable: "SeyahatTalepleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToplantiOdasiRezervasyonlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OdaId = table.Column<int>(type: "integer", nullable: false),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Konu = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Katilimcilar = table.Column<string>(type: "text", nullable: true),
                    Durum = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SirketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToplantiOdasiRezervasyonlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToplantiOdasiRezervasyonlari_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ToplantiOdasiRezervasyonlari_ToplantiOdalari_OdaId",
                        column: x => x.OdaId,
                        principalTable: "ToplantiOdalari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToplantiOdasiRezervasyonlari_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnketCevaplari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SoruId = table.Column<int>(type: "integer", nullable: false),
                    PersonelId = table.Column<int>(type: "integer", nullable: true),
                    Cevap = table.Column<string>(type: "text", nullable: true),
                    CevapTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnketCevaplari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnketCevaplari_AnketSorulari_SoruId",
                        column: x => x.SoruId,
                        principalTable: "AnketSorulari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnketCevaplari_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AracKullanimRaporlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AracId = table.Column<int>(type: "integer", nullable: false),
                    AracTalebiId = table.Column<int>(type: "integer", nullable: false),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    BaslangicKm = table.Column<int>(type: "integer", nullable: false),
                    BitisKm = table.Column<int>(type: "integer", nullable: false),
                    YakitTutari = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    DigerGiderler = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Notlar = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FaturaDosyalari = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    BaslangicTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KayitTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AracKullanimRaporlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AracKullanimRaporlari_AracTalepleri_AracTalebiId",
                        column: x => x.AracTalebiId,
                        principalTable: "AracTalepleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AracKullanimRaporlari_Araclar_AracId",
                        column: x => x.AracId,
                        principalTable: "Araclar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AracKullanimRaporlari_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnayAkislari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OnayTipi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReferansId = table.Column<int>(type: "integer", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    OnaylayiciPersonelId = table.Column<int>(type: "integer", nullable: false),
                    OnayDurumu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OnayTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SirketId = table.Column<int>(type: "integer", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AracTalebiId = table.Column<int>(type: "integer", nullable: true),
                    AvansTalebiId = table.Column<int>(type: "integer", nullable: true),
                    DosyaTalebiId = table.Column<int>(type: "integer", nullable: true),
                    MalzemeTalebiId = table.Column<int>(type: "integer", nullable: true),
                    MasrafTalebiId = table.Column<int>(type: "integer", nullable: true),
                    SeyahatTalebiId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnayAkislari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnayAkislari_AracTalepleri_AracTalebiId",
                        column: x => x.AracTalebiId,
                        principalTable: "AracTalepleri",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OnayAkislari_AvansTalepleri_AvansTalebiId",
                        column: x => x.AvansTalebiId,
                        principalTable: "AvansTalepleri",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OnayAkislari_DosyaTalepleri_DosyaTalebiId",
                        column: x => x.DosyaTalebiId,
                        principalTable: "DosyaTalepleri",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OnayAkislari_MalzemeTalepleri_MalzemeTalebiId",
                        column: x => x.MalzemeTalebiId,
                        principalTable: "MalzemeTalepleri",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OnayAkislari_MasrafTalepleri_MasrafTalebiId",
                        column: x => x.MasrafTalebiId,
                        principalTable: "MasrafTalepleri",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OnayAkislari_Personeller_OnaylayiciPersonelId",
                        column: x => x.OnaylayiciPersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OnayAkislari_SeyahatTalepleri_SeyahatTalebiId",
                        column: x => x.SeyahatTalebiId,
                        principalTable: "SeyahatTalepleri",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OnayAkislari_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ForumMesajlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KonuId = table.Column<int>(type: "integer", nullable: false),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    Mesaj = table.Column<string>(type: "text", nullable: false),
                    Dosyalar = table.Column<string>(type: "text", nullable: true),
                    BegeniSayisi = table.Column<int>(type: "integer", nullable: false),
                    MesajTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Silindi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumMesajlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumMesajlari_ForumKonulari_KonuId",
                        column: x => x.KonuId,
                        principalTable: "ForumKonulari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumMesajlari_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bildirimler_KullaniciId_Okundu",
                table: "Bildirimler",
                columns: new[] { "KullaniciId", "Okundu" });

            migrationBuilder.CreateIndex(
                name: "IX_AnketCevaplari_PersonelId",
                table: "AnketCevaplari",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_AnketCevaplari_SoruId",
                table: "AnketCevaplari",
                column: "SoruId");

            migrationBuilder.CreateIndex(
                name: "IX_Anketler_OlusturanKullaniciId",
                table: "Anketler",
                column: "OlusturanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Anketler_SirketId",
                table: "Anketler",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_AnketSorulari_AnketId",
                table: "AnketSorulari",
                column: "AnketId");

            migrationBuilder.CreateIndex(
                name: "IX_AracKullanimRaporlari_AracId",
                table: "AracKullanimRaporlari",
                column: "AracId");

            migrationBuilder.CreateIndex(
                name: "IX_AracKullanimRaporlari_AracTalebiId",
                table: "AracKullanimRaporlari",
                column: "AracTalebiId");

            migrationBuilder.CreateIndex(
                name: "IX_AracKullanimRaporlari_PersonelId",
                table: "AracKullanimRaporlari",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_Araclar_Plaka",
                table: "Araclar",
                column: "Plaka",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Araclar_SirketId",
                table: "Araclar",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_Araclar_TahsisliPersonelId",
                table: "Araclar",
                column: "TahsisliPersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_AracTalepleri_AracId",
                table: "AracTalepleri",
                column: "AracId");

            migrationBuilder.CreateIndex(
                name: "IX_AracTalepleri_PersonelId",
                table: "AracTalepleri",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_AracTalepleri_SirketId",
                table: "AracTalepleri",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_AvansTalepleri_PersonelId",
                table: "AvansTalepleri",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_AvansTalepleri_SirketId",
                table: "AvansTalepleri",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_DosyaTalepleri_OnaylayanKullaniciId",
                table: "DosyaTalepleri",
                column: "OnaylayanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_DosyaTalepleri_PersonelId",
                table: "DosyaTalepleri",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_DosyaTalepleri_SirketId",
                table: "DosyaTalepleri",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_Duyurular_OlusturanKullaniciId",
                table: "Duyurular",
                column: "OlusturanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Duyurular_SirketId",
                table: "Duyurular",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_EtkinlikKatilimcilari_EtkinlikId",
                table: "EtkinlikKatilimcilari",
                column: "EtkinlikId");

            migrationBuilder.CreateIndex(
                name: "IX_EtkinlikKatilimcilari_PersonelId",
                table: "EtkinlikKatilimcilari",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_Etkinlikler_DuzenleyenKullaniciId",
                table: "Etkinlikler",
                column: "DuzenleyenKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Etkinlikler_SirketId",
                table: "Etkinlikler",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumKonulari_ForumId",
                table: "ForumKonulari",
                column: "ForumId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumKonulari_OlusturanPersonelId",
                table: "ForumKonulari",
                column: "OlusturanPersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_Forumlar_SirketId",
                table: "Forumlar",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumMesajlari_KonuId",
                table: "ForumMesajlari",
                column: "KonuId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumMesajlari_PersonelId",
                table: "ForumMesajlari",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_GorevAtamalari_GorevId",
                table: "GorevAtamalari",
                column: "GorevId");

            migrationBuilder.CreateIndex(
                name: "IX_GorevAtamalari_PersonelId",
                table: "GorevAtamalari",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_Gorevler_AtayanPersonelId",
                table: "Gorevler",
                column: "AtayanPersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_Gorevler_SirketId",
                table: "Gorevler",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_Gorevler_UstGorevId",
                table: "Gorevler",
                column: "UstGorevId");

            migrationBuilder.CreateIndex(
                name: "IX_GorevTanimlari_DepartmanId",
                table: "GorevTanimlari",
                column: "DepartmanId");

            migrationBuilder.CreateIndex(
                name: "IX_GorevTanimlari_SirketId",
                table: "GorevTanimlari",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_GorevYorumlari_GorevId",
                table: "GorevYorumlari",
                column: "GorevId");

            migrationBuilder.CreateIndex(
                name: "IX_GorevYorumlari_PersonelId",
                table: "GorevYorumlari",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_IzinHaklari_PersonelId",
                table: "IzinHaklari",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_IzinHaklari_SirketId",
                table: "IzinHaklari",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_MalzemeTalepleri_PersonelId",
                table: "MalzemeTalepleri",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_MalzemeTalepleri_SirketId",
                table: "MalzemeTalepleri",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_MalzemeTalepleri_TeslimEdenKullaniciId",
                table: "MalzemeTalepleri",
                column: "TeslimEdenKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_MasrafTalepleri_PersonelId",
                table: "MasrafTalepleri",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_MasrafTalepleri_SirketId",
                table: "MasrafTalepleri",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_MazeretBildirimleri_OnaylayiciPersonelId",
                table: "MazeretBildirimleri",
                column: "OnaylayiciPersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_MazeretBildirimleri_PersonelId",
                table: "MazeretBildirimleri",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_MazeretBildirimleri_SirketId",
                table: "MazeretBildirimleri",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayAkislari_AracTalebiId",
                table: "OnayAkislari",
                column: "AracTalebiId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayAkislari_AvansTalebiId",
                table: "OnayAkislari",
                column: "AvansTalebiId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayAkislari_DosyaTalebiId",
                table: "OnayAkislari",
                column: "DosyaTalebiId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayAkislari_MalzemeTalebiId",
                table: "OnayAkislari",
                column: "MalzemeTalebiId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayAkislari_MasrafTalebiId",
                table: "OnayAkislari",
                column: "MasrafTalebiId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayAkislari_OnaylayiciPersonelId",
                table: "OnayAkislari",
                column: "OnaylayiciPersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayAkislari_OnayTipi_ReferansId_Sira",
                table: "OnayAkislari",
                columns: new[] { "OnayTipi", "ReferansId", "Sira" });

            migrationBuilder.CreateIndex(
                name: "IX_OnayAkislari_SeyahatTalebiId",
                table: "OnayAkislari",
                column: "SeyahatTalebiId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayAkislari_SirketId",
                table: "OnayAkislari",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_Oneriler_DegerlendirenKullaniciId",
                table: "Oneriler",
                column: "DegerlendirenKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Oneriler_PersonelId",
                table: "Oneriler",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_Oneriler_SirketId",
                table: "Oneriler",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_SeyahatMasraflari_SeyahatId",
                table: "SeyahatMasraflari",
                column: "SeyahatId");

            migrationBuilder.CreateIndex(
                name: "IX_SeyahatTalepleri_PersonelId",
                table: "SeyahatTalepleri",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_SeyahatTalepleri_SirketId",
                table: "SeyahatTalepleri",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_Sikayetler_AtananKullaniciId",
                table: "Sikayetler",
                column: "AtananKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Sikayetler_PersonelId",
                table: "Sikayetler",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_Sikayetler_SirketId",
                table: "Sikayetler",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_ToplantiOdalari_SirketId",
                table: "ToplantiOdalari",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_ToplantiOdasiRezervasyonlari_OdaId_BaslangicTarihi_BitisTar~",
                table: "ToplantiOdasiRezervasyonlari",
                columns: new[] { "OdaId", "BaslangicTarihi", "BitisTarihi" });

            migrationBuilder.CreateIndex(
                name: "IX_ToplantiOdasiRezervasyonlari_PersonelId",
                table: "ToplantiOdasiRezervasyonlari",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_ToplantiOdasiRezervasyonlari_SirketId",
                table: "ToplantiOdasiRezervasyonlari",
                column: "SirketId");

            migrationBuilder.CreateIndex(
                name: "IX_Zimmetler_PersonelId",
                table: "Zimmetler",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_Zimmetler_SirketId",
                table: "Zimmetler",
                column: "SirketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnketCevaplari");

            migrationBuilder.DropTable(
                name: "AracKullanimRaporlari");

            migrationBuilder.DropTable(
                name: "Duyurular");

            migrationBuilder.DropTable(
                name: "EtkinlikKatilimcilari");

            migrationBuilder.DropTable(
                name: "ForumMesajlari");

            migrationBuilder.DropTable(
                name: "GorevAtamalari");

            migrationBuilder.DropTable(
                name: "GorevTanimlari");

            migrationBuilder.DropTable(
                name: "GorevYorumlari");

            migrationBuilder.DropTable(
                name: "IzinHaklari");

            migrationBuilder.DropTable(
                name: "MazeretBildirimleri");

            migrationBuilder.DropTable(
                name: "OnayAkislari");

            migrationBuilder.DropTable(
                name: "Oneriler");

            migrationBuilder.DropTable(
                name: "SeyahatMasraflari");

            migrationBuilder.DropTable(
                name: "Sikayetler");

            migrationBuilder.DropTable(
                name: "ToplantiOdasiRezervasyonlari");

            migrationBuilder.DropTable(
                name: "Zimmetler");

            migrationBuilder.DropTable(
                name: "AnketSorulari");

            migrationBuilder.DropTable(
                name: "Etkinlikler");

            migrationBuilder.DropTable(
                name: "ForumKonulari");

            migrationBuilder.DropTable(
                name: "Gorevler");

            migrationBuilder.DropTable(
                name: "AracTalepleri");

            migrationBuilder.DropTable(
                name: "AvansTalepleri");

            migrationBuilder.DropTable(
                name: "DosyaTalepleri");

            migrationBuilder.DropTable(
                name: "MalzemeTalepleri");

            migrationBuilder.DropTable(
                name: "MasrafTalepleri");

            migrationBuilder.DropTable(
                name: "SeyahatTalepleri");

            migrationBuilder.DropTable(
                name: "ToplantiOdalari");

            migrationBuilder.DropTable(
                name: "Anketler");

            migrationBuilder.DropTable(
                name: "Forumlar");

            migrationBuilder.DropTable(
                name: "Araclar");

            migrationBuilder.DropIndex(
                name: "IX_Bildirimler_KullaniciId_Okundu",
                table: "Bildirimler");

            migrationBuilder.DropColumn(
                name: "ReferansId",
                table: "Bildirimler");

            migrationBuilder.DropColumn(
                name: "ReferansTip",
                table: "Bildirimler");

            migrationBuilder.CreateIndex(
                name: "IX_Bildirimler_KullaniciId",
                table: "Bildirimler",
                column: "KullaniciId");
        }
    }
}
