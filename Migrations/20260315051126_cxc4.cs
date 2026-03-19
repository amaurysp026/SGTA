using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFCH.Migrations
{
    /// <inheritdoc />
    public partial class cxc4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CxCs_Personas_ClienteId",
                table: "CxCs");

            migrationBuilder.AddColumn<bool>(
                name: "Anulado",
                table: "PagosCxC",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "PagosCxC",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ClienteId",
                table: "CxCs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "Anulado",
                table: "CxCs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Cliente2Id",
                table: "CxCs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Titular",
                table: "CxCs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PagosCxC_UsuarioId",
                table: "PagosCxC",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_CxCs_Cliente2Id",
                table: "CxCs",
                column: "Cliente2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CxCs_Entidad_Cliente2Id",
                table: "CxCs",
                column: "Cliente2Id",
                principalTable: "Entidad",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CxCs_Personas_ClienteId",
                table: "CxCs",
                column: "ClienteId",
                principalTable: "Personas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PagosCxC_Usuarios_UsuarioId",
                table: "PagosCxC",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CxCs_Entidad_Cliente2Id",
                table: "CxCs");

            migrationBuilder.DropForeignKey(
                name: "FK_CxCs_Personas_ClienteId",
                table: "CxCs");

            migrationBuilder.DropForeignKey(
                name: "FK_PagosCxC_Usuarios_UsuarioId",
                table: "PagosCxC");

            migrationBuilder.DropIndex(
                name: "IX_PagosCxC_UsuarioId",
                table: "PagosCxC");

            migrationBuilder.DropIndex(
                name: "IX_CxCs_Cliente2Id",
                table: "CxCs");

            migrationBuilder.DropColumn(
                name: "Anulado",
                table: "PagosCxC");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "PagosCxC");

            migrationBuilder.DropColumn(
                name: "Anulado",
                table: "CxCs");

            migrationBuilder.DropColumn(
                name: "Cliente2Id",
                table: "CxCs");

            migrationBuilder.DropColumn(
                name: "Titular",
                table: "CxCs");

            migrationBuilder.AlterColumn<int>(
                name: "ClienteId",
                table: "CxCs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CxCs_Personas_ClienteId",
                table: "CxCs",
                column: "ClienteId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
