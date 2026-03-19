using SFCH.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SFCH.Model
{
    public class DetalleFactura: INotifyPropertyChanged
    {
        public int Id { get; set; }
        public virtual Factura Factura { get; set; } = null!;
        public DateTime FechaEmision { get; set; }=DateTime.Now;
        public virtual Producto? Producto { get; set; } =null;
        public string NombreProducto { get; set; } = null!;
        private decimal _precioUnitario;
        public decimal PrecioUnitario
        {
            get { return _precioUnitario; }
            set
            {
                   _precioUnitario = value;
                    OnPropertyChanged(nameof(PrecioUnitario));
            }
        }
        public string UnidadMedida { get; set; } = null!;
        public decimal CostoUnitario { get; set; }
        public decimal Cantidad
            {
            get { return field; }
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(Cantidad));
                }
            }
        }
        public decimal SubTotal
        {
            get { return field; }
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(SubTotal));
                }
            }
        }
        public decimal PorcentajeITBIS { get; set; }
        public decimal ITBIS
        {
            get { return field; }
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(ITBIS));
                }
            }
        }
        //private decimal _itbis;

        public decimal Descuento
        {
            get { return field; }
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(Descuento));
                }
            }
        }
        
        public decimal Total
        {
            get { return field; }
            set
            {

                if (field != value)
                {

                    field = value;

                    OnPropertyChanged(nameof(Total));
                }
            }

        }
        private void CalcularTotal()
        {
             var nitibis = (PorcentajeITBIS / 100)+1;
          //  MessageBox.Show("NITBIS: "+ nitibis);

            SubTotal = (PrecioUnitario /nitibis)*Cantidad;

            ITBIS = (SubTotal * PorcentajeITBIS / 100);
            Total = SubTotal + ITBIS - Descuento;
    
        }
        private void OnPropertyChanged(string v)
        {
            CalcularTotal();


            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));


        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
