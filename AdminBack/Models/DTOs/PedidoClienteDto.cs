namespace AdminBack.Models.DTOs.PedidoCliente
{
    public class PedidoClienteDto
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public List<DetallePedidoDto> Detalles { get; set; } = new();
    }
}
