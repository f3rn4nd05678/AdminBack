using System.Threading.Tasks;

namespace AdminBack.Service.IService
{
    public interface IFacturaVentaService
    {
        Task<object?> GenerarFacturaJson(int pedidoId);
        Task<object?> ObtenerFacturaDesdeMongo(int pedidoId);

    }
}
