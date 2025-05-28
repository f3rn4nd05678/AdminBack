namespace AdminBack.Models.DTOs.Almacen
{
    public class AlmacenDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Ubicacion { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
