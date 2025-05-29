using AdminBack.Models.DTOs.Proveedor;

namespace AdminBack.Service.IService
{
    public interface IProveedorHttpService
    {
        Task<List<ProductoCatalogoProveedorDto>> ObtenerYGuardarCatalogo(int proveedorId);
        Task<object?> ConsultarTracking(int proveedorId, int ordenId);

    }
}
