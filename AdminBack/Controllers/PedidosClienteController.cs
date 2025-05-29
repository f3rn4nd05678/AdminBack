using AdminBack.Models.DTOs;
using AdminBack.Models.DTOs.PedidoCliente;
using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador,GestorVentas")]
    public class PedidosClienteController : ControllerBase
    {
        private readonly IPedidoClienteService _service;

        public PedidosClienteController(IPedidoClienteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var pedidos = await _service.ObtenerTodos();
            return Ok(ResponseHelper.Success(pedidos));
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] PedidoClienteCreateDto dto, [FromQuery] int almacenId)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var creado = await _service.Crear(dto, usuarioId, almacenId);
                return creado
                    ? Ok(ResponseHelper.Success<object>(null, "Pedido registrado"))
                    : BadRequest(ResponseHelper.Fail<object>("No se pudo registrar el pedido"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.Fail<object>(ex.Message));
            }
        }
    }
}
