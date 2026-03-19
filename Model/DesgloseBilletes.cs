using System;
using System.Collections.Generic;
using System.Text;

namespace SFCH.Model
{
    public class DesgloseBilletes
    {
        public int Id { get; set; }
        public decimal Denominacion { get; set; }   
        public int Cantidad { get; set; }
        public decimal Total
        {

            get
            {
                return Denominacion * Cantidad;
            }
            set
            {
                Total = field;
            }
        }
        public string Texto =>@$"{Cantidad} X {Denominacion.ToString("C")} = {Total.ToString("C")}";
        public virtual Turno? Turno { get; set; }

    }
}
