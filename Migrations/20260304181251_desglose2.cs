using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFCH.Migrations
{
    /// <inheritdoc />
    public partial class desglose2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DesgloseBilletes_Turnos_TurnoId",
                table: "DesgloseBilletes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DesgloseBilletes",
                table: "DesgloseBilletes");

            migrationBuilder.RenameTable(
                name: "DesgloseBilletes",
                newName: "DesglosesBilletes");

            migrationBuilder.RenameIndex(
                name: "IX_DesgloseBilletes_TurnoId",
                table: "DesglosesBilletes",
                newName: "IX_DesglosesBilletes_TurnoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DesglosesBilletes",
                table: "DesglosesBilletes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DesglosesBilletes_Turnos_TurnoId",
                table: "DesglosesBilletes",
                column: "TurnoId",
                principalTable: "Turnos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DesglosesBilletes_Turnos_TurnoId",
                table: "DesglosesBilletes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DesglosesBilletes",
                table: "DesglosesBilletes");

            migrationBuilder.RenameTable(
                name: "DesglosesBilletes",
                newName: "DesgloseBilletes");

            migrationBuilder.RenameIndex(
                name: "IX_DesglosesBilletes_TurnoId",
                table: "DesgloseBilletes",
                newName: "IX_DesgloseBilletes_TurnoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DesgloseBilletes",
                table: "DesgloseBilletes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DesgloseBilletes_Turnos_TurnoId",
                table: "DesgloseBilletes",
                column: "TurnoId",
                principalTable: "Turnos",
                principalColumn: "Id");
        }
    }
}
