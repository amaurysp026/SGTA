using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGTA.Migrations
{
    /// <inheritdoc />
    public partial class actualizar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContratosTractores");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContratosTractores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false),
                    EntidadId = table.Column<int>(type: "int", nullable: false),
                    TipoServicioTractorId = table.Column<int>(type: "int", nullable: false),
                    TurnoId = table.Column<int>(type: "int", nullable: false),
                    ConArado = table.Column<bool>(type: "bit", nullable: false),
                    ConMuro = table.Column<bool>(type: "bit", nullable: false),
                    Credito = table.Column<bool>(type: "bit", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nota = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nulo = table.Column<bool>(type: "bit", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Otros = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tarea = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPagado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPendiente = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratosTractores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContratosTractores_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContratosTractores_Entidad_EntidadId",
                        column: x => x.EntidadId,
                        principalTable: "Entidad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContratosTractores_TipoServicioTractores_TipoServicioTractorId",
                        column: x => x.TipoServicioTractorId,
                        principalTable: "TipoServicioTractores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContratosTractores_Turnos_TurnoId",
                        column: x => x.TurnoId,
                        principalTable: "Turnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContratosTractores_EmpleadoId",
                table: "ContratosTractores",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratosTractores_EntidadId",
                table: "ContratosTractores",
                column: "EntidadId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratosTractores_TipoServicioTractorId",
                table: "ContratosTractores",
                column: "TipoServicioTractorId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratosTractores_TurnoId",
                table: "ContratosTractores",
                column: "TurnoId");
        }
    }
}
