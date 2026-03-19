using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.IdentityModel.Tokens;
using SFCH.Controller;
using SFCH.IService;
using SFCH.Logica;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
    /// Lógica de interacción para VEgresoCaja.xaml
    /// </summary>
    public partial class VEgresoCaja : Window
    {
        ITurno turno = new  TurnoService();
        public List<DetalleTurno> detalleTurnos { get; set; } = new List<DetalleTurno>();
        public Turno tturno { get; set; }
        public VEgresoCaja()
        {
            InitializeComponent();
           Window_Loaded();
        }
        private async void Window_Loaded()
        {
      
            var turnoAbierto = await turno.ObtenerTurnosAsync();
          var turnoact = turnoAbierto.Find(x=>x.Id==SesionUsuario.TurnoActual.Id);
            if (turnoact != null) {
                tturno = turnoact;
              //  MessageBox.Show( $"Turno Abierto: {SesionUsuario.TurnoActual.Id}");
                txttotal.Text = turnoact.DetalleTurnos.Sum(x=>x.Egreso).ToString("C");
                data.ItemsSource = turnoact.DetalleTurnos;
                detalleTurnos= turnoact.DetalleTurnos;
            }
            else
             {
                 MessageBox.Show("No hay turno abierto");
                 this.Close();
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();   
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            txtmonto.Text = "";
            txtnombre.Text = "";
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(txtmonto.Text,out decimal Monto)||Monto<=0)
            {
                MessageBox.Show("El monto es incorrecto");
                return;
            }
            if ((tturno.Facturas.Where(X=>X.Condicion=="Contado").Sum(x=>x.Efectivo)+tturno.TotalInicial)>(Monto- detalleTurnos.Sum(x => x.Egreso)))
            {
                var s = await turno.RegistrarEgreso(txtnombre.Text,Monto);
                if (s != null)
                {
                    //detalleTurnos.Add(s);
                    
                  data.Items.Refresh();
                    txttotal.Text = detalleTurnos.Sum(x => x.Egreso).ToString("C");
                }
            }
            else
            {
                MessageBox.Show("Efectivo insuficiente para la transacción");
                return;
            }
          //  Window_Loaded();
          // MessageBox.Show(s.Id.ToString()) ;
        }
    }
}
