namespace AdminBack.Models
{
    public class PagoProveedor
    {
        public int Id { get; set; }
        public int OrdenId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public string? Referencia { get; set; }

        public virtual OrdenCompra Orden { get; set; } = null!;
    }

}
