using Microsoft.EntityFrameworkCore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para PRIngresosXmes.xaml
    /// </summary>
    public partial class PRIngresosXmes : Page
    {
        record Reporte(string Mes, decimal Total);
        List<Reporte> reportes = new List<Reporte>();
        //Yasmina Khan
        public PRIngresosXmes()
        {
            InitializeComponent();
            Cargar();
        }
        private async void Cargar()
        {
           using (var db = new Model.Conexion())
            {
                var t = await db.Transacciones.Where(x => x.Nula == false).ToListAsync();
                for (var Mes = 1; Mes <= 12; Mes++)
                {
                  //  MessageBox.Show("Nombre Mes: " + new DateTime(1, Mes, 1).ToString("MMMM") + t.Where(x => x.Fecha.Month == Mes).Sum(x => x.Monto).ToString());
                    reportes.Add(new Reporte(new DateTime(1, Mes, 1).ToString("MMMM"), t.Where(x => x.Fecha.Month == Mes).Sum(x => x.MontoCredito)));
                }
                reportes.Add(new Reporte("Total Anual", t.Sum(x => x.MontoCredito)));
                data.ItemsSource = null;
                data.ItemsSource = reportes;


            }
        }
        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
