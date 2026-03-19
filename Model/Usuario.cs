using Newtonsoft.Json.Bson;
using SFCH.PrintView;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SFCH.Model
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreUsuario { get; set; } = string.Empty; // Username/Login

        [Required]
        [StringLength(200)]
        public string ContrasenaHash { get; set; } = string.Empty; // Hash de la contraseña

        [Required]
        [StringLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;
        [Required]
        [StringLength(13)]
        public string Cedula { get; set; } = string.Empty;

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(15)]
        public string Telefono { get; set; } = string.Empty;

        // Rol o permisos en el sistema
        [Required]
        [StringLength(50)]
        public string Rol { get; set; } = "Cajero";
        // Ejemplos: Administrador, Cajero, Supervisor, Contador

        // Estado de la cuenta
        public bool Activo { get; set; } = true;

        // Auditoría
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public DateTime? UltimoAcceso { get; set; }
        public bool Administrador { get; set; }
        public bool Caja { get; set; }
        public bool Reportes { get; set; }
        public bool Cuentas { get; set; }
        public bool Entidades { get; set; }
        public bool Inventario { get; set; }
        public bool Compras { get; set; }
        public bool Productos { get; set; }
        public bool FacACredito { get; set; }=false;
        public bool PuntoVentas { get; set; }
        public bool ModificarPrecios { get; set; }
        public bool AnularFacturas { get; set; }
        public bool CerrarTurno { get; set; }
        public bool AbrirTurno { get; set; }
        public bool Facturacion { get; set; }
        public bool Configuracion { get; set; }
        public bool Herramientas { get; set; }
        public bool Usuarios { get; set; }
        public bool Contabilidad{ get; set; }
        public bool AnularTrasnas { get; set; } 
        public virtual List<Turno> Turnos { get; set; } = new List<Turno>();
        public virtual List<Factura> Facturas { get; set; } = new List<Factura>();
        public virtual List<Compra> RCompras { get; set; } = new List<Compra>();
        public virtual List<Log> Logs { get; set; } = new List<Log>();
     public bool Tractor { get; set;  }
        public bool Acopio { get; set; }
        public override string ToString()
        {
            return $"{NombreCompleto}";
        }
        
    }
}
