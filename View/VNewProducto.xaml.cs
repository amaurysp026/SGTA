using SFCH.Controller;
using SFCH.IService;
using SFCH.Logica;
using SFCH.Model;
using System;
using System.Collections.Generic;
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

namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para VNewProducto.xaml
    /// </summary>
    public partial class VNewProducto : Window
    {
        public record struct detalleProducto
        {
            public string Descripcion { get; set; }
            public decimal Cantidad { get; set; }
            public decimal CostoUnitario { get; set; }
            public decimal Total { get; set; }
        }
        List<detalleProducto> detalles = new List<detalleProducto>();
        public Producto producto { get; set; } =new Producto();
        ICategorias categorias= new CategoriaService();
        IProducto productos= new ProductoService();
        bool editando { get; set; }  = false;
        public VNewProducto()
        {
            InitializeComponent();
            Cargar();
            detalles.Add(new detalleProducto { Descripcion = "Existencia Inicial", Cantidad = 0, CostoUnitario = 0, Total = 0 });
            detalles.Add(new detalleProducto { Descripcion = "Entrada", Cantidad = 0, CostoUnitario = 0, Total = 0 });
            detalles.Add(new detalleProducto { Descripcion = "Salida", Cantidad = 0, CostoUnitario = 0, Total = 0 });
            data.ItemsSource=detalles;
        }
        public VNewProducto(Producto productoEd,bool Ver=false)
        {
            InitializeComponent();
            producto = productoEd;
            editando = true;
            if (Ver)
            {
                Title = "Detalle del Producto";
                btnGuardar.Visibility = Visibility.Collapsed;
                btnCancelar.Visibility = Visibility.Collapsed;
                griddat.IsEnabled = false;
            }
            Cargar();

        }
        public async void Cargar()
        {
            cbCategoria.ItemsSource = await categorias.ObtenerCategorias();
           cbUnidad.ItemsSource = await categorias.ObtenerUnidad();
            dImpuesto.ItemsSource = await productos.ObtenerImpuesto();
            try
            {
                imgp.Source=Util.LoadImage(producto.Imagen??new byte[0]);
                // if (producto.RutaImagen != null)
                // {
                //     var uri = new Uri(producto.RutaImagen, UriKind.RelativeOrAbsolute);
                //
                //     var bitmap = new BitmapImage();
                //     bitmap.BeginInit();
                //     bitmap.CacheOption = BitmapCacheOption.OnLoad; // asegura que el archivo no quede bloqueado
                //     bitmap.UriSource = uri;
                //     bitmap.EndInit();
                //     bitmap.Freeze(); // opcional: hace la imagen segura para hilos
                //     imgp.Source = bitmap;
                // }

            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar la imagen: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                imgp.Source = null;
            }
            this.DataContext = producto;
            cbCategoria.SelectedIndex= 0;
            cbUnidad.SelectedIndex= 0;
        }
     
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
         //MessageBox.Show(   producto.Unidad.Nombre);
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!editando)
            {
                var im = (Impuesto)dImpuesto.SelectedItem;
                producto.ITBIS = im.Porcentaje;
                var res = await productos.GuardarProducto(producto);
                if (res)
                {
                    MessageBox.Show("Producto guardado con éxito");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error al guardar el producto");
                }
            }
            else
            {
                var im = (Impuesto)dImpuesto.SelectedItem;
                producto.ITBIS = im.Porcentaje;
                var res = await productos.ActualizarProducto(producto);
                if (res)
                {
                    MessageBox.Show("Producto actualizado con éxito");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error al actualizar el producto");
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Guardar imagen y obtener la ruta
            if(urlimagen.Text!=string.Empty)
            {
                producto.RutaImagen= Util.DescargarImagenDeInternet(urlimagen.Text);
               // producto.RutaImagen = urlimagen.Text;
            }
            else
                 producto.RutaImagen = Util.GuardarImagenEnCarpetaSeleccionada(out string ruta);

            // Si no se seleccionó ninguna imagen, limpiar el control y salir
            if (string.IsNullOrWhiteSpace(producto.RutaImagen))
            {
                imgp.Source = null;
                return;
            }

            try
            {
                var uri = new Uri(producto.RutaImagen, UriKind.RelativeOrAbsolute);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad; // asegura que el archivo no quede bloqueado
                bitmap.UriSource = uri;
                bitmap.EndInit();
                bitmap.Freeze(); // opcional: hace la imagen segura para hilos
                producto.Imagen = Util.ConvertImageToByteArray(bitmap);
                imgp.Source = Util.LoadImage(producto.Imagen);

            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar la imagen: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                imgp.Source = null;
            }
        }
    }
}
