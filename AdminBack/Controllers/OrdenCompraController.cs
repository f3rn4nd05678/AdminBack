using AdminBack.Models.DTOs;
using AdminBack.Models.DTOs.OrdenCompra;
using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador,EncargadoCompras")]
    public class OrdenCompraController : ControllerBase
    {
        private readonly IOrdenCompraService _service;

        public OrdenCompraController(IOrdenCompraService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var result = await _service.ObtenerTodas();
            return Ok(ResponseHelper.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] OrdenCompraCreateDto dto)
        {
            var creado = await _service.Crear(dto);
            return creado
                ? Ok(ResponseHelper.Success<object>(null, "Orden creada"))
                : BadRequest(ResponseHelper.Fail<object>("Error al crear orden"));
        }

        [HttpPut("{id}/aprobar")]
        public async Task<IActionResult> Aprobar(int id, [FromBody] OrdenCompraAprobarDto dto)
        {
            var aprobado = await _service.Aprobar(id, dto.AlmacenId);
            return aprobado
                ? Ok(ResponseHelper.Success<object>(null, "Orden aprobada"))
                : BadRequest(ResponseHelper.Fail<object>("No se pudo aprobar la orden"));
        }

    }
}
