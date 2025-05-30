namespace AdminBack.Models.DTOs
{
    public class PagoProveedorDetalleDto
    {
        public int Id { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string? Referencia { get; set; }
        public string Proveedor { get; set; } = "";
    }

}
