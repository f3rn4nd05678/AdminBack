namespace AdminBack.Models.DTOs.Inventario
{
    public class EntradaInventarioDto
    {
        public int Id { get; set; }
        public string Producto { get; set; }
        public string Almacen { get; set; }
        public decimal Cantidad { get; set; }
        public DateTime FechaEntrada { get; set; }
        public string? Referencia { get; set; }
        public string Usuario { get; set; }
    }
}
