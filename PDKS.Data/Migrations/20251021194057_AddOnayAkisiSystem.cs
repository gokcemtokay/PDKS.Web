using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOnayAkisiSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnayAkislari_Personeller_OnaylayiciPersonelId",
                table: "OnayAkislari");

            migrationBuilder.DropIndex(
                name: "IX_OnayAkislari_OnaylayiciPersonelId",
                table: "OnayAkislari");

            migrationBuilder.DropIndex(
                name: "IX_OnayAkislari_OnayTipi_ReferansId_Sira",
                table: "OnayAkislari");

            migrationBuilder.DropColumn(
                name: "OnayDurumu",
                table: "OnayAkislari");

            migrationBuilder.DropColumn(
                name: "OnaylayiciPersonelId",
                table: "OnayAkislari");

            migrationBuilder.DropColumn(
                name: "ReferansId",
                table: "OnayAkislari");

            migrationBuilder.DropColumn(
                name: "Sira",
                table: "OnayAkislari");

            migrationBuilder.RenameColumn(
                name: "OnayTipi",
                table: "OnayAkislari",
                newName: "ModulTipi");

            migrationBuilder.RenameColumn(
                name: "OnayTarihi",
                table: "OnayAkislari",
                newName: "GuncellemeTarihi");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "OnayAkislari",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AkisAdi",
                table: "OnayAkislari",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Aktif",
                table: "OnayAkislari",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "OnayAdimlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OnayAkisiId = table.Column<int>(type: "integer", nullable: false),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    AdimAdi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OnaylayanRolId = table.Column<int>(type: "integer", nullable: true),
                    OnaylayanKullaniciId = table.Column<int>(type: "integer", nullable: true),
                    OnaylayanDepartmanId = table.Column<int>(type: "integer", nullable: true),
                    OnaylayanTipi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Zorunlu = table.Column<bool>(type: "boolean", nullable: false),
                    TimeoutGun = table.Column<int>(type: "integer", nullable: true),
                    EscalateKullaniciId = table.Column<int>(type: "integer", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnayAdimlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnayAdimlari_Departmanlar_OnaylayanDepartmanId",
                        column: x => x.OnaylayanDepartmanId,
                        principalTable: "Departmanlar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OnayAdimlari_Kullanicilar_OnaylayanKullaniciId",
                        column: x => x.OnaylayanKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OnayAdimlari_OnayAkislari_OnayAkisiId",
                        column: x => x.OnayAkisiId,
                        principalTable: "OnayAkislari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OnayAdimlari_Roller_OnaylayanRolId",
                        column: x => x.OnaylayanRolId,
                        principalTable: "Roller",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OnayKayitlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OnayAkisiId = table.Column<int>(type: "integer", nullable: false),
                    ModulTipi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReferansId = table.Column<int>(type: "integer", nullable: false),
                    TalepEdenKullaniciId = table.Column<int>(type: "integer", nullable: false),
                    MevcutAdimSira = table.Column<int>(type: "integer", nullable: false),
                    GenelDurum = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TalepTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TamamlanmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnayKayitlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnayKayitlari_Kullanicilar_TalepEdenKullaniciId",
                        column: x => x.TalepEdenKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OnayKayitlari_OnayAkislari_OnayAkisiId",
                        column: x => x.OnayAkisiId,
                        principalTable: "OnayAkislari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OnayDetaylari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OnayKaydiId = table.Column<int>(type: "integer", nullable: false),
                    OnayAdimiId = table.Column<int>(type: "integer", nullable: false),
                    AdimSira = table.Column<int>(type: "integer", nullable: false),
                    OnaylayanKullaniciId = table.Column<int>(type: "integer", nullable: true),
                    Durum = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OnayTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Aciklama = table.Column<string>(type: "text", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnayDetaylari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnayDetaylari_Kullanicilar_OnaylayanKullaniciId",
                        column: x => x.OnaylayanKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OnayDetaylari_OnayAdimlari_OnayAdimiId",
                        column: x => x.OnayAdimiId,
                        principalTable: "OnayAdimlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OnayDetaylari_OnayKayitlari_OnayKaydiId",
                        column: x => x.OnayKaydiId,
                        principalTable: "OnayKayitlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OnayAdimlari_OnayAkisiId",
                table: "OnayAdimlari",
                column: "OnayAkisiId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayAdimlari_OnaylayanDepartmanId",
                table: "OnayAdimlari",
                column: "OnaylayanDepartmanId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayAdimlari_OnaylayanKullaniciId",
                table: "OnayAdimlari",
                column: "OnaylayanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayAdimlari_OnaylayanRolId",
                table: "OnayAdimlari",
                column: "OnaylayanRolId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayDetaylari_OnayAdimiId",
                table: "OnayDetaylari",
                column: "OnayAdimiId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayDetaylari_OnayKaydiId",
                table: "OnayDetaylari",
                column: "OnayKaydiId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayDetaylari_OnaylayanKullaniciId",
                table: "OnayDetaylari",
                column: "OnaylayanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayKayitlari_OnayAkisiId",
                table: "OnayKayitlari",
                column: "OnayAkisiId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayKayitlari_TalepEdenKullaniciId",
                table: "OnayKayitlari",
                column: "TalepEdenKullaniciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnayDetaylari");

            migrationBuilder.DropTable(
                name: "OnayAdimlari");

            migrationBuilder.DropTable(
                name: "OnayKayitlari");

            migrationBuilder.DropColumn(
                name: "AkisAdi",
                table: "OnayAkislari");

            migrationBuilder.DropColumn(
                name: "Aktif",
                table: "OnayAkislari");

            migrationBuilder.RenameColumn(
                name: "ModulTipi",
                table: "OnayAkislari",
                newName: "OnayTipi");

            migrationBuilder.RenameColumn(
                name: "GuncellemeTarihi",
                table: "OnayAkislari",
                newName: "OnayTarihi");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "OnayAkislari",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "OnayDurumu",
                table: "OnayAkislari",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OnaylayiciPersonelId",
                table: "OnayAkislari",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReferansId",
                table: "OnayAkislari",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sira",
                table: "OnayAkislari",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OnayAkislari_OnaylayiciPersonelId",
                table: "OnayAkislari",
                column: "OnaylayiciPersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_OnayAkislari_OnayTipi_ReferansId_Sira",
                table: "OnayAkislari",
                columns: new[] { "OnayTipi", "ReferansId", "Sira" });

            migrationBuilder.AddForeignKey(
                name: "FK_OnayAkislari_Personeller_OnaylayiciPersonelId",
                table: "OnayAkislari",
                column: "OnaylayiciPersonelId",
                principalTable: "Personeller",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
