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
    public class EntregasController : ControllerBase
    {
        private readonly IEntregaService _service;

        public EntregasController(IEntregaService service)
        {
            _service = service;
        }

        [HttpPost("asignar")]
        public async Task<IActionResult> Asignar([FromBody] EntregaAsignarDto dto)
        {
            var ok = await _service.Asignar(dto);
            return ok
                ? Ok(ResponseHelper.Success<object>(null, "Entrega asignada"))
                : BadRequest(ResponseHelper.Fail<object>("Error al asignar entrega"));
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] string nuevoEstado)
        {
            var ok = await _service.CambiarEstado(id, nuevoEstado);
            return ok
                ? Ok(ResponseHelper.Success<object>(null, "Estado actualizado"))
                : BadRequest(ResponseHelper.Fail<object>("Error al cambiar estado"));
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var data = await _service.ObtenerTodas();
            return Ok(ResponseHelper.Success(data));
        }

        [HttpGet("pedido/{pedidoId}")]
        public async Task<IActionResult> ObtenerPorPedido(int pedidoId)
        {
            var data = await _service.ObtenerPorPedido(pedidoId);
            return Ok(ResponseHelper.Success(data));
        }
    }
}
