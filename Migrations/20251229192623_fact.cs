using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFCH.Migrations
{
    /// <inheritdoc />
    public partial class fact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FacACredito",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Productos",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PagoCon",
                table: "Facturas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Propina",
                table: "Facturas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Facturas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MesnajeFinal1",
                table: "Configuracions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MesnajeFinal2",
                table: "Configuracions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacACredito",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Productos",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "PagoCon",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "Propina",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "MesnajeFinal1",
                table: "Configuracions");

            migrationBuilder.DropColumn(
                name: "MesnajeFinal2",
                table: "Configuracions");
        }
    }
}
