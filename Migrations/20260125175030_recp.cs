using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFCH.Migrations
{
    /// <inheritdoc />
    public partial class recp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Frezzers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CapacidadTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frezzers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecepcionLeches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tanda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecepcionLeches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecepcionLeches_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleFrezzers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FrezzerId = table.Column<int>(type: "int", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salida = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Entrada = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleFrezzers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleFrezzers_Frezzers_FrezzerId",
                        column: x => x.FrezzerId,
                        principalTable: "Frezzers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleRecepcionLeches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecepcionLecheId = table.Column<int>(type: "int", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Litros = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Grasa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FreezerId = table.Column<int>(type: "int", nullable: true),
                    SolidosTotales = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioPorLitro = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Nulo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleRecepcionLeches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleRecepcionLeches_Frezzers_FreezerId",
                        column: x => x.FreezerId,
                        principalTable: "Frezzers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DetalleRecepcionLeches_RecepcionLeches_RecepcionLecheId",
                        column: x => x.RecepcionLecheId,
                        principalTable: "RecepcionLeches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFrezzers_FrezzerId",
                table: "DetalleFrezzers",
                column: "FrezzerId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleRecepcionLeches_FreezerId",
                table: "DetalleRecepcionLeches",
                column: "FreezerId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleRecepcionLeches_RecepcionLecheId",
                table: "DetalleRecepcionLeches",
                column: "RecepcionLecheId");

            migrationBuilder.CreateIndex(
                name: "IX_RecepcionLeches_UsuarioId",
                table: "RecepcionLeches",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleFrezzers");

            migrationBuilder.DropTable(
                name: "DetalleRecepcionLeches");

            migrationBuilder.DropTable(
                name: "Frezzers");

            migrationBuilder.DropTable(
                name: "RecepcionLeches");
        }
    }
}
