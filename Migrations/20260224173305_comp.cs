using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFCH.Migrations
{
    /// <inheritdoc />
    public partial class comp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "DetalleCompras");

            migrationBuilder.RenameColumn(
                name: "SubttalUSD",
                table: "DetalleCompras",
                newName: "Tasa");

            migrationBuilder.AddColumn<bool>(
                name: "Acopio",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Tractor",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Compuesto",
                table: "Productos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FacGrade",
                table: "Configuracions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "TasaCambio",
                table: "Configuracions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TasaCambio2",
                table: "Configuracions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "DetallesProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrecio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCosoto = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesProducto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesProducto_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetallesProducto_ProductoId",
                table: "DetallesProducto",
                column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetallesProducto");

            migrationBuilder.DropColumn(
                name: "Acopio",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Tractor",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Compuesto",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "FacGrade",
                table: "Configuracions");

            migrationBuilder.DropColumn(
                name: "TasaCambio",
                table: "Configuracions");

            migrationBuilder.DropColumn(
                name: "TasaCambio2",
                table: "Configuracions");

            migrationBuilder.RenameColumn(
                name: "Tasa",
                table: "DetalleCompras",
                newName: "SubttalUSD");

            migrationBuilder.AddColumn<decimal>(
                name: "Subtotal",
                table: "DetalleCompras",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
