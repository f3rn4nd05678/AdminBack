using AdminBack.Data;
using AdminBack.Models.DTOs;
using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AdminDbContext _context;
        private readonly IAuthService _authService;

        public AuthController(AdminDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            try
            {
                var user = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Email == login.Email);

                if (user == null || string.IsNullOrEmpty(user.Contrasena) || !PasswordHelper.VerifyPassword(login.Contrasena, user.Contrasena))
                {
                    return Unauthorized(ResponseHelper.Fail<object>("Credenciales inválidas", 401));
                }

                var token = _authService.GenerateToken(user);

                var result = new
                {
                    Token = token,
                    Usuario = new
                    {
                        user.Id,
                        user.NombreCompleto,
                        user.Email,
                        Rol = user.Rol?.Nombre
                    }
                };

                return Ok(ResponseHelper.Success(result));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message} \nSTACK: {ex.StackTrace}");
                return StatusCode(500, ResponseHelper.Fail<object>("Error interno en el servidor: " + ex.Message, 500));
            }
        }



    }
}
