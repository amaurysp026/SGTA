namespace SFCH.Model
{
    public class Transaccion
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public decimal Capital { get; set; }
        public decimal Interes { get; set; }
        public decimal Otros { get; set; }
        public decimal MontoCredito { get; set; }
        public decimal MontoDebito { get; set; }
        public string Tipo { get; set; } = null!;
        public string Descripcion { get; set; } = string.Empty;
        public string Medio { get; set; } = string.Empty;
        public string Referencia { get; set; } = string.Empty;
        public DateTime FechaOriginal { get; set; }= DateTime.Now;
        public virtual Cuenta Cuenta { get; set; } = null!;
        public string Usuario { get; set; } = string.Empty;
        public string Caja { get; set; } = string.Empty;
        public string Cliente { get; set; } = null!;
        public string Observaciones { get; set; } = string.Empty;
        public bool Nula { get; set; } = false;
    }
}