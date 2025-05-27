namespace AdminBack.Models.DTOs.Usuario
{
    public class UsuarioCreateDto
    {
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public int RolId { get; set; }
    }
}
