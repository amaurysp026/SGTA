using SFCH.Controller;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace SFCH.Model
{
    public class ContInventario: INotifyPropertyChanged
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public DateTime FechaConteo { get; set; } = DateTime.Now;
        public virtual Usuario Usuario { get; set; } = SesionUsuario.Usuario;
        public string? Observacion {
            get
            {
                return field;
            }
            set
            {
                field = value;
                OnPropertyChanged(nameof(Detalle));
            }
        }
        public string Estado
        {
            get
            {
                return field;
            }
            set
            {
                field = value;
                OnPropertyChanged(nameof(Detalle));
            }
        } = "En progreso";
        public bool Aplicado { get; set; } = false;
        public virtual ObservableCollection<DetalleContInventario> Detalle
        {
            get
            {
                if (field == null)
                {
                    field = new ObservableCollection<DetalleContInventario>();
                }
                return field;
            }
            set
            {
                field = value;
                OnPropertyChanged(nameof(Detalle));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
            private void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
