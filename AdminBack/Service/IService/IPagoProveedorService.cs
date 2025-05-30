using AdminBack.Models.DTOs;

namespace AdminBack.Service.IService
{
    public interface IPagoProveedorService
    {
        Task<bool> RegistrarPago(PagoProveedorDto dto);
        Task<List<PagoProveedorDetalleDto>> ObtenerPorOrden(int ordenId);
        Task<List<PagoProveedorDetalleDto>> ObtenerPorProveedor(int proveedorId);
        Task<EstadoPagoProveedorDto> ObtenerEstadoPagoPorOrden(int ordenId);

    }

}
