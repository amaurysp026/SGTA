using Microsoft.VisualBasic;
using SFCH.Controller;
using SFCH.IService;
using SFCH.Logica;
using SFCH.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para PFacturar.xaml
    /// </summary>
    public partial class PFacturar : Page, INotifyPropertyChanged
    {
        ITurno turno = new TurnoService();
        IFactura facturas = new FacturaService();
        ICategorias categoriasP = new CategoriaService();
        IPersona persona = new PersonaService();
        IProducto prd = new ProductoService();
        int FAbierta = 0;
        bool editando = false;
        private List<Lote> LotesDs { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        Factura facturat
        {
            get { return field; }
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(facturat));
                }
            }
        }
        public PFacturar()
        {
            InitializeComponent();

            // Agregar categorías iniciales de ejemplo
            _ = cargar();
            this.DataContext = facturat;
        }

        // Método que fuerza la actualización del DataGrid que muestra DetalleFactura.
        // Restablece ItemsSource y llama a Items.Refresh() en el hilo UI.
        private void RefrescarDetalleGrid()
        {
            // Asegurar ejecución en hilo UI y evitar excepciones si 'data' es null.
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    if (data != null)
                    {
                        // Forzar rebind completo para asegurar actualización de filas/valores.
                        data.ItemsSource = null;
                        if (facturat != null)
                        {
                            data.ItemsSource = facturat.Detalles;
                        }
                        data.Items.Refresh();
                    }
                }
                catch
                {
                    // Silenciar excepciones menores relacionadas con el refresco de UI.
                }
            }), System.Windows.Threading.DispatcherPriority.Background);
        }
        public async Task recargar()
        {

            facturat = new Factura()
            {
                Detalles = new ObservableCollection<DetalleFactura>()
            };
            this.DataContext = facturat;
            FAbierta = await turno.FacturasAbiertas(SesionUsuario.TurnoActual);
            bagabierta.Badge = FAbierta;
            RefrescarDetalleGrid();
            TotalFactura();
        }
        public async Task cargar()
        {
            if (!await turno.VerificarTurnoAbiertoAsync(SesionUsuario.Usuario))
            {
              var res= MessageBox.Show("No hay un turno abierto para este usuario. Por favor, abra un turno antes de facturar. Desea abrir un turno en este momento?", "Aviso!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res==MessageBoxResult.Yes)
                {
                    var abrirTurnoWindow = new VAbrirTurno();
                    abrirTurnoWindow.ShowDialog();
                }
               
                return;
            }
            tabControlCategorias.Items.Clear();
            FAbierta = await turno.FacturasAbiertas(SesionUsuario.TurnoActual);
            bagabierta.Badge = FAbierta;
            //  MessageBox.Show(FAbierta.ToString(), "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            facturat = new Factura()
            {
                Detalles = new ObservableCollection<DetalleFactura>()
            };
            facturat.Turno = SesionUsuario.TurnoActual;
            //LotesDs = await prd.OptenerLotes();
            // Asegurar que el DataContext apunte a la nueva factura para bindings.
            this.DataContext = facturat;

            // Inicializar/forzar el DataGrid con la colección Detalles.
            RefrescarDetalleGrid();

            foreach (var item in await categoriasP.ObtenerCategorias())
            {

                var nuevoTab = new TabItem
                {
                    Header = item.Nombre,
                    Content = item,
                    DataContext = item
                };
                // Agregar al TabControl
                tabControlCategorias.Items.Add(nuevoTab);
            }
            var c = await categoriasP.ObtenerTodosProductos();
            var neca = new Categoria()
            {
                Nombre = "TODOS",
                Productos = c,
            };
            var nuevoTabTodos = new TabItem
            {
                Header = neca.Nombre,
                Content = neca,
                DataContext = neca
            };
            tabControlCategorias.Items.Insert(0, nuevoTabTodos);
            tabControlCategorias.SelectedIndex = 0;
            txtcodproducto.Focus();
            TotalFactura();
        }


        private async void ListV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is Producto producto)
            {
                DetalleFactura ex = facturat.Detalles.FirstOrDefault(x => x.NombreProducto == producto.Nombre);

                if (ex != null)
                {
                    int index = facturat.Detalles.IndexOf(ex);
                    if (index >= 0)
                    {
                        facturat.Detalles[index].Cantidad++;
                    }

                    // Limpiar selección y salir
                    listView.SelectedIndex = -1;

                    // Refrescar DataGrid tras el cambio de cantidad
                    RefrescarDetalleGrid();
                    TotalFactura();
                    return;
                }
                else
                {
                    facturat.Detalles.Add(new DetalleFactura
                    {
                        NombreProducto = producto.Nombre,
                        PorcentajeITBIS = producto.ITBIS,
                        Cantidad = 1,
                        PrecioUnitario = producto.Precio,
                        Descuento = 0,
                        UnidadMedida = producto.Unidad.Nombre,
                        Producto = producto,
                    });
                    listView.SelectedIndex = -1;

                    // Refrescar DataGrid tras añadir nuevo detalle
                    RefrescarDetalleGrid();
                }

            }
            TotalFactura();
        }

        private async void btnabierta_Click(object sender, RoutedEventArgs e)
        {

            //   bagabierta.ToolTip = "Guardar Factura Abierta";
            //   bagabierta.BadgeChanged += (s, e) =>
            //   {
            //       // Lógica para manejar el cambio en la insignia
            //   };
            //   bagabierta.BadgeBackground = Brushes.Red;
            //   await   recargar();
            //   FAbierta++;
            //   bagabierta.Badge = FAbierta;

            var abierta = new VFacAbiertas();
            abierta.ShowDialog();
            if (abierta.DialogResult == true)
            {
                facturat = abierta.FacturaSelecionada;
                editando = true;
                // Asegurar que Detalles sea ObservableCollection
                if (facturat.Detalles == null)
                {
                    facturat.Detalles = new ObservableCollection<DetalleFactura>();
                }
                else if (!(facturat.Detalles is ObservableCollection<DetalleFactura> oc))
                {
                    facturat.Detalles = new ObservableCollection<DetalleFactura>(facturat.Detalles);
                }

                this.DataContext = facturat;
                data.ItemsSource = facturat.Detalles;
                RefrescarDetalleGrid();
                TotalFactura();


            }
        }

        private void tabControlCategorias_LostFocus(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Perdió el foco");
        }

        private void DataGrid_KeyUp(object sender, KeyEventArgs e)
        {
            TotalFactura();
        }
        public void TotalFactura()
        {
            facturat.SubTotal = facturat.Detalles.Sum(x => x.SubTotal);
            facturat.ITBIS = facturat.Detalles.Sum(x => x.ITBIS);
            facturat.Descuento = facturat.Detalles.Sum(x => x.Descuento);
            facturat.Total = facturat.SubTotal + facturat.ITBIS - facturat.Descuento;
        }

        private void DataGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            TotalFactura();
        }

        private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            TotalFactura();
            // A veces los cambios de celda no notifican, forzar refresco
            RefrescarDetalleGrid();
        }

        private void DataGrid_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TotalFactura();
        }

        private void DataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            TotalFactura();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                // Ejecutar SelectAll en la cola del Dispatcher para asegurar que 
                // la selección se aplique después de que el control gestione el foco.
                tb.Dispatcher.BeginInvoke(new Action(() => tb.SelectAll()), System.Windows.Threading.DispatcherPriority.Input);
            }

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            facturat.PagoCon = facturat.Total;
            facturat.Efectivo = facturat.Total;
            facturat.Condicion = "Contado";
            btnFacturar.Focus();
            facturat.Abierta = true;


            //   await facturas.ImprimirFactura(await facturas.ObtenerFacturaPorId(facturat));
            // await  recargar();
        }

        private void mbtneliminar_Click(object sender, RoutedEventArgs e)
        {
            if (data.SelectedItem is DetalleFactura detalle)
            {
                facturat.Detalles.Remove(detalle);
                // Refrescar el DataGrid después de eliminar
                RefrescarDetalleGrid();
                TotalFactura();
            }
            else
            {
                MessageBox.Show("No hay ningún detalle seleccionado para eliminar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F8)
            {
                Button_Click(sender, e);
            }
            if (e.Key == Key.F7)
            {

            }
            if (e.Key == Key.F12)
            {
                btnabierta_Click(sender, e);
            }
            if (e.Key == Key.Escape)
            {
                var result = MessageBox.Show("¿Está seguro que desea cancelar la factura actual?", "Confirmar Cancelación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _ = recargar();
                }
            }
            if (e.Key == Key.F11)
            {
                btnabierta_Click(sender, e);
            }
            if (e.Key == Key.F4)
            {
                Button_Click_1(sender, e);
            }
            if (e.Key == Key.F5)
            {
                Button_Click_2(sender, e);
            }
        }

        private async void btnVerAbiertas_Click(object sender, RoutedEventArgs e)
        {
            TotalFactura();
            if (facturat.Detalles.Count < 1)
            {
                MessageBox.Show("No es posible guardar una factura sin productos");
                return;
            }
            if (!editando)
            {
                facturat.Condicion = "Espera";
                facturat.Abierta = true;
                await facturas.GuardarFactura(facturat);
                // await facturas.ImprimirFactura(await facturas.ObtenerFacturaPorId(facturat));
                await recargar();
            }
            else
            {
                facturat.Condicion = "Espera";
                facturat.Abierta = true;
                await facturas.ActualizarFactura(facturat);
                //  MessageBox.Show("Factura actualizada correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                // await facturas.ImprimirFactura(await facturas.ObtenerFacturaPorId(facturat));
                await recargar();
                editando = false;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender is TextBox tb)
                {
                    // Mover el foco al siguiente control
                   // MessageBox.Show("Entró al Enter");
                    btnFacturar.Focus();
                    //tb.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }

        }

        private async void btnFacturar_Click(object sender, RoutedEventArgs e)
        {
            //if (facturat.Efectivo < facturat.Total)
            //{
            //    MessageBox.Show("No es posible la facturación a crédito. El efectivo no puede ser menor que el total de la factura.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}
            if (facturat.Detalles.Count() == 0)
            {
                MessageBox.Show("No es posible la facturación sin productos ", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!editando)
            {
                facturat.Condicion = "Contado";
                facturat.Abierta = false;
                if (facturat.Efectivo < facturat.Total)
                {
                    if (MessageBox.Show("Desea Facturar a credito","Aviso!",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
                    {
                        foreach (var item in facturat.Detalles)
                        {
                            if (!item.Producto?.AplicaCredito??true)
                            {
                                MessageBox.Show($"El producto {item.NombreProducto} no se puede facturar a crédito. Por favor, ajuste la cantidad o elimine el producto para continuar.", "Producto no apto para crédito", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;

                            }
                           
                        }
                        facturat.Condicion = "Credito";
                       facturat.TotalPendiente = facturat.Total-facturat.Efectivo;
                        facturat.TotalPagado = facturat.Efectivo;
                        facturat.EsContado = false;
                    }
                    else
                    {
                        return;
                    }

                }
               

                if (await facturas.GuardarFactura(facturat))
                {
                    if (SesionUsuario.Configuracion.FacGrade)
                    {
                        await facturas.ImprimirFacturaGrande(await facturas.ObtenerFacturaPorId(facturat));

                    }
                    else
                    {
                        await facturas.ImprimirFactura(await facturas.ObtenerFacturaPorId(facturat));
                    }
                    await recargar();

                }
                ;
            }
            else
            {
                if (facturat.Efectivo < facturat.Total)
                {
                    MessageBox.Show("El efectivo no puede ser menor que el total de la factura.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                facturat.Condicion = "Contado";
                facturat.Abierta = false;
                if (await facturas.ActualizarFactura(facturat))
                {
                    if (SesionUsuario.Configuracion.FacGrade)
                    {
                        await facturas.ImprimirFacturaGrande(await facturas.ObtenerFacturaPorId(facturat));

                    }
                    else
                    {
                        await facturas.ImprimirFactura(await facturas.ObtenerFacturaPorId(facturat));
                    }
                    editando = false;

                    await recargar();


                }
                ;
                //   MessageBox.Show("Factura actualizada correctamente.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);

            }


            txtcodproducto.Focus();

        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (false)
            {
                VBuscarAsog vBuscarAsog = new VBuscarAsog(await persona.ObtenerPersonasASO());
                vBuscarAsog.ShowDialog();
                if (vBuscarAsog != null)
                {
                    if (vBuscarAsog.entiddadselecionada != null)
                    {

                        facturat.NombreCliente = vBuscarAsog.entiddadselecionada.NOMBRE;
                        facturat.TelefonoCliente = vBuscarAsog.entiddadselecionada.TELEFONO1 ?? "";
                        facturat.RNCCliente = vBuscarAsog.entiddadselecionada.RNC ?? "";
                        facturat.DireccionCliente = vBuscarAsog.entiddadselecionada.DIRECCION ?? "";
                        facturat.CodigoSocio = vBuscarAsog.entiddadselecionada.CODIGO;
                    }



                }
            }
            else
            {

                VBuscarPersona vBuscarAsog = new  VBuscarPersona();
                vBuscarAsog.ShowDialog();
                if (vBuscarAsog != null)
                {
                    if (vBuscarAsog.PersonaSelecionada != null)
                    {
                        facturat.ClienteP= vBuscarAsog.PersonaSelecionada;
                        facturat.NombreCliente = vBuscarAsog.PersonaSelecionada.Nombre;
                        facturat.TelefonoCliente = vBuscarAsog.PersonaSelecionada.Telefono ?? "";
                        facturat.RNCCliente = vBuscarAsog.PersonaSelecionada.Identificacion ?? "";
                        facturat.DireccionCliente = vBuscarAsog.PersonaSelecionada.Direccion ?? "";
                        facturat.CodigoSocio = vBuscarAsog.PersonaSelecionada.CodSocioAsogafar??"";
                    }



                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            VBuscarProducto vBuscarProducto = new VBuscarProducto();
            vBuscarProducto.ShowDialog();
            if (vBuscarProducto.ProductoSeleccionado != null)
            {
                var producto = vBuscarProducto.ProductoSeleccionado;
                DetalleFactura ex = facturat.Detalles.FirstOrDefault(x => x.NombreProducto == producto.Nombre);
                if (ex != null)
                {
                    int index = facturat.Detalles.IndexOf(ex);
                    if (index >= 0)
                    {
                        facturat.Detalles[index].Cantidad++;
                    }
                    // Refrescar DataGrid tras el cambio de cantidad
                    RefrescarDetalleGrid();
                    TotalFactura();
                    return;
                }
                else
                {
                    facturat.Detalles.Add(new DetalleFactura
                    {
                        NombreProducto = producto.Nombre,
                        PorcentajeITBIS = producto.ITBIS,
                        Cantidad = 1,
                        PrecioUnitario = producto.Precio,
                        Descuento = 0,
                        UnidadMedida = producto.Unidad.Nombre,
                    });
                    // Refrescar DataGrid tras añadir nuevo detalle
                    RefrescarDetalleGrid();
                    TotalFactura();
                }
            }
        }

        private void TextBox_GotFocus_1(object sender, RoutedEventArgs e)
        {
            txtcliente.SelectAll();
        }

        private async void TextBox_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var pro = await categoriasP.ObtenerTodosProductos();
                var prodencontrad = pro.FirstOrDefault(x => x.Referencia == txtcodproducto.Text);
                if (prodencontrad != null)
                {
                    EntrarenDetalle(prodencontrad);
                    txtcodproducto.Clear();
                    return;
                }
                else
                {
                    var pordid = pro.FirstOrDefault(x => x.Id.ToString() == txtcodproducto.Text);
                    if (pordid != null)
                    {
                        EntrarenDetalle(pordid);
                        txtcodproducto.Clear();
                        return;
                    }
                    MessageBox.Show("No se encontró ningún producto con la referencia proporcionada.", "Producto no encontrado", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtcodproducto.Clear();

                }
            }

        }
        private async void EntrarenDetalle(Producto producto)
        {
            DetalleFactura ex = facturat.Detalles.FirstOrDefault(x => x.NombreProducto == producto.Nombre);
            if (ex != null)
            {
                int index = facturat.Detalles.IndexOf(ex);
                if (index >= 0)
                {
                    facturat.Detalles[index].Cantidad++;
                }
                // Limpiar selección y salir
                // Refrescar DataGrid tras el cambio de cantidad
                RefrescarDetalleGrid();
                TotalFactura();
                return;
            }
            else
            {
                facturat.Detalles.Add(new DetalleFactura
                {
                    NombreProducto = producto.Nombre,
                    PorcentajeITBIS = producto.ITBIS,
                    Cantidad = 1,
                    PrecioUnitario = producto.Precio,
                    Descuento = 0,
                    UnidadMedida = producto.Unidad.Nombre,
                });
                // Refrescar DataGrid tras añadir nuevo detalle
                RefrescarDetalleGrid();
            }
            TotalFactura();
        }

        private async void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            await cargar();
        }

        private void tabControlCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtcodproducto_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtcodproducto.Text.FirstOrDefault() == ' ')
            {
                Button_Click_2(sender, e);
            }
        }
    }
}
