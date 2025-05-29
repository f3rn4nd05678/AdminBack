using AdminBack.Models;

public class DetallePedidoCliente
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ProductoId { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }

    public virtual PedidoCliente Pedido { get; set; } = null!;
    public virtual Producto Producto { get; set; } = null!;
}
