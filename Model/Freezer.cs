using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Model
{
    public class Freezer
    {
        public int Id { get; set; }
        public string Numero { get; set; } = null!;
        public string Descripcion { get; set; } = string.Empty;
        public decimal CapacidadTotal { get; set; }
        public virtual List<DetalleRecepcionLeche> Recepciones { get; set; } = new();
        public virtual List<DetalleFrezzer> Detalle { get; set; } = new();
        public decimal TotalRegistrdo => Recepciones.Sum(x => x.Litros);
        public decimal TotalActual => Detalle.Sum(x => x.Entrada)-Detalle.Sum(x => x.Salida);
        public bool Activo { get; set; } = true;
    }
}
