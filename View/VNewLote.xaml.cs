using SFCH.Controller;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para VNewLote.xaml
    /// </summary>
    public partial class VNewLote : Window
    {
        public List<Lote> loteList { get; set; } = new List<Lote>();
        public VNewLote(Producto producto)
        {
            InitializeComponent();
            txtNombreProducto.Text = producto.Nombre;
            data.ItemsSource = loteList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(txtcantidad.Text.Trim(), out decimal Cantidad)||Cantidad<=0)
            {
                MessageBox.Show("La cantidad tiene que ser un numero mayor a 0");
                return;
            }
            if (fechavence.SelectedDate<DateTime.Now.Date)
            {
                MessageBox.Show("La fecha de vencimiento no puede ser igual o mayor que la fecha actual");
                return;
            }

            var lote = new Lote
            {
                Usuario = SesionUsuario.Usuario,
                FechaEntrada=DateTime.Now,
                CodigoLote=txtlote.Text,
                Cantidad=Cantidad,
                FechaFabricacion=fechafabrica.SelectedDate,
                FechaVencimiento=fechavence.SelectedDate??DateTime.Now,
                Color="",
                
            };
            loteList.Add(lote);
            data.ItemsSource = null;
            data.ItemsSource = loteList;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (loteList.Count()==0)
            {
                MessageBox.Show("No existe lotes");
                return;
            }
            if (MessageBox.Show("Deseas terminar y guardar los lotes en producto?","Aviso!",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                DialogResult = true;
            }
            return;

        }
    }
}
