using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SGTA.Model
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public string Placa { get; set; }=null!;
        public string Marca { get; set; } = "";
        public string Modelo { get; set; } = "";
        public string anno { get; set; } = "";
        public string FiltroAceite { get; set; } = "";
        public string FiltroAire { get; set; } = "";
        public string FiltroCombustible { get; set; } = "";
        public string FiltroCabina { get; set; } = "";
        public string Color { get; set; }   = string.Empty;
        public bool EnTaller { get; set; }=true;
        public virtual Persona? Persona { get; set; }
        public virtual List<MantVehiculo> MantVehiculos { get; set; } = new List<MantVehiculo>();
    }
}
