using AdminBack.Models.DTOs.Proveedor;

namespace AdminBack.Service.IService
{
    public interface IProveedorService
    {
        Task<List<ProveedorDto>> ObtenerTodos();
        Task<bool> Crear(ProveedorCreateDto dto);
        Task<bool> Actualizar(int id, ProveedorUpdateDto dto);
        Task<bool> Desactivar(int id);
    }
}
