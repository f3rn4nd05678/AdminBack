namespace AdminBack.Models.DTOs.Inventario
{
    public class InventarioActualDto
    {
        public int ProductoId { get; set; }
        public string Producto { get; set; }
        public int AlmacenId { get; set; }
        public string Almacen { get; set; }
        public decimal Cantidad { get; set; }
    }
}
