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
    /// Lógica de interacción para VLoteVenta.xaml
    /// </summary>
    public partial class VLoteVenta : Window
    { public string codlote=string.Empty;
        private List<Lote> Lotes { get; set;  }
        public VLoteVenta(List<Lote> lotes)
        {
            InitializeComponent();
            Lotes = lotes;
            txtnombre.Text = lotes.First().Producto.Nombre;
          //  MessageBox.Show(lotes.FirstOrDefault().CodigoLote);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtlote.Text))
            {
                codlote = txtlote.Text;

                if (Lotes.FirstOrDefault(x=>x.CodigoLote==codlote)!=null)
                {
                    DialogResult = true;

                }
                else
                {
                    MessageBox.Show("No coincide con ningún lote registrado");
                }
            }
            else
            {
                MessageBox.Show("Es necesario un código de lote");
                return;
            }

        }
    }
}
