namespace AdminBack.Models.DTOs
{
    public class TransportistaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string? Telefono { get; set; }
        public bool Activo { get; set; }
    }

}
