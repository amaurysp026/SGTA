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
    /// Lógica de interacción para PRecepcion.xaml
    /// </summary>
    public partial class PRecepcion : Page, INotifyPropertyChanged
    {
        public x RecepcionLeche { get; set; }= new x();
        public f Freezer { get; set; } = new f();
        private IEntidad entidad= new EntidadesService();
       // private IRecepcion recepcion = new RecepcionService();
        private class FreezerSummary
        {
            public f Freezer { get; set; }
            public decimal TotalLitros { get; set; }
        };
        private bool ActualizandoR=false;
        private xd reg = new xd();
        public PRecepcion()
        {
            InitializeComponent();
            RecepcionLeche.Tanda = "Mañana";
            Cargar();
           // this.DataContext = RecepcionLeche;
           // Cargar();

        }
        private async void Cargar()
        {
           // var r = await recepcion.ObtenerRecepcionesLecheAsync();
            if (true)
            {
                MessageBox.Show("Ninguna Recepcion encontrada");
                return;
            }
            // MessageBox.Show("Recepción cargada con éxito "+r.Id);
            cbf.ItemsSource = null;
          //  cbf.ItemsSource = await recepcion.ObtenerFreezersAsync();
           // RecepcionLeche = r.LastOrDefault()??new x() ;
            txttlitros.Text = (RecepcionLeche.Detalles.Sum(x => x.Litros)).ToString("N2");
            txttRegistro.Text = RecepcionLeche.Detalles.Count.ToString();
            var resumen = RecepcionLeche.Detalles
              .GroupBy(r => r.Freezer)
              .Select(g => new FreezerSummary
              {
                  Freezer = g.Key,
                  TotalLitros = g.Sum(r => r.Litros)
              })
              .ToList();
            datalitros.ItemsSource = null;
            datalitros.ItemsSource = resumen;
            data.ItemsSource = null;
            data.ItemsSource = RecepcionLeche.Detalles.OrderByDescending(x => x.Id);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RecepcionLeche)));
            this.DataContext = RecepcionLeche;

        }
        public event PropertyChangedEventHandler? PropertyChanged;
       


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
           GuardarNRegistro();
        }
        private async void Actualizardatos()
        {
           // RecepcionLeche = await recepcion.OptenerRecepcionLechePorIdAsync(RecepcionLeche.Id);
            txtlitros.Text = string.Empty;
            txtcodigo.Text = string.Empty;
            txtnombre.Text = string.Empty;
            txttlitros.Text = (RecepcionLeche.Detalles.Sum(x => x.Litros)).ToString("N2");
            txttRegistro.Text = RecepcionLeche.Detalles.Count.ToString("N0");
            data.ItemsSource = null;
            data.ItemsSource = RecepcionLeche.Detalles.OrderByDescending(x=>x.Id);
            datalitros.ItemsSource = null;
            datalitros.ItemsSource = RecepcionLeche.Detalles.OrderByDescending(x => x.Id);
            reg = new xd();

            var resumen = RecepcionLeche.Detalles
                .GroupBy(r => r.Freezer)
                .Select(g => new FreezerSummary
                {
                    Freezer = g.Key,
                    TotalLitros = g.Sum(r => r.Litros)
                })
                .ToList();
            datalitros.ItemsSource = null;
            datalitros.ItemsSource = resumen;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RecepcionLeche)));
            txtcodigo.Focus();

        }

        private async void btnNewRecepcion_Click(object sender, RoutedEventArgs e)
        {
            var n=new x()
            {
                Fecha=DateTime.Now,
            };
           if( true) //await recepcion.GuardarRecepcionLecheAsync(n,DateTime.Now))
             {
                 MessageBox.Show("Recepción guardada con éxito");
              //   var re= await recepcion.ObtenerRecepcionesLecheAsync();
              //   RecepcionLeche = re.LastOrDefault() ?? new x();
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RecepcionLeche)));
             }
             else
             {
                 MessageBox.Show("Error al guardar la recepción");
             }
            Cargar();
        }
        private async void GuardarNRegistro()
        {
            if (ActualizandoR)
            {
             //   var ent = await entidad.ObtenerEntidadesAsync();
                if (decimal.TryParse(txtlitros.Text, out decimal litros) == false)
                {
                    MessageBox.Show("Cantidad de litros no válida", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtlitros.Focus();
                    return;
                }
               if (RecepcionLeche.Detalles == null || RecepcionLeche.Detalles.Count == 0)
                {
                    MessageBox.Show("No se encontraron registros para actualizar", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (litros <= 0)
                {
                    MessageBox.Show("La cantidad de litros debe ser mayor a cero", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtlitros.Focus();
                    return;
                }
               
                var detalle = (xd)data.SelectedItem;
                detalle.Litros = litros;
                detalle.Freezer = cbf.SelectedItem as f ?? new f();
               // var resf = await recepcion.EditarRegistroAsync(detalle);
                if (true)
                {
                    MessageBox.Show("Registro actualizado con éxito", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Error);
                    Actualizardatos();
                    txtcodigo.IsReadOnly = false;
                    ActualizandoR = false;
                    btnguardar.Content = "Guardar";
                }
                else
                {
                    MessageBox.Show("Error al actualizar el registro", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else
            {
              //  var ent = await entidad.ObtenerEntidadesAsync();
                if (decimal.TryParse(txtlitros.Text, out decimal litros) == false)
                {
                    MessageBox.Show("Cantidad de litros no válida", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (reg.Proveedor==null)
                {
                   var resul= MessageBox.Show("No se a encontrado resultados con el código. Desea realizar una búsqueda avanzada?","Aviso!",MessageBoxButton.YesNo,MessageBoxImage.Warning);
                    if (resul==MessageBoxResult.Yes)
                    {
                        //abrir ventana de busqueda de socios
                    }
                    return;
                }
                if (litros <= 0)
                {
                    MessageBox.Show("La cantidad de litros debe ser mayor a cero", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtlitros.Focus();
                    return;
                }
                if(cbf.SelectedItem as f == null)
                {
                    MessageBox.Show("Seleccione un freezer", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Information);
                    cbf.Focus();
                    return;

                }
                if(RecepcionLeche==null)
                {
                    MessageBox.Show("No se encontró una recepción activa", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }
                if (RecepcionLeche.Detalles.FirstOrDefault(x=>x.Proveedor.Codigo==reg.Proveedor.Codigo)!=null)
                {
                    MessageBox.Show("El registro ya existe en la recepción actual", "Aviso!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Actualizardatos();
                    return;
                }
                reg.Litros = litros;
                reg.RecepcionLeche = RecepcionLeche;
                reg.Tanda = RecepcionLeche.Tanda;
                reg.Freezer = cbf.SelectedItem as f;
               
             //   var resf = await recepcion.GuardarRegistro(reg);
                //  MessageBox.Show(resf.Id.ToString());
             //   RecepcionLeche.Detalles.LastOrDefault()?.Id = resf.Id;
                
                //txttlitros.Text = (RecepcionLeche.Detalles.Sum(x => x.Litros)).ToString("N2");
                data.ItemsSource = null;
                data.ItemsSource = RecepcionLeche.Detalles.OrderByDescending(x => x.Id);
                // PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RecepcionLeche)));
                Actualizardatos();
            }
        }
        private void txtlitros_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var resul = MessageBox.Show("Desea continuar con el registro?", "Aviso!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (resul == MessageBoxResult.Yes)
                {
                    GuardarNRegistro();
                    //abrir ventana de busqueda de socios
                }
                else
                {
                    btnguardar.Focus();

                }

            }

        }

        private async void txtcodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
               txtlitros.Focus();
            //    // Buscar el proveedor por código
            //    var codigo = txtcodigo.Text.Trim();
            //    var proveedor = await entidad.ObtenerEntidadesAsync();
            //var en=    proveedor.FirstOrDefault(p => p.Codigo != null && p.Codigo == codigo);
            //    if (en != null)
            //    {
            //        txtnombre.Text = en.NombreCompleto;
            //        txtlitros.Focus();
            //    }
            //    else
            //    {
            //        MessageBox.Show("Proveedor no encontrado.");
            //        txtnombre.Text = string.Empty;
            //    }
            }

        }

        private async void txtlitros_LostFocus(object sender, RoutedEventArgs e)
        {
            
           
        }

        private async void txtcodigo_LostFocus(object sender, RoutedEventArgs e)
        {
            var codigo = txtcodigo.Text.Trim();
            var proveedor = await entidad.ObtenerEntidadesAsync();
            var en = proveedor.FirstOrDefault(p => p.Codigo != null && p.Codigo == codigo);
            if (en != null)
            {
                reg.Proveedor = en;
                txtnombre.Text = reg.Proveedor.NombreCompleto;
                
            }
            else
            {
              // MessageBox.Show("Codigo no encontrado.");
                txtnombre.Text = string.Empty;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            VBuscarRecepcion recepcion = new VBuscarRecepcion();    
            if (recepcion.ShowDialog() == true)
            {
                
                RecepcionLeche = recepcion.RecepcionLeche;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RecepcionLeche)));
                txttlitros.Text = (RecepcionLeche.Detalles.Sum(x => x.Litros)).ToString("N2");
                txttRegistro.Text = RecepcionLeche.Detalles.Count.ToString();
                data.ItemsSource = null;
                data.ItemsSource = RecepcionLeche.Detalles.OrderByDescending(x => x.Id);
                this.DataContext = RecepcionLeche;
            }
        }

        private void mbtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (data.SelectedItem != null)
            {
                var detalle = (xd)data.SelectedItem;
                
                txtcodigo.Text = detalle.Proveedor.Codigo ?? string.Empty;
                txtnombre.Text = detalle.Proveedor.NombreCompleto;
                txtlitros.Text = detalle.Litros.ToString("N2");
                cbf.SelectedItem = detalle.Freezer;
                ActualizandoR = true;
                txtcodigo.IsReadOnly = true;
                btnguardar.Content = "Actualizar";
            }

        }

        private async void mbtnAnular_Click(object sender, RoutedEventArgs e)
        {
            if (data.SelectedItem != null)
            {
                var detalle = (xd)data.SelectedItem;
                var result = MessageBox.Show("¿Está seguro de que desea anular este registro?", "Confirmar Anulación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                 if(  true)
                        MessageBox.Show("Registro anulado con éxito");
                    else
                        MessageBox.Show("Error al anular el registro");
                    Actualizardatos();
                }
            }

        }

        /*get
            {
                var hora = Fecha.TimeOfDay;

                // Ejemplo: Tandas comunes en lechería
                if (hora >= new TimeSpan(4, 0, 0) && hora < new TimeSpan(12, 0, 0))
                    return "Mañana";
                else if (hora >= new TimeSpan(12, 0, 0) && hora < new TimeSpan(20, 0, 0))
                    return "Tarde";
                else
                    return "Noche";
            }
            set
            {
                var hora = Fecha.TimeOfDay;

                // Ejemplo: Tandas comunes en lechería
                if (hora >= new TimeSpan(4, 0, 0) && hora < new TimeSpan(12, 0, 0))
                    value = "Mañana";
                else if (hora >= new TimeSpan(12, 0, 0) && hora < new TimeSpan(20, 0, 0))
                    value = "Tarde";
                else
                    value = "Noche";
            }*/
    }
}
