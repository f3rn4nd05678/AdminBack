using AdminBack.Models.DTOs;

namespace AdminBack.Service.IService
{
    public interface IEntregaService
    {
        Task<bool> Asignar(EntregaAsignarDto dto);
        Task<bool> CambiarEstado(int entregaId, string nuevoEstado);
        Task<List<EntregaDetalleDto>> ObtenerTodas();
        Task<List<EntregaDetalleDto>> ObtenerPorPedido(int pedidoId);
    }

}
