public class NotaCredito
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public decimal Monto { get; set; }
    public string Motivo { get; set; } = null!;
    public DateTime Fecha { get; set; }

    public virtual PedidoCliente Pedido { get; set; } = null!;
}
