namespace AdminBack.Models.DTOs
{
    public class PagoProveedorDto
    {
        public int OrdenId { get; set; }
        public decimal Monto { get; set; }
        public string? Referencia { get; set; }
    }

}
