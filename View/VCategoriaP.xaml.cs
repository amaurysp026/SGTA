using SFCH.IService;
using SFCH.Logica;
using SFCH.Model;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para VCategoriaP.xaml
    /// </summary>
    public partial class VCategoriaP : Window
    {
        bool editando=false;
        IHerramientas herramientas= new HerramientasService();
        Categoria categoriaEditar;
        public VCategoriaP()
        {
            InitializeComponent();
            Cargar();
        }
        private async void Cargar()
        {
            var categorias = await herramientas.ObtenerCategorias();
           data.ItemsSource = categorias;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!editando)
            {
                var cat = new Categoria()
                {
                    Nombre = txtcat.Text,
                    Activo = true,
                    Productos = new List<Producto>()


                };
                var res = await herramientas.GuardarCategorias(cat);
                if (res)
                {
                    data.ItemsSource = null;
                    data.ItemsSource = await herramientas.ObtenerCategorias();
                    txtcat.Text=string.Empty;
                }
                else
                {
                    MessageBox.Show("No se pudo guardar la categoría.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
               categoriaEditar.Nombre = txtcat.Text;
                var res = await herramientas.GuardarActualizar(categoriaEditar);
                if (res)
                {
                    data.ItemsSource = null;
                    data.ItemsSource = await herramientas.ObtenerCategorias();
                    txtcat.Text = string.Empty;
                    editando = false;
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar la categoría.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (data.SelectedItem != null)
            {
                categoriaEditar = (Categoria)data.SelectedItem;
                txtcat.Text = categoriaEditar.Nombre;
                editando = true;
            }

        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (data.SelectedItem is Categoria cat)
            {
                if (cat.CantProducto > 0)
                {
                    if(MessageBox.Show("La categoría contiene productos, si es desactivada los productos no se podrán visualizar en facturación a través de la selección rápida. Desa Continuar", "Aviso!", MessageBoxButton.YesNo, MessageBoxImage.Question)==MessageBoxResult.No){
                        return;
                    }
                    ;
                }
               await herramientas.DesactivarCategoria(cat);
            }
        }
    }
}
