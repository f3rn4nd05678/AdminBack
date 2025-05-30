using AdminBack.Models.DTOs;

public interface INotaCreditoService
{
    Task<bool> Registrar(NotaCreditoDto dto);
    Task<List<NotaCreditoDetalleDto>> ObtenerPorPedido(int pedidoId);
}
