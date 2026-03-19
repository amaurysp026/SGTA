using Microsoft.EntityFrameworkCore;
using SFCH.IService;
using SFCH.Logica;
using SFCH.Model;
using SFCH.PrintView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para PTransacciones.xaml
    /// </summary>
    public partial class PTransacciones : Page
    {
        private ICuenta cuenta = new CuentaService();
        public PTransacciones()
        {
            InitializeComponent();
            
            using (var db = new Model.Conexion())
            {
                var t = db.Transacciones.Where(x=>x.Nula==false).Include(x=>x.Cuenta).ThenInclude(x=>x.Titular).ToList();
                //for (var Mes=1;Mes<=12;Mes++)
                //{
                    
                //   MessageBox.Show("Nombre Mes: " + new DateTime(1, Mes, 1).ToString("MMMM") + t.Where(x=>x.Fecha.Month==Mes).Sum(x=>x.Monto).ToString());
                //}
                data.ItemsSource = t;
            }
            
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            PPuntoVen pPuntoVen = new PPuntoVen((Transaccion)data.SelectedItem);
                
        }

        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (data.SelectedItem is Transaccion t)
            {
                Autorizacion autorizacion = new Autorizacion();
                if (autorizacion.ShowDialog() == true)
                {
                    if (await cuenta.AnularTransa(t))
                    {
                        
                    }
                }
                    
                
            }
        }
    }
}
