namespace AdminBack.Models.DTOs
{
    public class PagoClienteDto
    {
        public int PedidoId { get; set; }
        public decimal Monto { get; set; }
        public string? Referencia { get; set; }
    }

}
