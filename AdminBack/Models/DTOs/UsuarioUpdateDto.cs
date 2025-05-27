namespace AdminBack.Models.DTOs.Usuario
{
    public class UsuarioUpdateDto
    {
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public int RolId { get; set; }
        public bool Activo { get; set; }
    }
}
