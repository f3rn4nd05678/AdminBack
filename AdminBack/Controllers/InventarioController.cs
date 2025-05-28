using AdminBack.Models.DTOs;
using AdminBack.Models.DTOs.Inventario;
using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador,ResponsableBodega")]
    public class InventarioController : ControllerBase
    {
        private readonly IInventarioService _service;

        public InventarioController(IInventarioService service)
        {
            _service = service;
        }

        [HttpGet("stock")]
        public async Task<IActionResult> ConsultarStock()
        {
            var stock = await _service.ConsultarStock();
            return Ok(ResponseHelper.Success(stock));
        }

        [HttpGet("entradas")]
        public async Task<IActionResult> ObtenerEntradas()
        {
            var entradas = await _service.ObtenerEntradas();
            return Ok(ResponseHelper.Success(entradas));
        }

        [HttpGet("salidas")]
        public async Task<IActionResult> ObtenerSalidas()
        {
            var salidas = await _service.ObtenerSalidas();
            return Ok(ResponseHelper.Success(salidas));
        }

        [HttpPost("entrada")]
        public async Task<IActionResult> RegistrarEntrada([FromBody] EntradaInventarioCreateDto dto)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var result = await _service.RegistrarEntrada(dto, usuarioId);
            return result
                ? Ok(ResponseHelper.Success<object>(null, "Entrada registrada"))
                : BadRequest(ResponseHelper.Fail<object>("No se pudo registrar la entrada"));
        }

        [HttpPost("salida")]
        public async Task<IActionResult> RegistrarSalida([FromBody] SalidaInventarioCreateDto dto)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var result = await _service.RegistrarSalida(dto, usuarioId);
            return result
                ? Ok(ResponseHelper.Success<object>(null, "Salida registrada"))
                : BadRequest(ResponseHelper.Fail<object>("Stock insuficiente o datos inválidos"));
        }
    }
}
