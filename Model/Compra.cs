using SFCH.Controller;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace SFCH.Model
{
    public class Compra:INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string? NumeroCompra { get
            {
                return $"C-{Id:000000}";
            }
            set {
            field = value;
                OnPropertyChanged(NumeroFactura!);
            } }
        public string? NumeroFactura { get { return field; } set { field = value; OnPropertyChanged("NumeroFactura"); } }
        public DateTime FechaCompra { get; set; } = DateTime.Now;
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        public decimal TasaCambio { get; set; }
        public decimal TotalUSD { get; set; }
        public decimal Total { get; set; }
        public decimal MontoPagado { get; set; }
        public decimal MontoPendiente { get; set; }
        public virtual Proveedor Proveedor { get; set; } = null!;
        public string Descripcion { get; set; } = string.Empty;
        public Usuario Usuario { get; set; } = SesionUsuario.Usuario;
        public ObservableCollection<DetalleCompra> Detalles { get; set; } = new ObservableCollection<DetalleCompra>();

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool Nula { get; set; } = false;
        public string? Color { get; set;  }
    }
}
