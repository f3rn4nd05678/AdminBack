using AdminBack.Models.DTOs.Cliente;

namespace AdminBack.Service.IService
{
    public interface IClienteService
    {
        Task<List<ClienteDto>> ObtenerTodos();
        Task<bool> Crear(ClienteCreateDto dto);
        Task<bool> Actualizar(int id, ClienteUpdateDto dto);
        Task<bool> Desactivar(int id);
    }
}
