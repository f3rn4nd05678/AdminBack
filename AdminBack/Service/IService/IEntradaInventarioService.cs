using AdminBack.Models.DTOs.Inventario;

namespace AdminBack.Service.IService
{
    public interface IEntradaInventarioService
    {
        Task<List<EntradaInventarioDto>> ObtenerTodas();
        Task<bool> Registrar(EntradaInventarioCreateDto dto, int usuarioId);
    }
}
