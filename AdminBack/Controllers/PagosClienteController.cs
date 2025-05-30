using AdminBack.Models.DTOs;
using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador,GestorVentas")]
    public class PagosClienteController : ControllerBase
    {
        private readonly IPagoClienteService _service;

        public PagosClienteController(IPagoClienteService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] PagoClienteDto dto)
        {
            var registrado = await _service.RegistrarPago(dto);
            return registrado
                ? Ok(ResponseHelper.Success<object>(null, "Pago registrado"))
                : BadRequest(ResponseHelper.Fail<object>("No se pudo registrar el pago"));
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var pagos = await _service.ObtenerTodos();
            return Ok(ResponseHelper.Success(pagos));
        }

        [HttpGet("pedido/{id}")]
        public async Task<IActionResult> ObtenerPorPedido(int id)
        {
            var pagos = await _service.ObtenerPorPedido(id);
            return Ok(ResponseHelper.Success(pagos));
        }

        [HttpGet("cliente/{id}")]
        public async Task<IActionResult> ObtenerPorCliente(int id)
        {
            var pagos = await _service.ObtenerPorCliente(id);
            return Ok(ResponseHelper.Success(pagos));
        }

        [HttpGet("pedido/{id}/estado")]
        public async Task<IActionResult> ObtenerEstado(int id)
        {
            try
            {
                var estado = await _service.ObtenerEstadoPagoPorPedido(id);
                return Ok(ResponseHelper.Success(estado));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.Fail<object>(ex.Message));
            }
        }


    }
}
