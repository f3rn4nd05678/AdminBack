public class PagoClienteDetalleDto
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public string Cliente { get; set; }
    public decimal Monto { get; set; }
    public DateTime FechaPago { get; set; }
    public string? Referencia { get; set; }
}
