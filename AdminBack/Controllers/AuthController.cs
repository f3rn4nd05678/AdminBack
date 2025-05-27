using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AdminBack.Service.IService;
using AdminBack.Models.DTOs;
using System.Security.Claims;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Iniciar sesión
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<string>.Fail("Datos inválidos", errors));
            }

            var result = await _authService.LoginAsync(loginDto);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Registrar nuevo usuario
        /// </summary>
        [HttpPost("register")]
        [Authorize] // Solo usuarios autenticados pueden registrar otros usuarios
        public async Task<IActionResult> Register([FromBody] UsuarioRegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<string>.Fail("Datos inválidos", errors));
            }

            var result = await _authService.RegisterAsync(registerDto);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Cambiar contraseña del usuario actual
        /// </summary>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<string>.Fail("Datos inválidos", errors));
            }

            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (usuarioId == 0)
            {
                return Unauthorized(ApiResponse<string>.Fail("Usuario no válido"));
            }

            var result = await _authService.ChangePasswordAsync(usuarioId, changePasswordDto);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Obtener información del usuario actual
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var userInfo = new
            {
                Id = userId,
                NombreCompleto = userName,
                Email = userEmail,
                Rol = userRole
            };

            return Ok(ApiResponse<object>.Success(userInfo));
        }

        /// <summary>
        /// Verificar si el token es válido
        /// </summary>
        [HttpGet("verify-token")]
        [Authorize]
        public IActionResult VerifyToken()
        {
            return Ok(ApiResponse<string>.Success("Token válido"));
        }
    }
}