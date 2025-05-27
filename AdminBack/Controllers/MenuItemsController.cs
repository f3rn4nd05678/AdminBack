using AdminBack.Data;
using AdminBack.Models.DTOs;
using AdminBack.Models.DTOs.Menu;
using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly AdminDbContext _context;

        public MenuItemsController(IMenuService menuService, AdminDbContext context)
        {
            _menuService = menuService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMenu()
        {
            var usuarioEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(usuarioEmail))
                return Unauthorized(ResponseHelper.Fail<object>("Usuario no autenticado", 401));

            var usuario = _context.Usuarios.Include(u => u.Rol).FirstOrDefault(u => u.Email == usuarioEmail);

            if (usuario == null || usuario.RolId == null)
                return Forbid();

            var menu = await _menuService.ObtenerMenuPorRol(usuario.RolId.Value);
            return Ok(ResponseHelper.Success(menu, "Menú generado por rol"));
        }
    }
}
