using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cihazlar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cihaz_adi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ip_adres = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    lokasyon = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false),
                    son_baglanti_zamani = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cihazlar", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "parametreler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    anahtar = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Deger = table.Column<string>(type: "text", nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parametreler", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roller",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rol_adi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roller", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tatiller",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    tarih = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    resmi_tatil = table.Column<bool>(type: "boolean", nullable: false),
                    aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tatiller", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Vardiyalar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    baslangic_saati = table.Column<TimeSpan>(type: "interval", nullable: false),
                    bitis_saati = table.Column<TimeSpan>(type: "interval", nullable: false),
                    gece_vardiyasi_mi = table.Column<bool>(type: "boolean", nullable: false),
                    esnek_vardiya_mi = table.Column<bool>(type: "boolean", nullable: false),
                    tolerans_suresi_dakika = table.Column<int>(type: "integer", nullable: false),
                    aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vardiyalar", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cihaz_loglari",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cihaz_id = table.Column<int>(type: "integer", nullable: false),
                    islem = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    basarili = table.Column<bool>(type: "boolean", nullable: false),
                    detay = table.Column<string>(type: "text", nullable: false),
                    tarih = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cihaz_loglari", x => x.id);
                    table.ForeignKey(
                        name: "FK_cihaz_loglari_cihazlar_cihaz_id",
                        column: x => x.cihaz_id,
                        principalTable: "cihazlar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "personeller",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad_soyad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    sicil_no = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    departman = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    gorev = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    telefon = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    durum = table.Column<bool>(type: "boolean", nullable: false),
                    giris_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    cikis_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    vardiya_id = table.Column<int>(type: "integer", nullable: true),
                    maas = table.Column<decimal>(type: "numeric", nullable: false),
                    avans_limiti = table.Column<decimal>(type: "numeric", nullable: false),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personeller", x => x.id);
                    table.ForeignKey(
                        name: "FK_personeller_Vardiyalar_vardiya_id",
                        column: x => x.vardiya_id,
                        principalTable: "Vardiyalar",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "giris_cikislar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    personel_id = table.Column<int>(type: "integer", nullable: false),
                    giris_zamani = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cikis_zamani = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    kaynak = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cihaz_id = table.Column<int>(type: "integer", nullable: true),
                    fazla_mesai_suresi = table.Column<int>(type: "integer", nullable: true),
                    gec_kalma_suresi = table.Column<int>(type: "integer", nullable: true),
                    erken_cikis_suresi = table.Column<int>(type: "integer", nullable: true),
                    durum = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    elle_giris = table.Column<bool>(type: "boolean", nullable: false),
                    not = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_giris_cikislar", x => x.id);
                    table.ForeignKey(
                        name: "FK_giris_cikislar_cihazlar_cihaz_id",
                        column: x => x.cihaz_id,
                        principalTable: "cihazlar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_giris_cikislar_personeller_personel_id",
                        column: x => x.personel_id,
                        principalTable: "personeller",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "kullanicilar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    personel_id = table.Column<int>(type: "integer", nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    sifre_hash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    rol_id = table.Column<int>(type: "integer", nullable: false),
                    aktif = table.Column<bool>(type: "boolean", nullable: false),
                    son_giris_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kullanicilar", x => x.id);
                    table.ForeignKey(
                        name: "FK_kullanicilar_personeller_personel_id",
                        column: x => x.personel_id,
                        principalTable: "personeller",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_kullanicilar_roller_rol_id",
                        column: x => x.rol_id,
                        principalTable: "roller",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "primler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    personel_id = table.Column<int>(type: "integer", nullable: false),
                    donem = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    tutar = table.Column<decimal>(type: "numeric", nullable: false),
                    prim_tipi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_primler", x => x.id);
                    table.ForeignKey(
                        name: "FK_primler_personeller_personel_id",
                        column: x => x.personel_id,
                        principalTable: "personeller",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "avanslar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    personel_id = table.Column<int>(type: "integer", nullable: false),
                    tutar = table.Column<decimal>(type: "numeric", nullable: false),
                    talep_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    odeme_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    durum = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    onaylayan_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_avanslar", x => x.id);
                    table.ForeignKey(
                        name: "FK_avanslar_kullanicilar_onaylayan_id",
                        column: x => x.onaylayan_id,
                        principalTable: "kullanicilar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_avanslar_personeller_personel_id",
                        column: x => x.personel_id,
                        principalTable: "personeller",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bildirimler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kullanici_id = table.Column<int>(type: "integer", nullable: false),
                    baslik = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    mesaj = table.Column<string>(type: "text", nullable: false),
                    tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    okundu = table.Column<bool>(type: "boolean", nullable: false),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bildirimler", x => x.id);
                    table.ForeignKey(
                        name: "FK_bildirimler_kullanicilar_kullanici_id",
                        column: x => x.kullanici_id,
                        principalTable: "kullanicilar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "izinler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    personel_id = table.Column<int>(type: "integer", nullable: false),
                    izin_tipi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    baslangic_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    bitis_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    gun_sayisi = table.Column<int>(type: "integer", nullable: false),
                    aciklama = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    onay_durumu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    onaylayan_id = table.Column<int>(type: "integer", nullable: true),
                    onay_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    red_nedeni = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_izinler", x => x.id);
                    table.ForeignKey(
                        name: "FK_izinler_kullanicilar_onaylayan_id",
                        column: x => x.onaylayan_id,
                        principalTable: "kullanicilar",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_izinler_personeller_personel_id",
                        column: x => x.personel_id,
                        principalTable: "personeller",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "loglar",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    kullanici_id = table.Column<int>(type: "integer", nullable: true),
                    islem = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    modul = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    detay = table.Column<string>(type: "text", nullable: false),
                    ip_adres = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    tarih = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loglar", x => x.id);
                    table.ForeignKey(
                        name: "FK_loglar_kullanicilar_kullanici_id",
                        column: x => x.kullanici_id,
                        principalTable: "kullanicilar",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "Vardiyalar",
                columns: new[] { "id", "aciklama", "ad", "baslangic_saati", "bitis_saati", "durum", "esnek_vardiya_mi", "gece_vardiyasi_mi", "tolerans_suresi_dakika" },
                values: new object[,]
                {
                    { 1, "Standart gündüz vardiyası 08:00-17:00", "Gündüz Vardiyası", new TimeSpan(0, 8, 0, 0, 0), new TimeSpan(0, 17, 0, 0, 0), true, false, false, 15 },
                    { 2, "Gece vardiyası 20:00-05:00", "Gece Vardiyası", new TimeSpan(0, 20, 0, 0, 0), new TimeSpan(0, 5, 0, 0, 0), true, false, true, 15 },
                    { 3, "Esnek çalışma saatleri", "Esnek Çalışma", new TimeSpan(0, 9, 0, 0, 0), new TimeSpan(0, 18, 0, 0, 0), true, true, false, 30 }
                });

            migrationBuilder.InsertData(
                table: "parametreler",
                columns: new[] { "id", "Aciklama", "anahtar", "Deger" },
                values: new object[,]
                {
                    { 1, "Fazla mesai ücret oranı", "FazlaMesaiOrani", "1.5" },
                    { 2, "Geç kalma tolerans süresi (dakika)", "GecKalmaToleransDakika", "15" },
                    { 3, "Erken çıkış tolerans süresi (dakika)", "ErkenCikisToLeransDakika", "15" },
                    { 4, "Haftalık çalışma günü sayısı", "HaftalikcalismaGun", "5" },
                    { 5, "Günlük çalışma saati", "GunlukCalismaSaat", "8" }
                });

            migrationBuilder.InsertData(
                table: "roller",
                columns: new[] { "id", "aciklama", "rol_adi" },
                values: new object[,]
                {
                    { 1, "Sistem Yöneticisi", "Admin" },
                    { 2, "İnsan Kaynakları", "IK" },
                    { 3, "Departman Yöneticisi", "Yönetici" },
                    { 4, "Çalışan", "Personel" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_avanslar_onaylayan_id",
                table: "avanslar",
                column: "onaylayan_id");

            migrationBuilder.CreateIndex(
                name: "IX_avanslar_personel_id",
                table: "avanslar",
                column: "personel_id");

            migrationBuilder.CreateIndex(
                name: "IX_bildirimler_kullanici_id",
                table: "bildirimler",
                column: "kullanici_id");

            migrationBuilder.CreateIndex(
                name: "IX_cihaz_loglari_cihaz_id",
                table: "cihaz_loglari",
                column: "cihaz_id");

            migrationBuilder.CreateIndex(
                name: "IX_giris_cikislar_cihaz_id",
                table: "giris_cikislar",
                column: "cihaz_id");

            migrationBuilder.CreateIndex(
                name: "IX_giris_cikislar_giris_zamani",
                table: "giris_cikislar",
                column: "giris_zamani");

            migrationBuilder.CreateIndex(
                name: "IX_giris_cikislar_personel_id_giris_zamani",
                table: "giris_cikislar",
                columns: new[] { "personel_id", "giris_zamani" });

            migrationBuilder.CreateIndex(
                name: "IX_izinler_onaylayan_id",
                table: "izinler",
                column: "onaylayan_id");

            migrationBuilder.CreateIndex(
                name: "IX_izinler_personel_id",
                table: "izinler",
                column: "personel_id");

            migrationBuilder.CreateIndex(
                name: "IX_kullanicilar_email",
                table: "kullanicilar",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_kullanicilar_personel_id",
                table: "kullanicilar",
                column: "personel_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_kullanicilar_rol_id",
                table: "kullanicilar",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "IX_loglar_kullanici_id",
                table: "loglar",
                column: "kullanici_id");

            migrationBuilder.CreateIndex(
                name: "IX_personeller_email",
                table: "personeller",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_personeller_sicil_no",
                table: "personeller",
                column: "sicil_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_personeller_vardiya_id",
                table: "personeller",
                column: "vardiya_id");

            migrationBuilder.CreateIndex(
                name: "IX_primler_personel_id",
                table: "primler",
                column: "personel_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "avanslar");

            migrationBuilder.DropTable(
                name: "bildirimler");

            migrationBuilder.DropTable(
                name: "cihaz_loglari");

            migrationBuilder.DropTable(
                name: "giris_cikislar");

            migrationBuilder.DropTable(
                name: "izinler");

            migrationBuilder.DropTable(
                name: "loglar");

            migrationBuilder.DropTable(
                name: "parametreler");

            migrationBuilder.DropTable(
                name: "primler");

            migrationBuilder.DropTable(
                name: "tatiller");

            migrationBuilder.DropTable(
                name: "cihazlar");

            migrationBuilder.DropTable(
                name: "kullanicilar");

            migrationBuilder.DropTable(
                name: "personeller");

            migrationBuilder.DropTable(
                name: "roller");

            migrationBuilder.DropTable(
                name: "Vardiyalar");
        }
    }
}
