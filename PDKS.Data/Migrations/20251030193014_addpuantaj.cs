using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class addpuantaj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Puantajlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SirketId = table.Column<int>(type: "integer", nullable: false),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    Yil = table.Column<int>(type: "integer", nullable: false),
                    Ay = table.Column<int>(type: "integer", nullable: false),
                    ToplamCalismaSaati = table.Column<int>(type: "integer", nullable: false),
                    NormalMesaiSaati = table.Column<int>(type: "integer", nullable: false),
                    FazlaMesaiSaati = table.Column<int>(type: "integer", nullable: false),
                    GeceMesaiSaati = table.Column<int>(type: "integer", nullable: false),
                    HaftaSonuMesaiSaati = table.Column<int>(type: "integer", nullable: false),
                    ToplamCalisilanGun = table.Column<int>(type: "integer", nullable: false),
                    DevamsizlikGunu = table.Column<int>(type: "integer", nullable: false),
                    IzinGunu = table.Column<int>(type: "integer", nullable: false),
                    RaporluGun = table.Column<int>(type: "integer", nullable: false),
                    HaftaTatiliGunu = table.Column<int>(type: "integer", nullable: false),
                    ResmiTatilGunu = table.Column<int>(type: "integer", nullable: false),
                    GecKalmaGunu = table.Column<int>(type: "integer", nullable: false),
                    GecKalmaSuresi = table.Column<int>(type: "integer", nullable: false),
                    ErkenCikisGunu = table.Column<int>(type: "integer", nullable: false),
                    ErkenCikisSuresi = table.Column<int>(type: "integer", nullable: false),
                    EksikCalismaSaati = table.Column<int>(type: "integer", nullable: false),
                    Durum = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OnayTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OnaylayanKullaniciId = table.Column<int>(type: "integer", nullable: true),
                    Notlar = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puantajlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Puantajlar_Kullanicilar_OnaylayanKullaniciId",
                        column: x => x.OnaylayanKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Puantajlar_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Puantajlar_sirketler_SirketId",
                        column: x => x.SirketId,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PuantajDetaylar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PuantajId = table.Column<int>(type: "integer", nullable: false),
                    PersonelId = table.Column<int>(type: "integer", nullable: false),
                    Tarih = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VardiyaId = table.Column<int>(type: "integer", nullable: true),
                    GirisCikisId = table.Column<int>(type: "integer", nullable: true),
                    PlanlananGiris = table.Column<TimeSpan>(type: "interval", nullable: true),
                    PlanlananCikis = table.Column<TimeSpan>(type: "interval", nullable: true),
                    PlanlananSure = table.Column<int>(type: "integer", nullable: false),
                    GerceklesenGiris = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GerceklesenCikis = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GerceklesenSure = table.Column<int>(type: "integer", nullable: true),
                    NormalMesai = table.Column<int>(type: "integer", nullable: true),
                    FazlaMesai = table.Column<int>(type: "integer", nullable: true),
                    GeceMesai = table.Column<int>(type: "integer", nullable: true),
                    GecKalmaSuresi = table.Column<int>(type: "integer", nullable: true),
                    ErkenCikisSuresi = table.Column<int>(type: "integer", nullable: true),
                    EksikSure = table.Column<int>(type: "integer", nullable: true),
                    GunDurumu = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IzinTuru = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HaftaSonuMu = table.Column<bool>(type: "boolean", nullable: false),
                    ResmiTatilMi = table.Column<bool>(type: "boolean", nullable: false),
                    GecKaldiMi = table.Column<bool>(type: "boolean", nullable: false),
                    ErkenCiktiMi = table.Column<bool>(type: "boolean", nullable: false),
                    DevamsizMi = table.Column<bool>(type: "boolean", nullable: false),
                    Notlar = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PuantajDetaylar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PuantajDetaylar_GirisCikislar_GirisCikisId",
                        column: x => x.GirisCikisId,
                        principalTable: "GirisCikislar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PuantajDetaylar_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PuantajDetaylar_Puantajlar_PuantajId",
                        column: x => x.PuantajId,
                        principalTable: "Puantajlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PuantajDetaylar_Vardiyalar_VardiyaId",
                        column: x => x.VardiyaId,
                        principalTable: "Vardiyalar",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_PuantajDetaylar_VardiyaId",
                table: "PuantajDetaylar",
                column: "VardiyaId");

            migrationBuilder.CreateIndex(
                name: "IX_Puantajlar_OnaylayanKullaniciId",
                table: "Puantajlar",
                column: "OnaylayanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Puantajlar_PersonelId",
                table: "Puantajlar",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_Puantajlar_SirketId",
                table: "Puantajlar",
                column: "SirketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PuantajDetaylar");

            migrationBuilder.DropTable(
                name: "Puantajlar");
        }
    }
}
