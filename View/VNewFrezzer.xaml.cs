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
    /// Lógica de interacción para VNewFrezzer.xaml
    /// </summary>
    public partial class VNewFrezzer : Window
    {
        public f freezerrespuesta { get; set; }
        public bool Editando { get; set; } = false; 
        public VNewFrezzer( f freezer)
        {
            InitializeComponent();
            freezerrespuesta = freezer;
            txtcapacidad.Text=freezerrespuesta.CapacidadTotal.ToString();
            txtdescripcion.Text = freezerrespuesta.Descripcion;
            txtNumero.Text = freezerrespuesta.Numero;
            Editando= true;
        }
        public VNewFrezzer()
        {
            InitializeComponent();
            freezerrespuesta = new f();
            txtcapacidad.Text = freezerrespuesta.CapacidadTotal.ToString();
            txtdescripcion.Text = freezerrespuesta.Descripcion;
            txtNumero.Text = freezerrespuesta.Numero;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(txtcapacidad.Text.Trim(),out decimal capacidad))
            {
                MessageBox.Show("La capacidad tiene que ser un numero", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtdescripcion.Text))
            {
                MessageBox.Show("La descripción es obligatoria", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (capacidad<1m)
            {
                MessageBox.Show("La capacidad debe de ser mayor que 1 litro", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
         
                freezerrespuesta.Descripcion=txtdescripcion.Text.Trim();
                freezerrespuesta.CapacidadTotal = capacidad;
                freezerrespuesta.Numero=txtNumero.Text.Trim();

            DialogResult = true;
        }
    }
}
