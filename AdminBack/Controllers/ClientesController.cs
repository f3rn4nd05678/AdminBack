using AdminBack.Models.DTOs;
using AdminBack.Models.DTOs.Cliente;
using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador,GestorVentas")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _service;

        public ClientesController(IClienteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await _service.ObtenerTodos();
            return Ok(ResponseHelper.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] ClienteCreateDto dto)
        {
            var creado = await _service.Crear(dto);
            return creado
                ? Ok(ResponseHelper.Success<object>(null, "Cliente creado"))
                : BadRequest(ResponseHelper.Fail<object>("No se pudo crear el cliente"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ClienteUpdateDto dto)
        {
            var actualizado = await _service.Actualizar(id, dto);
            return actualizado
                ? Ok(ResponseHelper.Success<object>(null, "Cliente actualizado"))
                : NotFound(ResponseHelper.Fail<object>("Cliente no encontrado"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var desactivado = await _service.Desactivar(id);
            return desactivado
                ? Ok(ResponseHelper.Success<object>(null, "Cliente desactivado"))
                : NotFound(ResponseHelper.Fail<object>("Cliente no encontrado"));
        }
    }
}
