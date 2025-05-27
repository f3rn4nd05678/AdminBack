using AdminBack.Models.DTOs.Menu;

namespace AdminBack.Service.IService
{
    public interface IMenuService
    {
        Task<List<MenuItemDto>> ObtenerMenuPorRol(int rolId);
    }
}
