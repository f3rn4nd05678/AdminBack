using AdminBack.Models.DTOs;

namespace AdminBack.Service.IService
{
    public interface IGestionFacturasService
    {
        Task<bool> AnularFactura(int pedidoId);
        Task<List<ReporteClienteDto>> ObtenerReportePorCliente(int clienteId);

    }

}
