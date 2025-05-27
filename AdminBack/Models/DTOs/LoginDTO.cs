using System.ComponentModel.DataAnnotations;

namespace AdminBack.Models.DTOs
{
    // Solo para login
    public class LoginDto
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; } = string.Empty;
    }

    // Solo para respuesta de login
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public Usuario Usuario { get; set; } = null!;
    }

    // Solo para cambiar contraseña
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "La contraseña actual es requerida")]
        public string PasswordActual { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
        public string PasswordNueva { get; set; } = string.Empty;
    }
}