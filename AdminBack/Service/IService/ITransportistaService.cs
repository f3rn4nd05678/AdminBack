using AdminBack.Models.DTOs;

namespace AdminBack.Service.IService
{
    public interface ITransportistaService
    {
        Task<List<TransportistaDto>> ObtenerTodos();
        Task<TransportistaDto?> ObtenerPorId(int id);
        Task<bool> Crear(TransportistaCreateDto dto);
        Task<bool> Actualizar(int id, TransportistaCreateDto dto);
        Task<bool> Desactivar(int id);
    }

}
