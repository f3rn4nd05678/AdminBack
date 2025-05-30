using AdminBack.Models.DTOs;
using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador,AnalistaContable,GestorVentas")]
    public class GestionFacturasController : ControllerBase
    {
        private readonly INotaCreditoService _notaService;

        public GestionFacturasController(INotaCreditoService notaService)
        {
            _notaService = notaService;
        }

        // POST: api/GestionFacturas/notas-credito
        [HttpPost("notas-credito")]
        public async Task<IActionResult> RegistrarNota([FromBody] NotaCreditoDto dto)
        {
            var ok = await _notaService.Registrar(dto);
            return ok
                ? Ok(ResponseHelper.Success<object>(null, "Nota registrada"))
                : BadRequest(ResponseHelper.Fail<object>("Error al registrar nota"));
        }

        // GET: api/GestionFacturas/notas-credito/{pedidoId}
        [HttpGet("notas-credito/{pedidoId}")]
        public async Task<IActionResult> ObtenerNotas(int pedidoId)
        {
            var notas = await _notaService.ObtenerPorPedido(pedidoId);
            return Ok(ResponseHelper.Success(notas));
        }


    }
}
