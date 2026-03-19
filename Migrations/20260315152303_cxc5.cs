using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFCH.Migrations
{
    /// <inheritdoc />
    public partial class cxc5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientePId",
                table: "Facturas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_ClientePId",
                table: "Facturas",
                column: "ClientePId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Personas_ClientePId",
                table: "Facturas",
                column: "ClientePId",
                principalTable: "Personas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Personas_ClientePId",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_ClientePId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "ClientePId",
                table: "Facturas");
        }
    }
}
