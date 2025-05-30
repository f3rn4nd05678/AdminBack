namespace AdminBack.Models.DTOs
{
    public class ReporteVentaPagoDto
    {
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public decimal TotalFactura { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal Pendiente { get; set; }
    }

}
