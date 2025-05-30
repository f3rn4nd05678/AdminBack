namespace AdminBack.Models.DTOs
{
    public class DashboardProveedorDto
    {
        public string Proveedor { get; set; } = "";
        public decimal TotalOrdenado { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal Pendiente { get; set; }
    }

}
