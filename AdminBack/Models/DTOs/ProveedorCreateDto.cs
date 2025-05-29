namespace AdminBack.Models.DTOs.Proveedor
{
    public class ProveedorCreateDto
    {
        public string Nombre { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? CatalogoUrl { get; set; }
        public string? TrackUrl { get; set; }
    }
}
