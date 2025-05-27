namespace AdminBack.Models.DTOs
{
    public class UsuarioRegisterDto
    {
        public string NombreCompleto { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
        public int? RolId { get; set; }
    }
}
