namespace AdminBack.Models
{
    public class PagoCliente
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public string? Referencia { get; set; }

        public virtual PedidoCliente Pedido { get; set; } = null!;
    }

}
