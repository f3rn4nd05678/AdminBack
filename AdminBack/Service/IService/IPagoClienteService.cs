using AdminBack.Models.DTOs;

namespace AdminBack.Service.IService
{
    public interface IPagoClienteService
    {
        Task<bool> RegistrarPago(PagoClienteDto dto);
        Task<List<PagoClienteDetalleDto>> ObtenerTodos();
        Task<List<PagoClienteDetalleDto>> ObtenerPorPedido(int pedidoId);
        Task<List<PagoClienteDetalleDto>> ObtenerPorCliente(int clienteId);
        Task<EstadoPagoPedidoDto> ObtenerEstadoPagoPorPedido(int pedidoId);


    }
}
