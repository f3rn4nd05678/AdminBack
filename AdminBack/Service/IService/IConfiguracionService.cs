using AdminBack.Models.DTOs.Config;

namespace AdminBack.Service.IService
{
    public interface IConfiguracionService
    {
        Task<List<ConfiguracionDto>> ObtenerTodo();
        Task<ConfiguracionDto?> ObtenerPorClave(string clave);
        Task<bool> Crear(ConfiguracionCreateDto dto);
        Task<bool> Actualizar(string clave, string nuevoValor);
    }
}
