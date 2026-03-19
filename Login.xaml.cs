using Microsoft.EntityFrameworkCore;
using SFCH.Controller;
using SFCH.IService;
using SFCH.Logica;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SFCH
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private IUsuario usarios = new UsuarioService();
        private string Tiemp { get; set; }
        public Login()
        {
            var sw = Stopwatch.StartNew(); // inicia el cronómetro
            InitializeComponent();
            _ = Cargar();
            sw.Stop(); // detiene el cronómetro
                       // MessageBox.Show($"La ventana tardó {sw.ElapsedMilliseconds} ms en iniciar");
                       // txtfoot.Text = $"La ventana tardó {sw.ElapsedMilliseconds} ms en iniciar";
            Tiemp = ((decimal)sw.ElapsedMilliseconds / 1000).ToString();
            // InitializeComponent();
            //  _= Cargar();
            txtfoot.Text += " | "+ Tiemp;

            //  MessageBox.Show("Ya se inicio");

        }

        private async Task Cargar()
        {

            Configuracion congen;
            ConfiguracionLocal con;
            using (var db = new Conexion())
            {

                //  MessageBox.Show(Util.RutaEjecutable());
                //  MessageBox.Show("Bienvenido a "+ db.Configuracions.FirstOrDefault()?.NombreComercial,"Bienvenido",MessageBoxButton.OK,MessageBoxImage.Information);
                // txtfoot.Text=SesionUsuario.InfConexion;
                congen = await db.Configuracions.AsNoTracking().FirstAsync();
                con = await db.ConfiguracionLocals.AsNoTracking().FirstAsync(x => x.NombreEquipo == Environment.MachineName) ?? new ConfiguracionLocal();
                if (con == null)
                {
                    MessageBox.Show("No se encontró configuración local para este equipo. Se creará una nueva configuración local con valores predeterminados.", "Configuración Local No Encontrada", MessageBoxButton.OK, MessageBoxImage.Warning);
                    await db.ConfiguracionLocals.AddAsync(new ConfiguracionLocal { NombreEquipo = Environment.MachineName, RecordarUsuario = false, UsuarioRec = "" });
                    await db.SaveChangesAsync();
                    con = await db.ConfiguracionLocals.FirstOrDefaultAsync(x => x.NombreEquipo == Environment.MachineName) ?? new ConfiguracionLocal();

                }
                if (congen == null)
                {
                        MessageBox.Show("No se encontró configuración general. Se creará una nueva configuración con valores predeterminados.", "Configuración No Encontrada", MessageBoxButton.OK, MessageBoxImage.Warning);
                    await db.Configuracions.AddAsync(new Configuracion { NombreComercial = Environment.MachineName, NombreEmpresa = "", Telefono = "" });
                    await db.SaveChangesAsync();
                    this.Close();
                }
                if (con.RecordarUsuario)
                {
                    txtnombre.Text = con.UsuarioRec;
                    ChRecUsuario.IsChecked = con.RecordarUsuario;
                    txtclave.Focus();
                }

            }



            //  img.Source = null;
            img.Source = Util.LoadImage(congen.LogoPrincipal);
            var imagen = Util.LoadImage(congen.LogoPrincipal);
            carlogo.Visibility = Visibility.Visible;
            progreso.Visibility = Visibility.Collapsed;

            // Util.GuardarImagenEnCarpetaSeleccionada(imagen,out string ruta);
            // Util.GuardarImagenEnCarpetaSeleccionada(out string ruta);






        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            //  Button_Click(this, e);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
             this.Close();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtclave.Password) || string.IsNullOrWhiteSpace(txtnombre.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios", "Advertencia!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = await usarios.ObtenerUsuarioPorNombreAsync(txtnombre.Text);
            if (user == null)
            {

                MessageBox.Show("Usuario no encontrado", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            bool resu = await usarios.ValidarUsuarioAsync(user, txtclave.Password);
            if (!resu)
            {
                MessageBox.Show("Clave incorrecta", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SesionUsuario.Usuario = user;
            await usarios.EntradaConfiguracionLocal(SesionUsuario.Usuario.NombreUsuario, ChRecUsuario.IsChecked ?? false);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void txtnombre_KeyDown(object sender, KeyEventArgs e)
        {
            //  Button_Click(this, e);
            if (e.Key == Key.Enter)
            {
                txtclave.Focus();

            }
        }

        private void txtclave_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                Button_Click_1(sender, e);

            }

        }
    }
}
