using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdminBack.Data;
using AdminBack.Models;
using AdminBack.Models.DTOs;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AdminDbContext _context;

        public UsuariosController(AdminDbContext context)
        {
            _context = context;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioRegisterDto dto)
        {
            var existe = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
            if (existe)
            {
                return BadRequest(ApiResponse<string>.Fail("El correo ya está registrado", new List<string> { dto.Email }));
            }

            var nuevoUsuario = new Usuario
            {
                NombreCompleto = dto.NombreCompleto,
                Email = dto.Email,
                Contrasena = dto.Contrasena,
                RolId = dto.RolId
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return Ok(ApiResponse<object>.Success(new { id = nuevoUsuario.Id }, "Usuario registrado correctamente"));
        }

        [HttpGet("conexion")]
        public async Task<IActionResult> VerificarConexion()
        {
            var puedeConectar = await _context.Database.CanConnectAsync();
            if (puedeConectar)
                return Ok(ApiResponse<string>.Success("Conexión exitosa"));
            else
                return StatusCode(500, ApiResponse<string>.Fail("Error al conectar con la base de datos"));
        }
    }
}
