using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador,GestorVentas")]
    public class FacturaVentaController : ControllerBase
    {
        private readonly IFacturaVentaService _service;

        public FacturaVentaController(IFacturaVentaService service)
        {
            _service = service;
        }

        [HttpGet("{pedidoId}/json")]
        public async Task<IActionResult> ObtenerJson(int pedidoId)
        {
            try
            {
                var json = await _service.GenerarFacturaJson(pedidoId);
                return Ok(ResponseHelper.Success(json, "Factura generada"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.Fail<object>(ex.Message));
            }
        }

        [HttpGet("{pedidoId}/mongo")]
        public async Task<IActionResult> ObtenerDesdeMongo(int pedidoId)
        {
            try
            {
                var json = await _service.ObtenerFacturaDesdeMongo(pedidoId);
                return Ok(ResponseHelper.Success(json, "Factura desde MongoDB"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.Fail<object>(ex.Message));
            }
        }

    }
}
