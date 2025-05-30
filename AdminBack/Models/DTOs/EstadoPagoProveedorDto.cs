namespace AdminBack.Models.DTOs
{
    public class EstadoPagoProveedorDto
    {
        public int OrdenId { get; set; }
        public string Proveedor { get; set; } = "";
        public decimal TotalOrden { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal Pendiente { get; set; }
        public string Estado { get; set; }
    }

}
