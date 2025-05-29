namespace AdminBack.Models.DTOs.Proveedor
{
    public class ProveedorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? CatalogoUrl { get; set; }
        public string? TrackUrl { get; set; }
        public bool Activo { get; set; }
    }
}
