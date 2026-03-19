using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFCH.Migrations
{
    /// <inheritdoc />
    public partial class paseinventario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CiereAutoRecepcionLeche",
                table: "Configuracions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CierreAutoTurno",
                table: "Configuracions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DiasBackup",
                table: "Configuracions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LitroMaximo",
                table: "Configuracions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UsarDolar",
                table: "Configuracions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ContInventarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaConteo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aplicado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContInventarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContInventarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleContInventarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContInventarioId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    CantSistema = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CantFisico = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Diferencia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Contador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleContInventarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleContInventarios_ContInventarios_ContInventarioId",
                        column: x => x.ContInventarioId,
                        principalTable: "ContInventarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleContInventarios_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContInventarios_UsuarioId",
                table: "ContInventarios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleContInventarios_ContInventarioId",
                table: "DetalleContInventarios",
                column: "ContInventarioId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleContInventarios_ProductoId",
                table: "DetalleContInventarios",
                column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleContInventarios");

            migrationBuilder.DropTable(
                name: "ContInventarios");

            migrationBuilder.DropColumn(
                name: "CiereAutoRecepcionLeche",
                table: "Configuracions");

            migrationBuilder.DropColumn(
                name: "CierreAutoTurno",
                table: "Configuracions");

            migrationBuilder.DropColumn(
                name: "DiasBackup",
                table: "Configuracions");

            migrationBuilder.DropColumn(
                name: "LitroMaximo",
                table: "Configuracions");

            migrationBuilder.DropColumn(
                name: "UsarDolar",
                table: "Configuracions");
        }
    }
}
