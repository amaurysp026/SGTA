using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFCH.Migrations
{
    /// <inheritdoc />
    public partial class prod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CostoUSD",
                table: "Productos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioUSD",
                table: "Productos",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "UnidadCompraId",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Facturas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EntidadesASOs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ENTIDAD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CODIGO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NOMBRE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CONTACTO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TELEFONO1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TELEFONO2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TELCONTACTO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RNC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CTABANC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DIRECCION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GENERO = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    FECNAC = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntidadesASOs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Existencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CostoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Documento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Entrada = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Salida = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CantidadTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Existencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Existencias_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TipoEntidadAso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoEntidadAso", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entidad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoCivil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Celular = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaEntrada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CuentaBanco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Credito = table.Column<bool>(type: "bit", nullable: false),
                    LimiteCredito = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiasCredito = table.Column<int>(type: "int", nullable: false),
                    Socio = table.Column<bool>(type: "bit", nullable: false),
                    TipoEntidadAsoId = table.Column<int>(type: "int", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entidad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entidad_TipoEntidadAso_TipoEntidadAsoId",
                        column: x => x.TipoEntidadAsoId,
                        principalTable: "TipoEntidadAso",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Productos_UnidadCompraId",
                table: "Productos",
                column: "UnidadCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_ClienteId",
                table: "Facturas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Entidad_TipoEntidadAsoId",
                table: "Entidad",
                column: "TipoEntidadAsoId");

            migrationBuilder.CreateIndex(
                name: "IX_Existencias_ProductoId",
                table: "Existencias",
                column: "ProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Entidad_ClienteId",
                table: "Facturas",
                column: "ClienteId",
                principalTable: "Entidad",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_UnidadMedidas_UnidadCompraId",
                table: "Productos",
                column: "UnidadCompraId",
                principalTable: "UnidadMedidas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Entidad_ClienteId",
                table: "Facturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Productos_UnidadMedidas_UnidadCompraId",
                table: "Productos");

            migrationBuilder.DropTable(
                name: "Entidad");

            migrationBuilder.DropTable(
                name: "EntidadesASOs");

            migrationBuilder.DropTable(
                name: "Existencias");

            migrationBuilder.DropTable(
                name: "TipoEntidadAso");

            migrationBuilder.DropIndex(
                name: "IX_Productos_UnidadCompraId",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_ClienteId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "CostoUSD",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "PrecioUSD",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "UnidadCompraId",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Facturas");
        }
    }
}
