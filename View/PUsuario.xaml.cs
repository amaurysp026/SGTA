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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SFCH.View
{
    /// <summary>
    /// Lógica de interacción para PUsuario.xaml
    /// </summary>
    public partial class PUsuario : Page
    {
        public ICollection<Usuario> usuarios { get; set; } = new List<Usuario>();
        private IUsuario iUsuario = new UsuarioService();

        public PUsuario()
        {
            InitializeComponent();
            // Registrar el manejador del evento RowEditEnding para guardar cambios al terminar la edición de una fila
            data.RowEditEnding += Data_RowEditEnding;
            Cargar();
        }

        private async void Cargar()
        {
            usuarios = await iUsuario.ObtenerUsuariosAsync();
            DataContext = this;
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (data.SelectedItem is Usuario usuarioSeleccionado)
            {
                await iUsuario.ActualizarUsuario(usuarioSeleccionado);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        // Manejador que se activa cuando se termina la edición de una fila del DataGrid `data`.
        // Guarda los cambios en la base de datos mediante el servicio iUsuario.
        private void Data_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            try
            {
                if (e.EditAction != DataGridEditAction.Commit)
                    return;

                if (e.Row.Item is not Usuario usuario)
                    return;

                // Forzar commit de la fila para que los valores editados se apliquen al objeto
              //  data.CommitEdit(DataGridEditingUnit.Row, true);

                // Ejecutar la actualización después de que la UI haya aplicado los cambios.
                // No se espera la operación aquí para no bloquear la UI, pero se maneja cualquier excepción.
                Dispatcher.InvokeAsync(async () =>
                {
                    try
                    {
                        await iUsuario.ActualizarUsuario(usuario);
                    }
                    catch (Exception ex)
                    {
                        // Notificar al usuario si falla la persistencia
                        MessageBox.Show($"Error al guardar usuario: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }, System.Windows.Threading.DispatcherPriority.Background);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en RowEditEnding: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
