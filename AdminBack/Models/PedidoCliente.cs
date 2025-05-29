using AdminBack.Models;

public class PedidoCliente
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = "Pendiente";
    public decimal Total { get; set; }
    public string? FacturaIdMongo { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;
    public virtual ICollection<DetallePedidoCliente> Detalles { get; set; } = new List<DetallePedidoCliente>();
}
