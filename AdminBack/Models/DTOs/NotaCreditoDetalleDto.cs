namespace AdminBack.Models.DTOs
{
    public class NotaCreditoDetalleDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Motivo { get; set; }
    }

}
