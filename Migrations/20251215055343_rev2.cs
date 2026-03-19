using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFCH.Migrations
{
    /// <inheritdoc />
    public partial class rev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleFactura_Facturas_FacturaId",
                table: "DetalleFactura");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleFactura_Productos_ProductoId",
                table: "DetalleFactura");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetalleFactura",
                table: "DetalleFactura");

            migrationBuilder.RenameTable(
                name: "DetalleFactura",
                newName: "DetalleFacturas");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleFactura_ProductoId",
                table: "DetalleFacturas",
                newName: "IX_DetalleFacturas_ProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleFactura_FacturaId",
                table: "DetalleFacturas",
                newName: "IX_DetalleFacturas_FacturaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetalleFacturas",
                table: "DetalleFacturas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleFacturas_Facturas_FacturaId",
                table: "DetalleFacturas",
                column: "FacturaId",
                principalTable: "Facturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleFacturas_Productos_ProductoId",
                table: "DetalleFacturas",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleFacturas_Facturas_FacturaId",
                table: "DetalleFacturas");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleFacturas_Productos_ProductoId",
                table: "DetalleFacturas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetalleFacturas",
                table: "DetalleFacturas");

            migrationBuilder.RenameTable(
                name: "DetalleFacturas",
                newName: "DetalleFactura");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleFacturas_ProductoId",
                table: "DetalleFactura",
                newName: "IX_DetalleFactura_ProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_DetalleFacturas_FacturaId",
                table: "DetalleFactura",
                newName: "IX_DetalleFactura_FacturaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetalleFactura",
                table: "DetalleFactura",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleFactura_Facturas_FacturaId",
                table: "DetalleFactura",
                column: "FacturaId",
                principalTable: "Facturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleFactura_Productos_ProductoId",
                table: "DetalleFactura",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id");
        }
    }
}
