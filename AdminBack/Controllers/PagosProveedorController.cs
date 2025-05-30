using AdminBack.Models.DTOs;
using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador,AnalistaContable")]
    public class PagosProveedorController : ControllerBase
    {
        private readonly IPagoProveedorService _service;

        public PagosProveedorController(IPagoProveedorService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] PagoProveedorDto dto)
        {
            var registrado = await _service.RegistrarPago(dto);
            return registrado
                ? Ok(ResponseHelper.Success<object>(null, "Pago registrado"))
                : BadRequest(ResponseHelper.Fail<object>("Error al registrar pago"));
        }

        [HttpGet("orden/{ordenId}")]
        public async Task<IActionResult> ObtenerPorOrden(int ordenId)
        {
            var data = await _service.ObtenerPorOrden(ordenId);
            return Ok(ResponseHelper.Success(data));
        }

        [HttpGet("proveedor/{proveedorId}")]
        public async Task<IActionResult> ObtenerPorProveedor(int proveedorId)
        {
            var data = await _service.ObtenerPorProveedor(proveedorId);
            return Ok(ResponseHelper.Success(data));
        }

        [HttpGet("orden/{ordenId}/estado")]
        public async Task<IActionResult> EstadoPorOrden(int ordenId)
        {
            try
            {
                var estado = await _service.ObtenerEstadoPagoPorOrden(ordenId);
                return Ok(ResponseHelper.Success(estado));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.Fail<object>(ex.Message));
            }
        }

    }
}
