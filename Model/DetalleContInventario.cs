using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SFCH.Model
{
    public class DetalleContInventario : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public virtual ContInventario ContInventario{ get; set; }
        public virtual Producto Producto { get; set; } = null!;
        public decimal Costo { get; set; } 
        public decimal ValorDiferencia => Diferencia * Costo;
        public decimal CantSistema { get; set; }
        public decimal CantFisico {
            get
            {
                return field; 
            }
            set { 
               
                    field = value;
                    OnPropertyChanged(nameof(CantFisico));
                
            }
        }
        public decimal Diferencia { get; set; }
        public decimal DiferenciaCalculada => CantFisico- CantSistema;
        public string? Contador { get; set; }
        public string? Observaciones { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            Diferencia =  CantFisico- CantSistema;
           
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
       
        }
    }
}
