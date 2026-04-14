using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using SFCH.Controller;
using SFCH.IService;
using SFCH.Logica;
using SFCH.Model;
using SFCH.PrintView;
using SFCH.View;
using SFCH.View;
using SGTA.View;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SFCH
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IHerramientas herramientas = new HerramientasService();
        public bool Ganadero = false;
        public MainWindow()
        {
            InitializeComponent();
            // Registrar el evento Loaded para navegar cuando la ventana esté lista
             InitializeAsync();
            if (Ganadero)
            {
                Tabadministracion.Visibility = Visibility.Collapsed;
                TabAcopio.Visibility = Visibility.Visible;
                
            }
            TabAcopio.Visibility = SesionUsuario.Usuario.Acopio ? Visibility.Visible : Visibility.Collapsed;
            Tabadministracion.Visibility = SesionUsuario.Usuario.Administrador ? Visibility.Visible : Visibility.Collapsed;
            GrouConfiguracion.Visibility = SesionUsuario.Usuario.Configuracion ? Visibility.Visible : Visibility.Collapsed;
            GrupPuntoVenta.Visibility = SesionUsuario.Usuario.PuntoVentas ? Visibility.Visible : Visibility.Collapsed;
            GrupTractor.Visibility = SesionUsuario.Usuario.Tractor ? Visibility.Visible : Visibility.Collapsed;
            GrupInventario.Visibility = SesionUsuario.Usuario.Inventario ? Visibility.Visible : Visibility.Collapsed;
            btnCompras.Visibility = SesionUsuario.Usuario.Compras ? Visibility.Visible : Visibility.Collapsed;
            GrupContabilidad.Visibility = SesionUsuario.Usuario.Contabilidad ? Visibility.Visible : Visibility.Collapsed;
            TabEntidades.Visibility = SesionUsuario.Usuario.Entidades ? Visibility.Visible : Visibility.Collapsed;
            GroupHerramientas.Visibility = SesionUsuario.Usuario.Herramientas ? Visibility.Visible : Visibility.Collapsed;
            btnmovinventario.Visibility = SesionUsuario.Usuario.Administrador ? Visibility.Visible : Visibility.Collapsed;
            btnCerrTurnoActual.Visibility = SesionUsuario.Usuario.CerrarTurno ? Visibility.Visible : Visibility.Collapsed;
            btnturnos.Visibility = SesionUsuario.Usuario.CerrarTurno ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void InitializeAsync()
        {
            try
            {

                if (SesionUsuario.Usuario == null)
                {
                    Login login = new Login();
                    var result = login.ShowDialog();
                    if (result != true)
                    {
                        Application.Current.Shutdown();
                        return;
                    }
                }
                txtUsuariobar.Text = $"" + SesionUsuario.InfConexion + " | Usuario: " + SesionUsuario.Usuario.NombreUsuario+"";
                await Cargar();
                await Dispatcher.InvokeAsync(() =>
                {
                    FrameInicio.Content = new PInicio();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: "+ex.Message);
              
            }
         
        }

        private async Task Cargar()
        {

            using (var db = new Conexion())
            {
                var cl = await db.ConfiguracionLocals.AsNoTracking().FirstOrDefaultAsync(x => x.NombreEquipo == Environment.MachineName);
                if (cl == null)
                {
                    await db.ConfiguracionLocals.AddAsync(new ConfiguracionLocal()
                    {
                        PrintReporte = Util.CargarImpresoras().FirstOrDefault(x => x.ToUpper().Contains("PDF")) ?? "",
                        Color = "#FF3F51B5",
                        NombreEquipo = Environment.MachineName,
                    });
                    if (await db.SaveChangesAsync() > 0)
                    {
                        MessageBox.Show("Se ha creado una configuración local para este equipo, por favor verifique las impresoras en la sección de configuración.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {

                    SesionUsuario.ConfiguracionLocal = cl;
                    SesionUsuario.Configuracion = await db.Configuracions.FirstOrDefaultAsync();

                }

            }
        }

        private void btnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Content = new PUsuario();
        }

        private void btnCuentas_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Content = new PCuentas();
        }

        private void btnaperturarC_Click(object sender, RoutedEventArgs e)
        {
            VNewCuenta cuenta = new VNewCuenta();
            cuenta.ShowDialog();
        }

        private async void btnSocios_Click_1(object sender, RoutedEventArgs e)
        {
            var tip = await herramientas.ObtenerEntidades();
            var entidad = tip.Where(x => x.Nombre == "SOCIO")?.FirstOrDefault() ?? null;
            if (entidad == null)
            {
                MessageBox.Show("No existe la entidad SOCIO, por favor cree la entidad SOCIO primero.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            FrameInicio.Navigate(new PPersona(entidad));
        }

        private void btnTrIngreso_Click(object sender, RoutedEventArgs e)
        {
            VTransaccion transaccion = new VTransaccion();
            transaccion.ShowDialog();

        }

        private void btnTrEgreso_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Navigate(new PTurnos());
        }

        private void btnCarImagenInicio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new Conexion())
                {
                    var cnf = db.Configuracions.FirstOrDefault();
                    if (cnf != null)
                    {
                        var openFileDialog = new Microsoft.Win32.OpenFileDialog
                        {
                            Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
                        };
                        if (openFileDialog.ShowDialog() == true)
                        {
                            string selectedFilePath = openFileDialog.FileName;
                            byte[] imageBytes = System.IO.File.ReadAllBytes(selectedFilePath);
                            cnf.LogoPrincipal = imageBytes;
                            db.SaveChanges();
                            img.Source = Util.LoadImage(cnf.LogoPrincipal);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la configuración en la base de datos.", "Error");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void btnnewSocio_Click(object sender, RoutedEventArgs e)
        {
            var tip = await herramientas.ObtenerEntidades();
            var entidad = tip.Where(x => x.Nombre=="SOCIO")?.FirstOrDefault()??null;
            if (entidad == null)
            {
                MessageBox.Show("No existe la entidad SOCIO, por favor cree la entidad SOCIO primero.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            VNewPersona vNewPersona= new VNewPersona(entidad);
            vNewPersona.ShowDialog();
        }

        private void btnnewEntidad_Click(object sender, RoutedEventArgs e)
        {
            VNewEntidad vNewEntidad= new VNewEntidad();
            vNewEntidad.ShowDialog();
        }

        private void btnCarImagenprint_Click(object sender, RoutedEventArgs e)
        {
            return;
            try
            {
                using (var db = new Conexion())
                {
                    var cnf = db.Configuracions.FirstOrDefault();
                    if (cnf != null)
                    {
                        var openFileDialog = new Microsoft.Win32.OpenFileDialog
                        {
                            Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
                        };
                        if (openFileDialog.ShowDialog() == true)
                        {
                            string selectedFilePath = openFileDialog.FileName;
                            byte[] imageBytes = System.IO.File.ReadAllBytes(selectedFilePath);
                            cnf.Timbrado= imageBytes;
                            db.SaveChanges();
                            img2.Source = Util.LoadImage(cnf.Timbrado);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se encontró la configuración en la base de datos.", "Error");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnusuario_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnTransacciones_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Content = new PTransacciones();
        }

        private void btninicio_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Content = new PInicio();
        }

        private void btnReportTransacciones_Click(object sender, RoutedEventArgs e)
        {
            VReportes vReportes= new VReportes();
            vReportes.Show();
        }

        private void btnAddUsuario_Click(object sender, RoutedEventArgs e)
        {
           VNewUsuario vNewUsuario =new VNewUsuario();
            vNewUsuario.Show();
        }

        private void btnFacturar_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Navigate(new PFacturar());
        }

        private void NewProducto_Click(object sender, RoutedEventArgs e)
        {
            VNewProducto vNewProducto = new VNewProducto();
            vNewProducto.ShowDialog();
        }

        private void btnProductos_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Navigate(new PProductos());
        }

        private void btntodaslasfacturas_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Navigate(new PFacturas());

        }

        private void btnCategoriasp_Click(object sender, RoutedEventArgs e)
        {
            VCategoriaP vCategoriaP = new VCategoriaP();
            vCategoriaP.ShowDialog();
        }

        private void btnCompras_Click(object sender, RoutedEventArgs e)
        {   if (SesionUsuario.Usuario.Compras)
                FrameInicio.Navigate(new PCompra());
            else
                MessageBox.Show("No tiene permisos para acceder a esta sección.", "Permisos", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void btnProveedor_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Navigate(new PProveedores());
        }

        private void btnnewProveedor_Click(object sender, RoutedEventArgs e)
        {
            VNewProveedor vNewProveedor= new VNewProveedor();
            vNewProveedor.ShowDialog();
        }

        private void btnReportIngresos_Click(object sender, RoutedEventArgs e)
        {
            VPagina vPagina = new VPagina(new PRIngresosXmes());
            vPagina.Show();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Navigate(new PRecepcion());
        }

        private void btnRecepciones_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Navigate(new Precepciones());
        }

        private void btnNewRecepcion_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnCerrarsecion_Click(object sender, RoutedEventArgs e)
        {
          //  File.Copy(@"C:\Users\Amaurys\Desktop\Proyectos Personales\Importante\SFCH\SFCH\bin\Debug\net10.0-windows7.0\SFCH.exe", @"C:\Users\Amaurys\Desktop\RED\SFCH.exe", true);
         //   MessageBox.Show("Sistema actualizado. Reinicie la aplicación.");
            Application.Current.Shutdown();
            //Util.BackupDatabase(new Conexion(),@"C:\\BackupsSQL\",1);
          //  MessageBox.Show("resdfgfdcvbcbvcbvcbvcbvcb");
        }

        private void btnBackUps_Click(object sender, RoutedEventArgs e)
        {
         var res=   MessageBox.Show("Desea realizar un respaldo de la base de datos?", "Respaldo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res==MessageBoxResult.Yes)
            {
               
                Util.BackupDatabase(new Conexion(), @"C:\\BackupsSQL\", 1);
                MessageBox.Show("Se a respaldado la base de datos.", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Information);
                var res2 = MessageBox.Show("Desea realizar un respaldo del ejecutable actual?", "Respaldo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res2 == MessageBoxResult.Yes)
                {
                    File.Copy(@"C:\Users\Amaurys\Desktop\Proyectos Personales\Importante\SFCH\SFCH\bin\Debug\net10.0-windows7.0\SFCH.exe", @"C:\\BackupsSQL\SFCH.exe", true);
                    MessageBox.Show("Sistema actualizado. Reinicie la aplicación.");
                    Application.Current.Shutdown();
                }
                }
        }

        private void btnFrezzer_Click(object sender, RoutedEventArgs e)
        {
            PFrezzer pFrezzer = new PFrezzer();
            pFrezzer.ShowDialog();

        }

        private void btnmovinventario_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Navigate(new PMovInventario());
        }


        private void btnEgresoCaja_Click(object sender, RoutedEventArgs e)
        {
            VEgresoCaja vEgresoCaja = new VEgresoCaja();
            vEgresoCaja.ShowDialog();
        }

        private void btnCerrTurnoActual_Click(object sender, RoutedEventArgs e)
        {
            if (SesionUsuario.TurnoActual.Abierto)
            {
                VCierreCuadre vCierreCuadre = new VCierreCuadre(SesionUsuario.TurnoActual);
                vCierreCuadre.ShowDialog();
            }
            else
            {
                MessageBox.Show("No tienes turno abierto");
            }

        }

        private void btninventariar_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Navigate(new PPaseInventario());
        }

        private void btninventarios_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Navigate(new PInventarios());
        }

        private void btnCompraslist_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Navigate(new PComprasList());
        }

        private void btncxccliente_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Navigate(new PCxC());
        }

        private void btnPagocxc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRecep_Click(object sender, RoutedEventArgs e)
        {
            FrameInicio.Navigate(new PRecepVehiculo());
        }
    }
}