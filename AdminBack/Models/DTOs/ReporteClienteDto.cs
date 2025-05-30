namespace AdminBack.Models.DTOs
{
    public class ReporteClienteDto
    {
        public int PedidoId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal TotalFactura { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal TotalNotasCredito { get; set; }
        public decimal Pendiente { get; set; }
    }

}
