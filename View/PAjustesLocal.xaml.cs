using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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

namespace SGTA.View
{
    /// <summary>
    /// Lógica de interacción para PAjustesLocal.xaml
    /// </summary>
    public partial class PAjustesLocal : Page
    {
        public PAjustesLocal()
        {
            InitializeComponent();
            cargar();
        }
        public async void cargar()
        {
            using (var db = new Conexion())
            {
                var configlocal = await db.ConfiguracionLocals.ToListAsync();
                data.ItemsSource = configlocal;
                foreach (var item in await db.Personas.ToListAsync())
                {   
                    if (item.TipoEntidad==null)
                    {
                       MessageBox.Show(  item.Nombre);
                        
                    }
                }
            }
        }
    }
}
