using SFCH.IService;
using SFCH.Logica;
using SFCH.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Lógica de interacción para PPaseInventario.xaml
    /// </summary>
    public partial class PPaseInventario : Page, INotifyPropertyChanged
    {
        public ContInventario Inventario {
            get { return field; }
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(Inventario));
                }
            }
        } = new ContInventario();

        IProducto produc = new ProductoService();
        ICategorias categorias = new CategoriaService();

        public event PropertyChangedEventHandler? PropertyChanged;

        public PPaseInventario()
        {
            InitializeComponent();
            Cargar();
            this.DataContext = Inventario;
        }
      
        public async void Cargar()
        {
            cbcategorias.ItemsSource = await categorias.ObtenerCategorias();
            Inventario =new ContInventario
            {
                Fecha = DateTime.Now,
                FechaConteo = DateTime.Now,
               
                Observacion = "",
                Estado = "Abierto",
                Aplicado = false,
               
                
            };

        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void btnNuevoConteo_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnCargarInventario_Click(object sender, RoutedEventArgs e)
        {
            var productos = await produc.ObtenerProductos();

            foreach (var item in productos)
            {
                if (item != null && !item.EsServicio)
                {
                    Inventario.Detalle.Add(new DetalleContInventario
                    {
                        Producto = item,
                        CantSistema = item.CantidadDisponible,
                        CantFisico = 0,
                        Diferencia = 0,
                         
                    });
                }
            }

            Inventario.Estado = "Inventario cargado exitosamente. " + Inventario.Detalle.Count.ToString();

            // Forzar refresco del binding para que el DataGrid muestre la lista actualizada.
            // 1) Reiniciamos el DataContext de la página (simple y seguro si la colección no es ObservableCollection)
            var prevContext = this.DataContext;
            this.DataContext = null;
            this.DataContext = Inventario;

            // 2) Además, intentamos refrescar la vista por si la colección ya es IEnumerable enlazable
            CollectionViewSource.GetDefaultView(Inventario.Detalle)?.Refresh();

            MessageBox.Show("Inventario cargado exitosamente. "+ Inventario.Detalle.Count.ToString(), "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async void btnGuardarConteo_Click(object sender, RoutedEventArgs e)
        {
            if ( await produc.GuardarInventario(Inventario))
            {
                MessageBox.Show("Inventario guardado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                Cargar();            
            }
            else
            {
                MessageBox.Show("Error al guardar el inventario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //remover de la lista el item seleccionado
            if (dgConteoInventario.SelectedItem is DetalleContInventario selectedDetalle)
            {
                Inventario.Detalle.Remove(selectedDetalle);
                // Forzar refresco del binding para que el DataGrid muestre la lista actualizada.
                var prevContext = this.DataContext;
                this.DataContext = null;
                this.DataContext = Inventario;
                CollectionViewSource.GetDefaultView(Inventario.Detalle)?.Refresh();
            }

        }
    }
}
