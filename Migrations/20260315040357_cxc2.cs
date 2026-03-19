using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFCH.Migrations
{
    /// <inheritdoc />
    public partial class cxc2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiasCredito",
                table: "CxCs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FacturaId",
                table: "CxCs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PagosCxC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CxCId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pagado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FormaPago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Referencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Banco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagosCxC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagosCxC_CxCs_CxCId",
                        column: x => x.CxCId,
                        principalTable: "CxCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CxCs_FacturaId",
                table: "CxCs",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_PagosCxC_CxCId",
                table: "PagosCxC",
                column: "CxCId");

            migrationBuilder.AddForeignKey(
                name: "FK_CxCs_Facturas_FacturaId",
                table: "CxCs",
                column: "FacturaId",
                principalTable: "Facturas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CxCs_Facturas_FacturaId",
                table: "CxCs");

            migrationBuilder.DropTable(
                name: "PagosCxC");

            migrationBuilder.DropIndex(
                name: "IX_CxCs_FacturaId",
                table: "CxCs");

            migrationBuilder.DropColumn(
                name: "DiasCredito",
                table: "CxCs");

            migrationBuilder.DropColumn(
                name: "FacturaId",
                table: "CxCs");
        }
    }
}
