using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFCH.Migrations
{
    /// <inheritdoc />
    public partial class prod2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Cedula",
                table: "Entidad",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "DetalleRecepcionLeches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ProveedorId",
                table: "DetalleRecepcionLeches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Tanda",
                table: "DetalleRecepcionLeches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetalleRecepcionLeches_ProveedorId",
                table: "DetalleRecepcionLeches",
                column: "ProveedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleRecepcionLeches_Entidad_ProveedorId",
                table: "DetalleRecepcionLeches",
                column: "ProveedorId",
                principalTable: "Entidad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleRecepcionLeches_Entidad_ProveedorId",
                table: "DetalleRecepcionLeches");

            migrationBuilder.DropIndex(
                name: "IX_DetalleRecepcionLeches_ProveedorId",
                table: "DetalleRecepcionLeches");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "DetalleRecepcionLeches");

            migrationBuilder.DropColumn(
                name: "ProveedorId",
                table: "DetalleRecepcionLeches");

            migrationBuilder.DropColumn(
                name: "Tanda",
                table: "DetalleRecepcionLeches");

            migrationBuilder.AlterColumn<string>(
                name: "Cedula",
                table: "Entidad",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
