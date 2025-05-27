namespace AdminBack.Models.DTOs.Inventario
{
    public class EntradaInventarioCreateDto
    {
        public int ProductoId { get; set; }
        public int AlmacenId { get; set; }
        public decimal Cantidad { get; set; }
        public string? Referencia { get; set; }
    }
}
