using SFCH.Controller;
using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;

namespace SFCH.Model
{
    public class ContratoTractor
    {
        public int Id { get; set; } 
        public string Numero { get; set; }=null!;
        public DateTime Fecha { get; set; }= DateTime.Now;
        public virtual Entidad Entidad { get; set; }=null!;
        public virtual Turno Turno { get; set; }=null!;
        public virtual Empleado Empleado { get; set; }=null!;
        public string Estado { get; set; }= "ABIERTA";
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public virtual TipoServicioTractor TipoServicioTractor { get; set; }=null!;
        public bool ConMuro { get; set; }=false;
        public bool ConArado { get; set; }=false;
        public string Otros { get; set;} = string.Empty;
        public string Usuario { get; set; } = SesionUsuario.Usuario.NombreCompleto;
        public decimal Tarea { get; set; }
        public decimal Precio { get; set; }
        public decimal Total { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal TotalPendiente { get; set; }
        public bool Credito { get; set; }
        public bool Nulo { get; set; }
        public string? Nota { get; set; }

    }
}
