using AdminBack.Models.DTOs.OrdenCompra;

namespace AdminBack.Service.IService
{
    public interface IOrdenCompraService
    {
        Task<List<OrdenCompraDto>> ObtenerTodas();
        Task<bool> Crear(OrdenCompraCreateDto dto);
        Task<bool> Aprobar(int id);
        Task<bool> Aprobar(int id, int almacenId);

    }
}
