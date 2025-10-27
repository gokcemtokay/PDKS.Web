using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class Yetkilendirme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Aktif",
                table: "Roller",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "IslemYetkiler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IslemKodu = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IslemAdi = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ModulAdi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Aciklama = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Aktif = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IslemYetkiler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menuler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MenuAdi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MenuKodu = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Url = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Icon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UstMenuId = table.Column<int>(type: "integer", nullable: true),
                    Sira = table.Column<int>(type: "integer", nullable: false),
                    Aktif = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menuler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menuler_Menuler_UstMenuId",
                        column: x => x.UstMenuId,
                        principalTable: "Menuler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolIslemYetkiler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RolId = table.Column<int>(type: "integer", nullable: false),
                    IslemYetkiId = table.Column<int>(type: "integer", nullable: false),
                    Izinli = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolIslemYetkiler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolIslemYetkiler_IslemYetkiler_IslemYetkiId",
                        column: x => x.IslemYetkiId,
                        principalTable: "IslemYetkiler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolIslemYetkiler_Roller_RolId",
                        column: x => x.RolId,
                        principalTable: "Roller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuRoller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MenuId = table.Column<int>(type: "integer", nullable: false),
                    RolId = table.Column<int>(type: "integer", nullable: false),
                    Okuma = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuRoller", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuRoller_Menuler_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menuler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuRoller_Roller_RolId",
                        column: x => x.RolId,
                        principalTable: "Roller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Roller",
                keyColumn: "Id",
                keyValue: 1,
                column: "Aktif",
                value: true);

            migrationBuilder.UpdateData(
                table: "Roller",
                keyColumn: "Id",
                keyValue: 2,
                column: "Aktif",
                value: true);

            migrationBuilder.UpdateData(
                table: "Roller",
                keyColumn: "Id",
                keyValue: 3,
                column: "Aktif",
                value: true);

            migrationBuilder.UpdateData(
                table: "Roller",
                keyColumn: "Id",
                keyValue: 4,
                column: "Aktif",
                value: true);

            migrationBuilder.CreateIndex(
                name: "IX_IslemYetkiler_IslemKodu",
                table: "IslemYetkiler",
                column: "IslemKodu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menuler_MenuKodu",
                table: "Menuler",
                column: "MenuKodu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menuler_UstMenuId",
                table: "Menuler",
                column: "UstMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuRoller_MenuId_RolId",
                table: "MenuRoller",
                columns: new[] { "MenuId", "RolId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuRoller_RolId",
                table: "MenuRoller",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_RolIslemYetkiler_IslemYetkiId",
                table: "RolIslemYetkiler",
                column: "IslemYetkiId");

            migrationBuilder.CreateIndex(
                name: "IX_RolIslemYetkiler_RolId_IslemYetkiId",
                table: "RolIslemYetkiler",
                columns: new[] { "RolId", "IslemYetkiId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuRoller");

            migrationBuilder.DropTable(
                name: "RolIslemYetkiler");

            migrationBuilder.DropTable(
                name: "Menuler");

            migrationBuilder.DropTable(
                name: "IslemYetkiler");

            migrationBuilder.DropColumn(
                name: "Aktif",
                table: "Roller");
        }
    }
}
