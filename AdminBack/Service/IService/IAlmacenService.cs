using AdminBack.Models.DTOs.Almacen;

namespace AdminBack.Service.IService
{
    public interface IAlmacenService
    {
        Task<List<AlmacenDto>> ObtenerTodos();
        Task<bool> Crear(AlmacenCreateDto dto);
        Task<bool> Actualizar(int id, AlmacenUpdateDto dto);
        Task<bool> Desactivar(int id);
    }
}
