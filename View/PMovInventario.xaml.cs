using SFCH.IService;
using SFCH.Logica;
using SFCH.Model;
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
    /// Lógica de interacción para PMovInventario.xaml
    /// </summary>
    public partial class PMovInventario : Page
    {
        IProducto produc = new  ProductoService();
        public PMovInventario()
        {
            InitializeComponent();
            Cargar();
        }
        public async void Cargar()
        {
         // using (var db= new Conexion())
         // {
         //  var d=  await db.Productos.FindAsync(1);
         //     d.Imagen = null;
         //     await db.SaveChangesAsync();
         // }

            data.ItemsSource = null;
      data.ItemsSource=    await  produc.OptenerExistencias();
            data2.ItemsSource = null;
            data2.ItemsSource = await produc.OptenerLotes();
        }
    }
}
