namespace AdminBack.Models.DTOs.PedidoCliente
{
    public class PedidoClienteCreateDto
    {
        public int ClienteId { get; set; }
        public List<DetallePedidoDto> Detalles { get; set; } = new();
    }

    public class DetallePedidoDto
    {
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
