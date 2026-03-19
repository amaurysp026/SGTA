using SFCH.Controller;
using SFCH.IService;
using SFCH.Logica;
using SFCH.Migrations;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Compra = SFCH.Model.Compra;

namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para PCompra.xaml
    /// </summary>
    public partial class PCompra : Page, INotifyPropertyChanged
    {
        private ICompra Icompra = new CompraService();
        public Compra Compra
        {
            get
            {
                return field;

            }
            set
            {
                field = value;
                OnPropertyChanged(nameof(Compra));

            }
        } = new Compra();
        
        public DetalleCompra detalle
        {
            get
            {
                return field;

            }
            set
            {
                field = value;
                OnPropertyChanged(nameof(detalle));

            }
        } = new DetalleCompra();

        public PCompra()
        {
            InitializeComponent();

            this.DataContext = this;

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            
        }
        public void CalcularTotales()
        {
            var valUSd= Compra.Detalles.Sum(x => x.SubttalUSD);
           
                Compra.Total = valUSd*Compra.TasaCambio;
                Compra.TotalUSD = valUSd;
                Compra.Total = Compra.Detalles.Sum(x => x.Subtotal);
           

            Compra.MontoPendiente = Compra.Total;
            Compra.MontoPagado = 0;
            OnPropertyChanged(nameof(Compra));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (detalle.Producto == null)
            {

                VBuscarProducto vBuscarProducto = new VBuscarProducto();
                var res = vBuscarProducto.ShowDialog();
                if (vBuscarProducto.ProductoSeleccionado==null)
                {
                    return;
                }
                if (vBuscarProducto.ProductoSeleccionado.EsServicio)
                {
                    MessageBox.Show("El producto seleccionado es un servicio y no se permite en compras"); return;
                }

                if (res == true) { detalle.Producto = vBuscarProducto.ProductoSeleccionado; }
                decimal cant = 1m;
                if (detalle.Producto.Vence)
                {
                    MessageBox.Show("El producto tiene vencimiento, agregar los lotes correspondientes","Aviso!",MessageBoxButton.OK,MessageBoxImage.Question);
                    VNewLote vNewLote = new VNewLote(detalle.Producto);
                    vNewLote.ShowDialog();
                    if (vNewLote.loteList.Count() > 0)
                    {
                        foreach (var item in vNewLote.loteList)
                        {
                            item.Producto = detalle.Producto;
                        }
                        detalle.Producto.lotes = vNewLote.loteList;
                        cant = vNewLote.loteList.Sum(x=>x.Cantidad);
                    }
                }
              
                detalle.PrecioUnitario = detalle.Producto.Costo;
                if (Compra.TasaCambio > 0)
                {
                    detalle.PrecioUSD = detalle.Producto.CostoUSD;

                }
                detalle.Tasa = Compra.TasaCambio;
                detalle.Cantidad = cant;
                OnPropertyChanged(nameof(detalle));
                return;
            }
            if (detalle.Cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a cero");
                return;
            }
            if (detalle.PrecioUnitario <= 0)
            {
                MessageBox.Show("El precio unitario debe ser mayor a cero");
                return;
            }
            if (detalle.Producto.Vence&&detalle.Cantidad!=detalle.Producto.lotes.Sum(x=>x.Cantidad))
            {
                MessageBox.Show("Incoherencia en datos, La cantidad en lotes es diferente a la cantidad de compra.");
                return;
            }

            Compra.Detalles.Add(detalle);
            CalcularTotales();

            //dCompra.Add(detalle);
            detalle = new DetalleCompra();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            VBuscarProveedores vBuscarProveedores = new VBuscarProveedores();
            var res = vBuscarProveedores.ShowDialog();
            if (res == true)
            {
               
                Compra.Proveedor = vBuscarProveedores.ProveedorSeleccionado; OnPropertyChanged(nameof(Compra));
            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {

            var res = await Icompra.GuardarCompra(Compra);
            if (res)
            {
                MessageBox.Show("Compra registrada con éxito","Aviso!",MessageBoxButton.OK,MessageBoxImage.Information);
                Compra = new Compra();
                OnPropertyChanged(nameof(Compra));
            }
            else
            {
                MessageBox.Show("Error al guardar la compra", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            VPagina vPagina = new VPagina(new PCompras());
            vPagina.Show();
        }

        private void btnnuevoproducto_Click(object sender, RoutedEventArgs e)
        {
            VNewProducto vNewProducto = new VNewProducto();
            vNewProducto.Show();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Compra.TasaCambio==0)
            {
            //MessageBox.Show(detalle.PrecioUnitario.ToString());
                return;

            }
            detalle.PrecioUnitario = detalle.PrecioUSD * Compra.TasaCambio;
           // MessageBox.Show(detalle.PrecioUnitario.ToString());
        }
    }
}
