using AdminBack.Models.DTOs.Inventario;

namespace AdminBack.Service.IService
{
    public interface IInventarioService
    {
        Task<List<InventarioActualDto>> ConsultarStock();
        Task<List<EntradaInventarioDto>> ObtenerEntradas();
        Task<List<SalidaInventarioDto>> ObtenerSalidas();
        Task<bool> RegistrarEntrada(EntradaInventarioCreateDto dto, int usuarioId);
        Task<bool> RegistrarSalida(SalidaInventarioCreateDto dto, int usuarioId);
    }
}
