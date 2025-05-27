
namespace AdminBack.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }

        public ICollection<Usuario>? Usuarios { get; set; }
    }
}
