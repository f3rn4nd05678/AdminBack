using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador,AnalistaContable")]
    public class FinanzasController : ControllerBase
    {
        private readonly IFinanzasService _service;

        public FinanzasController(IFinanzasService service)
        {
            _service = service;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetResumen([FromQuery] DateTime inicio, [FromQuery] DateTime fin)
        {
            var data = await _service.ObtenerResumenFinanciero(inicio, fin);
            return Ok(ResponseHelper.Success(data));
        }

        [HttpGet("dashboard/detallado")]
        public async Task<IActionResult> GetDetallado([FromQuery] DateTime inicio, [FromQuery] DateTime fin)
        {
            var data = await _service.ObtenerResumenDetallado(inicio, fin);
            return Ok(ResponseHelper.Success(data));
        }

        [HttpGet("dashboard/proveedores")]
        public async Task<IActionResult> GetProveedores([FromQuery] DateTime inicio, [FromQuery] DateTime fin)
        {
            var data = await _service.ObtenerResumenPorProveedor(inicio, fin);
            return Ok(ResponseHelper.Success(data));
        }

    }
}
