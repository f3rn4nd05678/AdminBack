namespace AdminBack.Models.DTOs.OrdenCompra
{
    public class OrdenCompraDto
    {
        public int Id { get; set; }
        public string Proveedor { get; set; }
        public string Estado { get; set; }
        public decimal TotalEstimado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Enviada { get; set; }
        public List<DetalleOrdenCompraDto> Detalles { get; set; } = new();
    }
}
