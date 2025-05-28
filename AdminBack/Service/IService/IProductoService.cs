using AdminBack.Models.DTOs.Producto;

namespace AdminBack.Service.IService
{
    public interface IProductoService
    {
        Task<List<ProductoDto>> ObtenerTodos();
        Task<bool> Crear(ProductoCreateDto dto);
        Task<bool> Actualizar(int id, ProductoUpdateDto dto);
        Task<bool> Desactivar(int id);

    }
}
