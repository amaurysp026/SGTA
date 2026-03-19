using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFCH.Migrations
{
    /// <inheritdoc />
    public partial class prod3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entidad_TipoEntidadAso_TipoEntidadAsoId",
                table: "Entidad");

            migrationBuilder.DropTable(
                name: "TipoEntidadAso");

            migrationBuilder.DropColumn(
                name: "CODIGO",
                table: "EntidadesASOs");

            migrationBuilder.DropColumn(
                name: "CONTACTO",
                table: "EntidadesASOs");

            migrationBuilder.DropColumn(
                name: "CTABANC",
                table: "EntidadesASOs");

            migrationBuilder.DropColumn(
                name: "DIRECCION",
                table: "EntidadesASOs");

            migrationBuilder.DropColumn(
                name: "ENTIDAD",
                table: "EntidadesASOs");

            migrationBuilder.DropColumn(
                name: "FECNAC",
                table: "EntidadesASOs");

            migrationBuilder.DropColumn(
                name: "GENERO",
                table: "EntidadesASOs");

            migrationBuilder.DropColumn(
                name: "RNC",
                table: "EntidadesASOs");

            migrationBuilder.DropColumn(
                name: "TELCONTACTO",
                table: "EntidadesASOs");

            migrationBuilder.DropColumn(
                name: "TELEFONO1",
                table: "EntidadesASOs");

            migrationBuilder.DropColumn(
                name: "TELEFONO2",
                table: "EntidadesASOs");

            migrationBuilder.RenameColumn(
                name: "NOMBRE",
                table: "EntidadesASOs",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "EntidadesASOs",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entidad_EntidadesASOs_TipoEntidadAsoId",
                table: "Entidad",
                column: "TipoEntidadAsoId",
                principalTable: "EntidadesASOs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entidad_EntidadesASOs_TipoEntidadAsoId",
                table: "Entidad");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "EntidadesASOs",
                newName: "NOMBRE");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "EntidadesASOs",
                newName: "ID");

            migrationBuilder.AddColumn<string>(
                name: "CODIGO",
                table: "EntidadesASOs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CONTACTO",
                table: "EntidadesASOs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CTABANC",
                table: "EntidadesASOs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DIRECCION",
                table: "EntidadesASOs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ENTIDAD",
                table: "EntidadesASOs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FECNAC",
                table: "EntidadesASOs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GENERO",
                table: "EntidadesASOs",
                type: "nvarchar(1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RNC",
                table: "EntidadesASOs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TELCONTACTO",
                table: "EntidadesASOs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TELEFONO1",
                table: "EntidadesASOs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TELEFONO2",
                table: "EntidadesASOs",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Entidad_TipoEntidadAso_TipoEntidadAsoId",
                table: "Entidad",
                column: "TipoEntidadAsoId",
                principalTable: "TipoEntidadAso",
                principalColumn: "Id");
        }
    }
}
