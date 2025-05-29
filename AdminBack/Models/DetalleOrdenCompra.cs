using AdminBack.Models;

public class DetalleOrdenCompra
{
    public int Id { get; set; }
    public int OrdenId { get; set; }
    public int ProductoId { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }

    public virtual OrdenCompra Orden { get; set; } = null!;
    public virtual Producto Producto { get; set; } = null!;
}
