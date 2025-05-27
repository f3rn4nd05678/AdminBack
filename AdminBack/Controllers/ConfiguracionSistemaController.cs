using AdminBack.Models.DTOs;
using AdminBack.Models.DTOs.Config;
using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class ConfiguracionSistemaController : ControllerBase
    {
        private readonly IConfiguracionService _service;

        public ConfiguracionSistemaController(IConfiguracionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodo()
        {
            var result = await _service.ObtenerTodo();
            return Ok(ResponseHelper.Success(result));
        }

        [HttpGet("{clave}")]
        public async Task<IActionResult> ObtenerPorClave(string clave)
        {
            var config = await _service.ObtenerPorClave(clave);
            if (config == null)
                return NotFound(ResponseHelper.Fail<object>("Clave no encontrada", 404));

            return Ok(ResponseHelper.Success(config));
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] ConfiguracionCreateDto dto)
        {
            var creado = await _service.Crear(dto);
            if (!creado)
                return Conflict(ResponseHelper.Fail<object>("La clave ya existe", 409));

            return Ok(ResponseHelper.Success<object>(null, "Configuración creada"));
        }

        [HttpPut("{clave}")]
        public async Task<IActionResult> Actualizar(string clave, [FromBody] string nuevoValor)
        {
            var actualizado = await _service.Actualizar(clave, nuevoValor);
            if (!actualizado)
                return NotFound(ResponseHelper.Fail<object>("Clave no encontrada", 404));

            return Ok(ResponseHelper.Success<object>(null, "Configuración actualizada"));
        }
    }
}
