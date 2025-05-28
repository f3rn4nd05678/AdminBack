namespace AdminBack.Models.DTOs.Producto
{
    public class ProductoCreateDto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string UnidadMedida { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
