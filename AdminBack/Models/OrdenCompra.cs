using AdminBack.Models;

public class OrdenCompra
{
    public int Id { get; set; }
    public int ProveedorId { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string Estado { get; set; } = "Pendiente";
    public decimal TotalEstimado { get; set; }
    public bool Enviada { get; set; }

    public virtual Proveedore Proveedor { get; set; } = null!;
    public virtual ICollection<DetalleOrdenCompra> Detalles { get; set; } = new List<DetalleOrdenCompra>();
}
