using AdminBack.Models.DTOs.PedidoCliente;

namespace AdminBack.Service.IService
{
    public interface IPedidoClienteService
    {
        Task<List<PedidoClienteDto>> ObtenerTodos();
        Task<bool> Crear(PedidoClienteCreateDto dto, int usuarioId, int almacenId);
    }
}
