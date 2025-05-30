namespace AdminBack.Models.DTOs
{
    public class EntregaDetalleDto
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public string Transportista { get; set; } = "";
        public string Ruta { get; set; } = "";
        public string Estado { get; set; } = "";
        public DateTime FechaAsignacion { get; set; }
    }

}
