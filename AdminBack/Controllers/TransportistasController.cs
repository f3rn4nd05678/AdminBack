using AdminBack.Models.DTOs;
using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador,CoordinadorLogistica")]
    public class TransportistasController : ControllerBase
    {
        private readonly ITransportistaService _service;

        public TransportistasController(ITransportistaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _service.ObtenerTodos();
            return Ok(ResponseHelper.Success(data));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.ObtenerPorId(id);
            return data != null
                ? Ok(ResponseHelper.Success(data))
                : NotFound(ResponseHelper.Fail<object>("No encontrado"));
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] TransportistaCreateDto dto)
        {
            var ok = await _service.Crear(dto);
            return ok
                ? Ok(ResponseHelper.Success<object>(null, "Transportista creado"))
                : BadRequest(ResponseHelper.Fail<object>("Error al crear"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] TransportistaCreateDto dto)
        {
            var ok = await _service.Actualizar(id, dto);
            return ok
                ? Ok(ResponseHelper.Success<object>(null, "Transportista actualizado"))
                : BadRequest(ResponseHelper.Fail<object>("Error al actualizar"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var ok = await _service.Desactivar(id);
            return ok
                ? Ok(ResponseHelper.Success<object>(null, "Transportista desactivado"))
                : BadRequest(ResponseHelper.Fail<object>("Error al desactivar"));
        }
    }
}
