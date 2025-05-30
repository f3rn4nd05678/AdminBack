using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador,AnalistaContable")]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteService _service;

        public ReportesController(IReporteService service)
        {
            _service = service;
        }

        [HttpGet("ventas")]
        public async Task<IActionResult> ReporteVentas([FromQuery] DateTime inicio, [FromQuery] DateTime fin)
        {
            var data = await _service.ObtenerReporteVentas(inicio, fin);
            return Ok(ResponseHelper.Success(data));
        }
    }
}
