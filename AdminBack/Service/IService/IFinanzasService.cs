using AdminBack.Models.DTOs;

namespace AdminBack.Service.IService
{
    public interface IFinanzasService
    {
        Task<List<DashboardFinanzasDto>> ObtenerResumenFinanciero(DateTime inicio, DateTime fin);
        Task<List<DashboardFinanzasDetalladoDto>> ObtenerResumenDetallado(DateTime inicio, DateTime fin);

    }

}
