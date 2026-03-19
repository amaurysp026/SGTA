using SFCH.IService;
using SFCH.Logica;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
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
    /// Lógica de interacción para VTransaccion.xaml
    /// </summary>
    public partial class VTransaccion : Window
    {
        private IPersona persona = new PersonaService();
        private ICuenta cuenta = new CuentaService();
        private Persona Persona { get; set; }
        public Transaccion transaccion { get; set; }
        public VTransaccion()
        {
            InitializeComponent();
           // transaccion = new Transaccion
           // {
           //     Cuenta = cuen.FirstOrDefault()!,
           //     Cliente = c,
           //     MontoCredito = 350.00m,
           //     Tipo = "CREDITO",
           //     Fecha = DateTime.Now,
           //     Caja = "1",
           //     Descripcion = "Deps",
           //     Referencia = "1",
           //     Usuario = "d",
           //     Otros = 0
           // };
          
            carga();

            txtdescripcion.Text = "Descuento 5% ASOGAFAR para aportación";
            
        }
        public async void carga()
        {
            txtfecha.SelectedDate = DateTime.Now;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var cuen = await cuenta.ObtenerCuentas();
            if (CbCuentas.SelectedItem is Cuenta cc)
            {


                if (decimal.TryParse(txtmonto.Text ,out decimal mont))
                {
                    decimal montocapital = 0;
                    if (chCapital.IsChecked.Value)
                    {
                        montocapital = mont;
                    }
                    bool h = await cuenta.TrasnIngreso(new Transaccion
                    {
                        Cuenta = cc,
                        Cliente = txtNombre.Text,
                        MontoCredito = mont ,
                        Monto = mont,
                        Capital =montocapital,
                        Tipo = "CREDITO",
                        Fecha = txtfecha.SelectedDate.Value,
                        Caja = "1",
                        Descripcion = txtdescripcion.Text,
                        Referencia = txtdescripcion.Text,
                        Usuario = "Rossy",
                        Otros = 0,
                       
                    });
                    if (true)
                    {
                        DialogResult = h;
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione una cuenta");
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        { string t = (sender as TextBox)?.Text??"";
            string can = "01201171517";
            if (t.Length==can.Length)
            {
                persona.ObtenerPersonaPorCedula(t).ContinueWith(task =>
                {
                    var p = task.Result;
                    Dispatcher.Invoke(() =>
                    {
                        if (p != null)
                        {
                            Persona = p;
                            txtNombre.Text = p.Nombre+" " +p.Apellido;
                           CbCuentas.ItemsSource = null;
                            CbCuentas.ItemsSource = p.Cuentas;
                            CbCuentas.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Persona no encontrada");
                            txtNombre.Text = "";
                            txtId.Text = "";
                        }
                    });
                });
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void txtCodigoA_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
