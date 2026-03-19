using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFCH.Model
{
    public class Configuracion
    {
        public int Id { get; set; }
        public string NombreEmpresa { get; set; } = string.Empty;
        public string RNC { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string LogoPath { get; set; } = string.Empty;
        public string Moneda { get; set; } = "USD";
        public int Decimales { get; set; } = 2;
        public bool MostrarLogoEnReportes { get; set; } = true;
        public bool UsarImpuestos { get; set; } = false;
        public decimal TasaImpuesto { get; set; } = 0.0m;
        public bool HabilitarRedondeo { get; set; } = false;
        public bool DiasBackup { get; set; } = false;
        public bool CierreAutoTurno { get; set; } = false;
        public bool CiereAutoRecepcionLeche { get; set; } = false;
        public bool LitroMaximo { get; set; } = false;
        public bool UsarDolar { get; set; } = false;
        public decimal TasaRedondeo { get; set; } = 0.0m;
        public string FormatoFecha { get; set; } = "dd/MM/yyyy";
        public string FormatoHora { get; set; } = "HH:mm:ss";
        public byte[]? LogoPrincipal { get; set; } // Propiedad para almacenar la imagen
        public byte[]? Timbrado { get; set; }
        public bool VenderSinExistencia { get; set; } = false;
        public string MesnajeBienvenida { get; internal set; }
        public string NombreComercial { get; internal set; }
        public string? MesnajeFinal1 { get; internal set; }
        public string? MesnajeFinal2 { get; internal set; }
        public bool FacGrade { get; set; }=false;
        public decimal TasaCambio { get;set; }
        public decimal TasaCambio2 { get; set; }
    }
}
