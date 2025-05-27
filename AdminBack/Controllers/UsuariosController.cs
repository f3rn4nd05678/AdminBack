using AdminBack.Models.DTOs;
using AdminBack.Models.DTOs.Usuario;
using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var usuarios = await _usuarioService.ObtenerTodos();
            return Ok(ResponseHelper.Success(usuarios));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var usuario = await _usuarioService.ObtenerPorId(id);
            if (usuario == null)
                return NotFound(ResponseHelper.Fail<UsuarioDto>("Usuario no encontrado", 404));

            return Ok(ResponseHelper.Success(usuario));
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] UsuarioCreateDto dto)
        {
            var creado = await _usuarioService.Crear(dto);
            if (!creado)
                return BadRequest(ResponseHelper.Fail<object>("No se pudo crear el usuario, email ya existe", 400));

            return Ok(ResponseHelper.Success<object>(null, "Usuario creado exitosamente"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var desactivado = await _usuarioService.Desactivar(id);
            if (!desactivado)
                return BadRequest(ResponseHelper.Fail<object>("No se pudo desactivar el usuario", 400));

            return Ok(ResponseHelper.Success<object>(null, "Usuario desactivado"));
        }
    }
}
