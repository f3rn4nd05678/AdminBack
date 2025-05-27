using AdminBack.Data;
using AdminBack.Models.DTOs.Menu;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AdminBack.Service
{
    public class MenuService : IMenuService
    {
        private readonly AdminDbContext _context;

        public MenuService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuItemDto>> ObtenerMenuPorRol(int rolId)
        {
            using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT sp_obtener_menu_rol_jerarquico(@rolId)";
            var param = command.CreateParameter();
            param.ParameterName = "@rolId";
            param.Value = rolId;
            command.Parameters.Add(param);

            var result = await command.ExecuteScalarAsync();
            var json = result?.ToString();

            return JsonConvert.DeserializeObject<List<MenuItemDto>>(json ?? "[]") ?? new();
        }

    }
}
