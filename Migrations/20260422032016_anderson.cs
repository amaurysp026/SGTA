using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGTA.Migrations
{
    /// <inheritdoc />
    public partial class anderson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RutaImagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configuracions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEmpresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RNC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Moneda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Decimales = table.Column<int>(type: "int", nullable: false),
                    MostrarLogoEnReportes = table.Column<bool>(type: "bit", nullable: false),
                    UsarImpuestos = table.Column<bool>(type: "bit", nullable: false),
                    TasaImpuesto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HabilitarRedondeo = table.Column<bool>(type: "bit", nullable: false),
                    DiasBackup = table.Column<bool>(type: "bit", nullable: false),
                    CierreAutoTurno = table.Column<bool>(type: "bit", nullable: false),
                    CiereAutoRecepcionLeche = table.Column<bool>(type: "bit", nullable: false),
                    LitroMaximo = table.Column<bool>(type: "bit", nullable: false),
                    UsarDolar = table.Column<bool>(type: "bit", nullable: false),
                    TasaRedondeo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FormatoFecha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormatoHora = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoPrincipal = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Timbrado = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    VenderSinExistencia = table.Column<bool>(type: "bit", nullable: false),
                    MesnajeBienvenida = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreComercial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MesnajeFinal1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MesnajeFinal2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacGrade = table.Column<bool>(type: "bit", nullable: false),
                    TasaCambio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TasaCambio2 = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    Salario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comision = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechadeIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaSalida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Banco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CuentaBancaria = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntidadesASOs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntidadesASOs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "f",
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
                    table.PrimaryKey("PK_f", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Impuestos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreImpuesto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Porcentaje = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impuestos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RNC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contacto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoCuentas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCuentas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoEntidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoEntidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoServicioTractores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoServicioTractores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnidadMedidas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadMedidas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContrasenaHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimoAcceso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Administrador = table.Column<bool>(type: "bit", nullable: false),
                    Caja = table.Column<bool>(type: "bit", nullable: false),
                    Reportes = table.Column<bool>(type: "bit", nullable: false),
                    Cuentas = table.Column<bool>(type: "bit", nullable: false),
                    Entidades = table.Column<bool>(type: "bit", nullable: false),
                    Inventario = table.Column<bool>(type: "bit", nullable: false),
                    Compras = table.Column<bool>(type: "bit", nullable: false),
                    Productos = table.Column<bool>(type: "bit", nullable: false),
                    FacACredito = table.Column<bool>(type: "bit", nullable: false),
                    PuntoVentas = table.Column<bool>(type: "bit", nullable: false),
                    ModificarPrecios = table.Column<bool>(type: "bit", nullable: false),
                    AnularFacturas = table.Column<bool>(type: "bit", nullable: false),
                    CerrarTurno = table.Column<bool>(type: "bit", nullable: false),
                    AbrirTurno = table.Column<bool>(type: "bit", nullable: false),
                    Facturacion = table.Column<bool>(type: "bit", nullable: false),
                    Configuracion = table.Column<bool>(type: "bit", nullable: false),
                    Herramientas = table.Column<bool>(type: "bit", nullable: false),
                    Usuarios = table.Column<bool>(type: "bit", nullable: false),
                    Contabilidad = table.Column<bool>(type: "bit", nullable: false),
                    AnularTrasnas = table.Column<bool>(type: "bit", nullable: false),
                    Tractor = table.Column<bool>(type: "bit", nullable: false),
                    Acopio = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entidad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cedula = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entidad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entidad_EntidadesASOs_TipoEntidadAsoId",
                        column: x => x.TipoEntidadAsoId,
                        principalTable: "EntidadesASOs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "fd",
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
                    table.PrimaryKey("PK_fd", x => x.Id);
                    table.ForeignKey(
                        name: "FK_fd_f_FrezzerId",
                        column: x => x.FrezzerId,
                        principalTable: "f",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Identificacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoIdentificacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Celular = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CodSocioAsogafar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescuentoAsogafar = table.Column<bool>(type: "bit", nullable: false),
                    MontoDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoEntidadId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personas_TipoEntidades_TipoEntidadId",
                        column: x => x.TipoEntidadId,
                        principalTable: "TipoEntidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Referencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodFabricante = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioUSD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CostoUSD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ITBIS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RutaImagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    UnidadId = table.Column<int>(type: "int", nullable: false),
                    UnidadCompraId = table.Column<int>(type: "int", nullable: true),
                    FechaFabricacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CantidadMinima = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VenderSinExistencia = table.Column<bool>(type: "bit", nullable: false),
                    CambiarPrecio = table.Column<bool>(type: "bit", nullable: false),
                    PermitirStockNegativo = table.Column<bool>(type: "bit", nullable: false),
                    Vence = table.Column<bool>(type: "bit", nullable: false),
                    AplicaITBIS = table.Column<bool>(type: "bit", nullable: false),
                    EsServicio = table.Column<bool>(type: "bit", nullable: false),
                    AplicaDescuento = table.Column<bool>(type: "bit", nullable: false),
                    AplicaCredito = table.Column<bool>(type: "bit", nullable: false),
                    Compuesto = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Productos_UnidadMedidas_UnidadCompraId",
                        column: x => x.UnidadCompraId,
                        principalTable: "UnidadMedidas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Productos_UnidadMedidas_UnidadId",
                        column: x => x.UnidadId,
                        principalTable: "UnidadMedidas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroCompra = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroFactura = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCompra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TasaCambio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalUSD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoPagado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoPendiente = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProveedorId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Nula = table.Column<bool>(type: "bit", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compras_Proveedores_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Compras_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracionLocals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEquipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrintReporte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrintFactura = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrintRecibo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrintPuntoVenta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrintValeCaja = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UltimoUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UltimoInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Licencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    usuarioId = table.Column<int>(type: "int", nullable: false),
                    RecordarClave = table.Column<bool>(type: "bit", nullable: false),
                    RecordarUsuario = table.Column<bool>(type: "bit", nullable: false),
                    UsosC = table.Column<int>(type: "int", nullable: false),
                    UsosU = table.Column<int>(type: "int", nullable: false),
                    inicio = table.Column<int>(type: "int", nullable: false),
                    UsuarioRec = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClaveRec = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionLocals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracionLocals_Usuarios_usuarioId",
                        column: x => x.usuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "LiquidacionesLeche",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiquidacionesLeche", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiquidacionesLeche_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pantalla = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Detalles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Equipo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abierto = table.Column<bool>(type: "bit", nullable: false),
                    TotalInicial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ventas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Gastos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtrosIngresos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Efectivo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tarjetas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Transferencias = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cheques = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Diferencia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EfectivoContado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalFinal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Turnos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "x",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tanda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodSeguridad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_x", x => x.Id);
                    table.ForeignKey(
                        name: "FK_x_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cuentas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroCuenta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreCuenta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaApertura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Interes = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Moneda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    TitularId = table.Column<int>(type: "int", nullable: false),
                    TipoCuentaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuentas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cuentas_Personas_TitularId",
                        column: x => x.TitularId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cuentas_TipoCuentas_TipoCuentaId",
                        column: x => x.TipoCuentaId,
                        principalTable: "TipoCuentas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Placa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    anno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FiltroAceite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FiltroAire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FiltroCombustible = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FiltroCabina = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnTaller = table.Column<bool>(type: "bit", nullable: false),
                    PersonaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehiculos_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateTable(
                name: "Lotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoLote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFabricacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaEntrada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lotes_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lotes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetalleCompras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompraId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioUSD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tasa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Impuesto = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleCompras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleCompras_Compras_CompraId",
                        column: x => x.CompraId,
                        principalTable: "Compras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleCompras_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
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
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "DetallesLiquidacionLeche",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiquidacionLecheId = table.Column<int>(type: "int", nullable: false),
                    CodSocio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Litros = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDescuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetoCobrar = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetallesLiquidacionLeche", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetallesLiquidacionLeche_LiquidacionesLeche_LiquidacionLecheId",
                        column: x => x.LiquidacionLecheId,
                        principalTable: "LiquidacionesLeche",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContratosTractores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntidadId = table.Column<int>(type: "int", nullable: false),
                    TurnoId = table.Column<int>(type: "int", nullable: false),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoServicioTractorId = table.Column<int>(type: "int", nullable: false),
                    ConMuro = table.Column<bool>(type: "bit", nullable: false),
                    ConArado = table.Column<bool>(type: "bit", nullable: false),
                    Otros = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tarea = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPagado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPendiente = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credito = table.Column<bool>(type: "bit", nullable: false),
                    Nulo = table.Column<bool>(type: "bit", nullable: false),
                    Nota = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "DesglosesBilletes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denominacion = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TurnoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesglosesBilletes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesglosesBilletes_Turnos_TurnoId",
                        column: x => x.TurnoId,
                        principalTable: "Turnos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DetalleTurnos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TurnoId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ingreso = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Egreso = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleTurnos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleTurnos_Turnos_TurnoId",
                        column: x => x.TurnoId,
                        principalTable: "Turnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abierta = table.Column<bool>(type: "bit", nullable: false),
                    Anulada = table.Column<bool>(type: "bit", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: true),
                    ClientePId = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NombreCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RNCCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelefonoCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorreoCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoSocio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreVendedor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EsContado = table.Column<bool>(type: "bit", nullable: false),
                    TurnoId = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ITBIS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Condicion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NCF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoNCF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Consumo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiasCredito = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Efectivo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tarjeta = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Transferencia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cheque = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPagado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPendiente = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PagoCon = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Propina = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiasPlazo = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cambio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facturas_Entidad_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Entidad",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Facturas_Personas_ClientePId",
                        column: x => x.ClientePId,
                        principalTable: "Personas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Facturas_Turnos_TurnoId",
                        column: x => x.TurnoId,
                        principalTable: "Turnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Facturas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "xd",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecepcionLecheId = table.Column<int>(type: "int", nullable: false),
                    ProveedorId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Litros = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Grasa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tanda = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreezerId = table.Column<int>(type: "int", nullable: true),
                    SolidosTotales = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioPorLitro = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Nulo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_xd", x => x.Id);
                    table.ForeignKey(
                        name: "FK_xd_Entidad_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "Entidad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_xd_f_FreezerId",
                        column: x => x.FreezerId,
                        principalTable: "f",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_xd_x_RecepcionLecheId",
                        column: x => x.RecepcionLecheId,
                        principalTable: "x",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transacciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Capital = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Interes = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Otros = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoCredito = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoDebito = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Medio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Referencia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaOriginal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CuentaId = table.Column<int>(type: "int", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Caja = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nula = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacciones_Cuentas_CuentaId",
                        column: x => x.CuentaId,
                        principalTable: "Cuentas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MantVehiculos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Odometro = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProximoOdometro = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaProximo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VehiculoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MantVehiculos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MantVehiculos_Vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "Vehiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoteId = table.Column<int>(type: "int", nullable: true),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Existencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Existencias_Lotes_LoteId",
                        column: x => x.LoteId,
                        principalTable: "Lotes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Existencias_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CxCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: true),
                    Cliente2Id = table.Column<int>(type: "int", nullable: true),
                    NumeroFactura = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacturaId = table.Column<int>(type: "int", nullable: true),
                    Titular = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiasCredito = table.Column<int>(type: "int", nullable: false),
                    MontoFactura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MontoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoPendiente = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Pagado = table.Column<bool>(type: "bit", nullable: false),
                    Anulado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CxCs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CxCs_Entidad_Cliente2Id",
                        column: x => x.Cliente2Id,
                        principalTable: "Entidad",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CxCs_Facturas_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Facturas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CxCs_Personas_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Personas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DetalleFacturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FacturaId = table.Column<int>(type: "int", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: true),
                    NombreProducto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnidadMedida = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PorcentajeITBIS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ITBIS = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Descuento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleFacturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleFacturas_Facturas_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Facturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleFacturas_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PagosCxC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CxCId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pagado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FormaPago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Referencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Banco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Anulado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagosCxC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagosCxC_CxCs_CxCId",
                        column: x => x.CxCId,
                        principalTable: "CxCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PagosCxC_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compras_ProveedorId",
                table: "Compras",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_UsuarioId",
                table: "Compras",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionLocals_usuarioId",
                table: "ConfiguracionLocals",
                column: "usuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ContInventarios_UsuarioId",
                table: "ContInventarios",
                column: "UsuarioId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_TipoCuentaId",
                table: "Cuentas",
                column: "TipoCuentaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cuentas_TitularId",
                table: "Cuentas",
                column: "TitularId");

            migrationBuilder.CreateIndex(
                name: "IX_CxCs_Cliente2Id",
                table: "CxCs",
                column: "Cliente2Id");

            migrationBuilder.CreateIndex(
                name: "IX_CxCs_ClienteId",
                table: "CxCs",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_CxCs_FacturaId",
                table: "CxCs",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_DesglosesBilletes_TurnoId",
                table: "DesglosesBilletes",
                column: "TurnoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompras_CompraId",
                table: "DetalleCompras",
                column: "CompraId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleCompras_ProductoId",
                table: "DetalleCompras",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleContInventarios_ContInventarioId",
                table: "DetalleContInventarios",
                column: "ContInventarioId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleContInventarios_ProductoId",
                table: "DetalleContInventarios",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFacturas_FacturaId",
                table: "DetalleFacturas",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleFacturas_ProductoId",
                table: "DetalleFacturas",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesLiquidacionLeche_LiquidacionLecheId",
                table: "DetallesLiquidacionLeche",
                column: "LiquidacionLecheId");

            migrationBuilder.CreateIndex(
                name: "IX_DetallesProducto_ProductoId",
                table: "DetallesProducto",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleTurnos_TurnoId",
                table: "DetalleTurnos",
                column: "TurnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Entidad_TipoEntidadAsoId",
                table: "Entidad",
                column: "TipoEntidadAsoId");

            migrationBuilder.CreateIndex(
                name: "IX_Existencias_LoteId",
                table: "Existencias",
                column: "LoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Existencias_ProductoId",
                table: "Existencias",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_ClienteId",
                table: "Facturas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_ClientePId",
                table: "Facturas",
                column: "ClientePId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_TurnoId",
                table: "Facturas",
                column: "TurnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_UsuarioId",
                table: "Facturas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_fd_FrezzerId",
                table: "fd",
                column: "FrezzerId");

            migrationBuilder.CreateIndex(
                name: "IX_LiquidacionesLeche_UsuarioId",
                table: "LiquidacionesLeche",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_UsuarioId",
                table: "Logs",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Lotes_ProductoId",
                table: "Lotes",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Lotes_UsuarioId",
                table: "Lotes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_MantVehiculos_VehiculoId",
                table: "MantVehiculos",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_PagosCxC_CxCId",
                table: "PagosCxC",
                column: "CxCId");

            migrationBuilder.CreateIndex(
                name: "IX_PagosCxC_UsuarioId",
                table: "PagosCxC",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_TipoEntidadId",
                table: "Personas",
                column: "TipoEntidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CategoriaId",
                table: "Productos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_UnidadCompraId",
                table: "Productos",
                column: "UnidadCompraId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_UnidadId",
                table: "Productos",
                column: "UnidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_CuentaId",
                table: "Transacciones",
                column: "CuentaId");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_UsuarioId",
                table: "Turnos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_PersonaId",
                table: "Vehiculos",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_x_UsuarioId",
                table: "x",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_xd_FreezerId",
                table: "xd",
                column: "FreezerId");

            migrationBuilder.CreateIndex(
                name: "IX_xd_ProveedorId",
                table: "xd",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_xd_RecepcionLecheId",
                table: "xd",
                column: "RecepcionLecheId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracionLocals");

            migrationBuilder.DropTable(
                name: "Configuracions");

            migrationBuilder.DropTable(
                name: "ContratosTractores");

            migrationBuilder.DropTable(
                name: "DesglosesBilletes");

            migrationBuilder.DropTable(
                name: "DetalleCompras");

            migrationBuilder.DropTable(
                name: "DetalleContInventarios");

            migrationBuilder.DropTable(
                name: "DetalleFacturas");

            migrationBuilder.DropTable(
                name: "DetallesLiquidacionLeche");

            migrationBuilder.DropTable(
                name: "DetallesProducto");

            migrationBuilder.DropTable(
                name: "DetalleTurnos");

            migrationBuilder.DropTable(
                name: "Existencias");

            migrationBuilder.DropTable(
                name: "fd");

            migrationBuilder.DropTable(
                name: "Impuestos");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "MantVehiculos");

            migrationBuilder.DropTable(
                name: "PagosCxC");

            migrationBuilder.DropTable(
                name: "Transacciones");

            migrationBuilder.DropTable(
                name: "xd");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "TipoServicioTractores");

            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.DropTable(
                name: "ContInventarios");

            migrationBuilder.DropTable(
                name: "LiquidacionesLeche");

            migrationBuilder.DropTable(
                name: "Lotes");

            migrationBuilder.DropTable(
                name: "Vehiculos");

            migrationBuilder.DropTable(
                name: "CxCs");

            migrationBuilder.DropTable(
                name: "Cuentas");

            migrationBuilder.DropTable(
                name: "f");

            migrationBuilder.DropTable(
                name: "x");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "TipoCuentas");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "UnidadMedidas");

            migrationBuilder.DropTable(
                name: "Entidad");

            migrationBuilder.DropTable(
                name: "Personas");

            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.DropTable(
                name: "EntidadesASOs");

            migrationBuilder.DropTable(
                name: "TipoEntidades");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
