using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFCH.Migrations
{
    /// <inheritdoc />
    public partial class paseinventario2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Costo",
                table: "DetalleContInventarios",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Costo",
                table: "DetalleContInventarios");
        }
    }
}
