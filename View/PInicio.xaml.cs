using Microsoft.EntityFrameworkCore;
using SFCH.Controller;
using SFCH.Model;
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
        public PInicio()
        {
            InitializeComponent();

            _= Cargar();
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
        private async Task Cargar()
        {
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
                FechaUTC = DateTime.Now.ToString("dd/MM/yyyy");
              //  FechaInicio.Text = Util.TiempoEnRed().ToString("dd/MM/yyyy");

                FechaInicio.Text = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("America/Santo_Domingo")).ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener la fecha en zona horaria 'America/Santo_Domingo': " + ex.Message +" Se establecio la fecha del equipo local", "Error");
                FechaUTC = DateTime.Now.ToString("dd/MM/yyyy");
               // throw;
            }
           

                img.Source = Util.LoadImage(SesionUsuario.Configuracion.LogoPrincipal);
                txtSaludoUsuario.Text = SesionUsuario.Configuracion.NombreComercial;
                txtNombreEmpresa.Text = SesionUsuario.Configuracion.NombreEmpresa;
         


           
                FechaInicio.Text = FechaUTC;
                //  MessageBox.Show(Session.Configuracion.MensajeError + " :" + ex.Message, "Error");
               
            
        }
    }
}


