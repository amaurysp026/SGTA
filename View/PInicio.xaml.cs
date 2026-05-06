using Microsoft.EntityFrameworkCore;
using SFCH.Controller;
using SFCH.Model;
using SGTA.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Collections.Specialized.BitVector32;

namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para PInicio.xaml
    /// </summary>
    public partial class PInicio : Page
    {
        public class Dashboard
        {
            public int Totalvehiculostaller { get; set; }
            public int Totalfaturasestemes { get;   set; }
            public decimal TotaldeCobro { get; set; }
            public decimal TotalIngresosMes { get; set; } = 0;
            public int Totalfacturasincobrar { get; set; }
            public int Totalvehiculosurgentes { get; set; }
            public List<Vehiculo> VehiculosTaller { get; set; } = new List<Vehiculo>();
        }
        public Dashboard dashboard { get; set; }=new Dashboard();
        public PInicio()
        {
            InitializeComponent();

             Cargar();
            
        }
        public async Task<string> ConsultarApiAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Configurar headers básicos
                    client.DefaultRequestHeaders.Add("User-Agent", "MiAplicacion/1.0");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");

                    // Realizar la petición GET
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Asegurar que la respuesta sea exitosa
                    response.EnsureSuccessStatusCode();

                    // Leer el contenido como string
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    return jsonResponse;
                }
                catch (HttpRequestException ex)
                {
                    throw new Exception($"Error al consultar la API: {ex.Message}", ex);
                }
            }
        }

        private bool IsAdministrator()
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }
        private async void Cargar()
        {

            try
            {
                using (var db=new Conexion())
                {
                    dashboard = new Dashboard
                    {
                        Totalvehiculostaller = await db.Vehiculos.Where(x => x.EnTaller == true).CountAsync(),
                        Totalfaturasestemes = await db.Facturas.Where(x=>x.Abierta==false).CountAsync(),
                        TotaldeCobro = await db.Facturas.Where(x => x.TotalPendiente > 0).SumAsync(x => x.TotalPendiente)
                        ,
                        Totalfacturasincobrar = await db.Facturas.Where(x => x.TotalPendiente > 0).CountAsync(),
                        Totalvehiculosurgentes = await db.Vehiculos.Where(x => x.EnTaller == true && x.Estado == "Urgente").CountAsync(),
                        TotalIngresosMes= await db.Facturas.Where(x =>x.Abierta==false&& x.FechaEmision.Month == DateTime.Now.Month && x.FechaEmision.Year == DateTime.Now.Year).SumAsync(x => x.Total)
                    };
                    this.DataContext = dashboard;
                }
            }
            catch (Exception)
            {

                throw;
            }



       //  MessageBox.Show(  await ConsultarApiAsync("https://api.exchangerate-api.com/v4/latest/USD"));
            //if (!IsAdministrator())
            //{
            //    MessageBox.Show("Esta aplicación requiere permisos de administrador", "Aviso");
               
            //}
            //if (File.Exists("C:\\Users\\Amaurys\\Desktop\\vs.txt")) {
            //    MessageBox.Show("El documento existe");
            //    string text = File.ReadAllText("C:\\Users\\Amaurys\\Desktop\\vs.txt");
            //    if (text != "2.5.3")
            //    {
            //        MessageBox.Show("existe una nueva actualizacion");
            //        File.WriteAllText("C:\\Users\\Amaurys\\Desktop\\vs.txt", "2.5.9");
            //    }
            //}
            //else
            //{

            //    File.Create("C:\\Users\\Amaurys\\Desktop\\vs.txt");
             
            //    File.WriteAllText("C:\\Users\\Amaurys\\Desktop\\vs.txt", "2.5.3");
            //}

            string FechaUTC;
            try
            {
             //   FechaUTC = DateTime.Now.ToString("dd/MM/yyyy");
              //  FechaInicio.Text = Util.TiempoEnRed().ToString("dd/MM/yyyy");

            //    FechaInicio.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("America/Santo_Domingo")).ToString("dd/MM/yyyy");
                //img.Source = Util.LoadImage(SesionUsuario.Configuracion.LogoPrincipal??new byte[1]);
               // txtSaludoUsuario.Text = SesionUsuario.Configuracion.NombreComercial;
               // txtNombreEmpresa.Text = SesionUsuario.Configuracion.NombreEmpresa;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener la fecha en zona horaria 'America/Santo_Domingo': " + ex.Message +" Se establecio la fecha del equipo local", "Error");
                FechaUTC = DateTime.Now.ToString("dd/MM/yyyy");
               // throw;
            }
           

               


           
              //  FechaInicio.Text = FechaUTC;
                //  MessageBox.Show(Session.Configuracion.MensajeError + " :" + ex.Message, "Error");
               
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PFacturar());
        }
    }
}


