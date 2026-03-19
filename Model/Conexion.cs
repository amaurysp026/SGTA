using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.Model
{
    public class Conexion : DbContext
    {
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<ConfiguracionLocal> ConfiguracionLocals { get; set; }
        public virtual DbSet<Persona> Personas { get; set; }
        public virtual DbSet<TipoCuenta> TipoCuentas { get; set; }
        public virtual DbSet<Cuenta> Cuentas { get; set; }
        public virtual DbSet<Transaccion> Transacciones { get; set; }
        public virtual DbSet<TipoEntidad> TipoEntidades { get; set; }
        public virtual DbSet<Configuracion> Configuracions { get; set; }
        public virtual DbSet<Categoria> Categorias { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<UnidadMedida> UnidadMedidas { get; set; }
        public virtual DbSet<Factura> Facturas { get; set; }
        public virtual DbSet<DetalleFactura> DetalleFacturas { get; set; }
        public virtual DbSet<Turno> Turnos { get; set; }
        public virtual DbSet<Impuesto> Impuestos { get; set; }
        public virtual DbSet<CxC> CxCs { get; set; }
        public virtual DbSet<DetalleTurno> DetalleTurnos { get; set; }
        public virtual DbSet<Proveedor> Proveedores { get; set; }
        public virtual DbSet<Compra> Compras { get; set; }
        public virtual DbSet<DetalleCompra> DetalleCompras { get; set; }
        public virtual DbSet<DetalleFrezzer> DetalleFrezzers { get; set; }
        public virtual DbSet<Freezer> Frezzers { get; set; }    
        public virtual DbSet<RecepcionLeche> RecepcionLeches { get; set; }
        public virtual DbSet<DetalleRecepcionLeche> DetalleRecepcionLeches { get; set; }
        public virtual DbSet<TipoEntidadAso> EntidadesASOs { get; set; }
        public virtual DbSet<Entidad> Entidad { get; set; }
        public virtual DbSet<Existencia> Existencias { get; set; }
        public virtual DbSet<Lote> Lotes { get; set; }
        public virtual DbSet<Empleado> Empleados { get; set; }
        public virtual DbSet<TipoServicioTractor> TipoServicioTractores { get; set; }
        public virtual DbSet<ContratoTractor> ContratosTractores { get; set; }
        public virtual DbSet<LiquidacionLeche> LiquidacionesLeche { get; set; }
        public virtual DbSet<DetalleLiquidacionLeche> DetallesLiquidacionLeche { get; set; }
        public virtual DbSet<PagoCxC> PagosCxC { get; set; }
        public virtual DbSet<DetalleProducto> DetallesProducto { get; set; }
        public DbSet<DesgloseBilletes> DesglosesBilletes { get; set; }
        public DbSet<ContInventario> ContInventarios { get; set; }
        public DbSet<DetalleContInventario> DetalleContInventarios { get; set; }





        string connectionString = string.Empty;
        public Conexion()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
            //  MessageBox.Show("Sin parametros"+connectionString);
        }

        public Conexion(DbContextOptions<Conexion> options)
            : base(options)
        {

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
            //  MessageBox.Show("Con Parametros"+connectionString);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging(false);
            }
        }
    }
}
