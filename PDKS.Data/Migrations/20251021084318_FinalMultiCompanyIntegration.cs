using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinalMultiCompanyIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SirketId",
                table: "Vardiyalar",
                type: "integer",
                nullable: true,
                defaultValue: 1);

            migrationBuilder.Sql(@"
        UPDATE ""Vardiyalar""
        SET ""SirketId"" = (SELECT id FROM sirketler ORDER BY id LIMIT 1)
        WHERE ""SirketId"" IS NULL;
    ");

            migrationBuilder.AlterColumn<int>(
        name: "SirketId",
        table: "Vardiyalar",
        type: "integer",
        nullable: false,
        oldClrType: typeof(int),
        oldType: "integer",
        oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Vardiyalar",
                keyColumn: "Id",
                keyValue: 1,
                column: "SirketId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Vardiyalar",
                keyColumn: "Id",
                keyValue: 2,
                column: "SirketId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Vardiyalar",
                keyColumn: "Id",
                keyValue: 3,
                column: "SirketId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Vardiyalar_SirketId",
                table: "Vardiyalar",
                column: "SirketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vardiyalar_sirketler_SirketId",
                table: "Vardiyalar",
                column: "SirketId",
                principalTable: "sirketler",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vardiyalar_sirketler_SirketId",
                table: "Vardiyalar");

            migrationBuilder.DropIndex(
                name: "IX_Vardiyalar_SirketId",
                table: "Vardiyalar");

            migrationBuilder.DropColumn(
                name: "SirketId",
                table: "Vardiyalar");
        }
    }
}
