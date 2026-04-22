using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SFCH.Model
{
    public class x
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }=DateTime.Now;
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        public string Tanda { get; set; } = string.Empty;
        public ObservableCollection<xd> Detalles { get; set; } = new ObservableCollection<xd>();
        public Usuario Usuario { get; set; } = null!;
        public string? Color { get; set; }
        public string ? CodSeguridad { get; set; }
        public string Observacion { get; set; } = string.Empty;
    }
}
