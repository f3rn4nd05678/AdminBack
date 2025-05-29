namespace AdminBack.Models.DTOs.OrdenCompra
{
    public class OrdenCompraCreateDto
    {
        public int ProveedorId { get; set; }
        public List<DetalleOrdenCompraDto> Detalles { get; set; } = new();
    }

    public class DetalleOrdenCompraDto
    {
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
