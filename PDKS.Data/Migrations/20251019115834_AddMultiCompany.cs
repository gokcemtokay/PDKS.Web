using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. ÖNCE SIRKETLER TABLOSUNU OLUŞTUR
            migrationBuilder.CreateTable(
                name: "sirketler",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    unvan = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ticari_unvan = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    vergi_no = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    vergi_dairesi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    telefon = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    adres = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    il = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ilce = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    posta_kodu = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    website = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    logo_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    kurulis_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    aktif = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    para_birimi = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false, defaultValue: "TRY"),
                    notlar = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    guncelleme_tarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ana_sirket = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ana_sirket_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sirketler", x => x.id);
                    table.ForeignKey(
                        name: "FK_sirketler_sirketler_ana_sirket_id",
                        column: x => x.ana_sirket_id,
                        principalTable: "sirketler",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sirketler_ana_sirket_id",
                table: "sirketler",
                column: "ana_sirket_id");

            // 2. DEFAULT ŞIRKET OLUŞTUR
            migrationBuilder.Sql(@"
                INSERT INTO sirketler (unvan, vergi_no, aktif, ana_sirket, olusturma_tarihi, para_birimi)
                VALUES ('Ana Şirket', '0000000000', true, true, NOW(), 'TRY');
            ");

            // 3. DEPARTMANLAR TABLOSUNA KOLONLARI EKLE (ÖNCE NULL OLARAK)
            migrationBuilder.AddColumn<int>(
                name: "SirketId",
                table: "Departmanlar",
                type: "integer",
                nullable: true); // Önce NULL

            migrationBuilder.AddColumn<bool>(
                name: "Aktif",
                table: "Departmanlar",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GuncellemeTarihi",
                table: "Departmanlar",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OlusturmaTarihi",
                table: "Departmanlar",
                type: "timestamp with time zone",
                nullable: true,
                defaultValueSql: "NOW()");

            // 4. MEVCUT DEPARTMANLARA DEFAULT ŞİRKET ID'SİNİ ATA
            migrationBuilder.Sql(@"
                UPDATE ""Departmanlar""
                SET ""SirketId"" = (SELECT id FROM sirketler ORDER BY id LIMIT 1)
                WHERE ""SirketId"" IS NULL;
            ");

            // 5. SirketId'yi NOT NULL YAP
            migrationBuilder.AlterColumn<int>(
                name: "SirketId",
                table: "Departmanlar",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            // 6. INDEX VE FOREIGN KEY EKLE
            migrationBuilder.CreateIndex(
                name: "IX_Departmanlar_SirketId",
                table: "Departmanlar",
                column: "SirketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departmanlar_sirketler_SirketId",
                table: "Departmanlar",
                column: "SirketId",
                principalTable: "sirketler",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            // 7. PERSONELLER TABLOSUNA KOLON EKLE (ÖNCE NULL)
            migrationBuilder.AddColumn<int>(
                name: "sirket_id",
                table: "Personeller",
                type: "integer",
                nullable: true); // Önce NULL

            // 8. MEVCUT PERSONELLERE DEFAULT ŞİRKET ID'SİNİ ATA
            migrationBuilder.Sql(@"
                UPDATE ""Personeller""
                SET sirket_id = (SELECT id FROM sirketler ORDER BY id LIMIT 1)
                WHERE sirket_id IS NULL;
            ");

            // 9. sirket_id'yi NOT NULL YAP
            migrationBuilder.AlterColumn<int>(
                name: "sirket_id",
                table: "Personeller",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            // 10. INDEX VE FOREIGN KEY EKLE
            migrationBuilder.CreateIndex(
                name: "IX_Personeller_sirket_id",
                table: "Personeller",
                column: "sirket_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Personeller_sirketler_sirket_id",
                table: "Personeller",
                column: "sirket_id",
                principalTable: "sirketler",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            // 11. CIHAZLAR TABLOSU (NULLABLE KALACAK)
            migrationBuilder.AddColumn<int>(
                name: "SirketId",
                table: "cihazlar",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_cihazlar_SirketId",
                table: "cihazlar",
                column: "SirketId");

            migrationBuilder.AddForeignKey(
                name: "FK_cihazlar_sirketler_SirketId",
                table: "cihazlar",
                column: "SirketId",
                principalTable: "sirketler",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cihazlar_sirketler_SirketId",
                table: "cihazlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Departmanlar_sirketler_SirketId",
                table: "Departmanlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Personeller_sirketler_sirket_id",
                table: "Personeller");

            migrationBuilder.DropTable(
                name: "sirketler");

            migrationBuilder.DropIndex(
                name: "IX_Personeller_sirket_id",
                table: "Personeller");

            migrationBuilder.DropIndex(
                name: "IX_Departmanlar_SirketId",
                table: "Departmanlar");

            migrationBuilder.DropIndex(
                name: "IX_cihazlar_SirketId",
                table: "cihazlar");

            migrationBuilder.DropColumn(
                name: "sirket_id",
                table: "Personeller");

            migrationBuilder.DropColumn(
                name: "Aktif",
                table: "Departmanlar");

            migrationBuilder.DropColumn(
                name: "GuncellemeTarihi",
                table: "Departmanlar");

            migrationBuilder.DropColumn(
                name: "OlusturmaTarihi",
                table: "Departmanlar");

            migrationBuilder.DropColumn(
                name: "SirketId",
                table: "Departmanlar");

            migrationBuilder.DropColumn(
                name: "SirketId",
                table: "cihazlar");
        }
    }
}