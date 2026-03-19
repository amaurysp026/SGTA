using SFCH.Controller;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SFCH.Model
{
    public class Factura:INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Numero { get; set; } =string.Empty;
        public bool Abierta { get; set; } = true;
        public bool Anulada { get; set; } = false;
        public Entidad? Cliente { get;set; } 
        public Persona? ClienteP { get; set; }
        public string Tipo { get; set; } = "FAC";
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        private string _nombreCliente = "CLIENTE GENERICO";
        public string NombreCliente
        {
            get { return _nombreCliente; }
            set
            {
                if (_nombreCliente != value)
                {
                    _nombreCliente = value;
                  OnPropertyChanged(nameof(NombreCliente));
                }
            }

        }
        private string _RNCCliente = string.Empty;
        public string RNCCliente
        {
            get { return _RNCCliente; }
            set
            {
                if (_RNCCliente != value)
                {
                    _RNCCliente = value;
                   OnPropertyChanged(nameof(RNCCliente));
                }
            }

        }
        public string DireccionCliente { get; set; } = string.Empty;
        public string TelefonoCliente { get; set; } = string.Empty;
        public string CorreoCliente { get; set; } = string.Empty;
        private string _codigoSocio = string.Empty;
        public string CodigoSocio {  get { return _codigoSocio; }
            set
            {
                if (_codigoSocio != value)
                {
                    _codigoSocio = value;
                   OnPropertyChanged(nameof(CodigoSocio));
                }
            }
        }
        public string NombreVendedor { get; set; } = SesionUsuario.Usuario.NombreCompleto;
        public bool EsContado { get; set; }
        public virtual Turno Turno { get; set; } = null!;

        private ObservableCollection<DetalleFactura> _detalles = new ObservableCollection<DetalleFactura>();
                public ObservableCollection<DetalleFactura> Detalles
                {
                    get { return _detalles; }
                    set
                    {
                        if (_detalles != value)
                        {
                            // Desuscribir de la colección antigua y de sus ítems
                            UnsubscribeFromCollection(_detalles);
                            UnsubscribeFromAllDetalles(_detalles);

                            _detalles = value ?? new ObservableCollection<DetalleFactura>();

                            // Suscribir a la nueva colección y a sus ítems
                            SubscribeToCollection(_detalles);
                            SubscribeToAllDetalles(_detalles);

                            OnPropertyChanged(nameof(Detalles));
                            calcularTotal(); // Detalles pueden afectar SubTotal en la lógica de negocio externa
                        }
                    }
                }

        private decimal _subTotal;
        public decimal SubTotal { get { return _subTotal; }
            set
            {
                if (_subTotal != value)
                {
                    _subTotal = value;
                    OnPropertyChanged(nameof(SubTotal));
                    calcularTotal();
                }
            }
        }
        private decimal _itbis;
        public decimal ITBIS { get { return _itbis; }
            set
            {
                if (_itbis != value)
                {
                    _itbis = value;
                    OnPropertyChanged(nameof(ITBIS));
                    calcularTotal();
                }
            }
        }
        private decimal _descuento;
        public decimal Descuento
        {
            get { return _descuento; }
            set
            {
                if (_descuento != value)
                {
                    _descuento = value;
                    OnPropertyChanged(nameof(Descuento));
                    calcularTotal();
                }
            }
        }
        private decimal _total;
        public string Condicion { get; set; } = string.Empty;
        public string NCF { get; set; } = string.Empty;
        public string TipoNCF { get; set; } = string.Empty;
        public string Consumo { get; set; } = string.Empty;
        public int DiasCredito { get; set; } = 0;
       
        public event PropertyChangedEventHandler? PropertyChanged;

        public decimal Total
            {
            get { return _total; }
            set
            {
                if (_total != value)
                {
                    _total = value;
                    OnPropertyChanged(nameof(Total));
                    calcularTotal();
                }
            }
        }

        private decimal _efectivo;
        public decimal Efectivo { get { return _efectivo; }
            set
            {
                if (_efectivo != value)
                {
                    _efectivo = value;
                    OnPropertyChanged(nameof(Efectivo));
                    calcularTotal();
                }
            }
        }
        private decimal _tarjeta;
        public decimal Tarjeta { get { return _tarjeta; }
            set
            {
                if (_tarjeta != value)
                {
                    _tarjeta = value;
                    OnPropertyChanged(nameof(Tarjeta));
                    calcularTotal();
                }
            }
        }
        private decimal _transferencia;
        public decimal Transferencia
        {
            get { return _transferencia; }
            set
            {
                if (_transferencia != value)
                {
                    _transferencia = value;
                   OnPropertyChanged(nameof(Transferencia));
                   calcularTotal();
                }
            }
        }
        private decimal _cheque;
        public decimal Cheque { get { return _cheque; }
            set
            {
                if (_cheque != value)
                {
                    _cheque = value;
                    OnPropertyChanged(nameof(Cheque));
                    calcularTotal();
                }
            }
        }
        private decimal _totalPagado;
        public decimal TotalPagado { get { return _totalPagado; }
            set
            {
                if (_totalPagado != value)
                {
                    _totalPagado = value;
                    OnPropertyChanged(nameof(TotalPagado));
                }
            }
        }
        private decimal _totalPendiente;
        public decimal TotalPendiente { get { return _totalPendiente; }
            set
            {
                if (_totalPendiente != value)
                {
                    _totalPendiente = value;
                    OnPropertyChanged(nameof(TotalPendiente));
                }
            }
        }
        public decimal PagoCon
        {
            get { return field; }
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(PagoCon));
                    calcularTotal();
                }
            }
        }
        public decimal Propina
        {
            get { return field; }
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(Propina));
                }
            }
        }
        public int DiasPlazo
        {
            get { return field; }
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(DiasPlazo));
                }
            }
        }
        public string? Color { get; set; }
        private decimal _cambio;
        public decimal Cambio
        {
            get { return _cambio; }
            set
            {
                if (_cambio != value)
                {
                    _cambio = value;
                    OnPropertyChanged(nameof(Cambio));
                }
            }
        }

        // Bandera para evitar reentradas durante el cálculo
        private bool _isCalculating;
       // public string? Observacion { get; set; }
        // Constructor: suscribir la colección inicial
        public Factura()
        {
            SubscribeToCollection(_detalles);
            SubscribeToAllDetalles(_detalles);
        }

        private void OnPropertyChanged(string propertyName)
        {
            // No llamar a calcularTotal() aquí para evitar recursión infinita.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void calcularTotal()
        {
            if (_isCalculating) return;

            try
            {
                _isCalculating = true;
                Cambio = PagoCon - Total;
                if (PagoCon >= Total)
                {
                    Efectivo = Total ;
                }
                TotalPagado = Efectivo + Cheque + Tarjeta + Transferencia;
                
                TotalPendiente = Total - TotalPagado;

                if (TotalPendiente < 0)
                {
                    TotalPendiente = 0;
                }

                if (Cambio < 0)
                {
                    Cambio = 0;
                }
            }
            finally
            {
                _isCalculating = false;
            }
        }

        // ---------- Gestión de suscripciones a la colección y a los ítems ----------

        private void SubscribeToCollection(ObservableCollection<DetalleFactura> collection)
        {
            if (collection != null)
            {
                collection.CollectionChanged += OnDetallesCollectionChanged;
            }
        }

        private void UnsubscribeFromCollection(ObservableCollection<DetalleFactura> collection)
        {
            if (collection != null)
            {
                collection.CollectionChanged -= OnDetallesCollectionChanged;
            }
        }

        private void SubscribeToAllDetalles(IEnumerable<DetalleFactura> detalles)
        {
            if (detalles == null) return;
            foreach (var d in detalles)
            {
                SubscribeToDetalle(d);
            }
        }

        private void UnsubscribeFromAllDetalles(IEnumerable<DetalleFactura> detalles)
        {
            if (detalles == null) return;
            foreach (var d in detalles)
            {
                UnsubscribeFromDetalle(d);
            }
        }

        private void SubscribeToDetalle(DetalleFactura detalle)
        {
            if (detalle is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged += OnDetallePropertyChanged;
            }
        }

        private void UnsubscribeFromDetalle(DetalleFactura detalle)
        {
            if (detalle is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged -= OnDetallePropertyChanged;
            }
        }

        private void OnDetallesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e == null) return;

            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (var item in e.NewItems.OfType<DetalleFactura>())
                {
                    SubscribeToDetalle(item);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach (var item in e.OldItems.OfType<DetalleFactura>())
                {
                    UnsubscribeFromDetalle(item);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems.OfType<DetalleFactura>())
                    {
                        UnsubscribeFromDetalle(item);
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems.OfType<DetalleFactura>())
                    {
                        SubscribeToDetalle(item);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                // El reset puede significar que la colección fue limpiada; quitar suscripciones y volver a suscribir las actuales
                // Primero intentamos limpiar todas suscripciones del sender si es posible (no siempre tenemos OldItems)
                if (sender is ObservableCollection<DetalleFactura> col)
                {
                    UnsubscribeFromAllDetalles(col);
                    SubscribeToAllDetalles(col);
                }
            }

            // Los cambios en la colección afectan el total
            calcularTotal();
        }

        private void OnDetallePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Si alguna propiedad de un detalle cambia (por ejemplo cantidad, precio, subtotal), recalcular total.
            calcularTotal();
        }
    }
}
