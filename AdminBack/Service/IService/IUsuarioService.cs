using AdminBack.Models.DTOs.Usuario;

namespace AdminBack.Service.IService
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDto>> ObtenerTodos();
        Task<UsuarioDto?> ObtenerPorId(int id);
        Task<bool> Crear(UsuarioCreateDto dto);
        Task<bool> Desactivar(int id);
        Task<bool> Actualizar(int id, UsuarioUpdateDto dto);

    }
}
