namespace AdminBack.Models.DTOs
{
    public class EstadoPagoPedidoDto
    {
        public int PedidoId { get; set; }
        public decimal TotalFactura { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal Pendiente { get; set; }
        public string Estado { get; set; }
    }

}
