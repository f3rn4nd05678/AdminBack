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
        public async Task<IActionResult> GetResumen([FromQuery] string inicio, [FromQuery] string fin)
        {
            if (!DateTime.TryParse(inicio, out var fechaInicio) || !DateTime.TryParse(fin, out var fechaFin))
                return BadRequest(ResponseHelper.Fail<object>("Formato de fecha inválido"));


            fechaInicio = DateTime.SpecifyKind(fechaInicio, DateTimeKind.Utc);
            fechaFin = DateTime.SpecifyKind(fechaFin, DateTimeKind.Utc);

            var data = await _service.ObtenerResumenFinanciero(fechaInicio, fechaFin);
            return Ok(ResponseHelper.Success(data));
        }



        [HttpGet("dashboard/detallado")]
        public async Task<IActionResult> GetDetallado([FromQuery] string inicio, [FromQuery] string fin)
        {
            if (!DateTime.TryParse(inicio, out var fechaInicio) || !DateTime.TryParse(fin, out var fechaFin))
                return BadRequest(ResponseHelper.Fail<object>("Formato de fecha inválido"));

            fechaInicio = DateTime.SpecifyKind(fechaInicio, DateTimeKind.Utc);
            fechaFin = DateTime.SpecifyKind(fechaFin, DateTimeKind.Utc);

            var data = await _service.ObtenerResumenDetallado(fechaInicio, fechaFin);
            return Ok(ResponseHelper.Success(data));
        }


        [HttpGet("dashboard/proveedores")]
        public async Task<IActionResult> GetProveedores([FromQuery] string inicio, [FromQuery] string fin)
        {
            if (!DateTime.TryParse(inicio, out var fechaInicio) || !DateTime.TryParse(fin, out var fechaFin))
                return BadRequest(ResponseHelper.Fail<object>("Formato de fecha inválido"));

            fechaInicio = DateTime.SpecifyKind(fechaInicio, DateTimeKind.Utc);
            fechaFin = DateTime.SpecifyKind(fechaFin, DateTimeKind.Utc);

            var data = await _service.ObtenerResumenPorProveedor(fechaInicio, fechaFin);
            return Ok(ResponseHelper.Success(data));
        }


    }
}
