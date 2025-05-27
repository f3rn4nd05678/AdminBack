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
    public class EntradasInventarioController : ControllerBase
    {
        private readonly IEntradaInventarioService _service;

        public EntradasInventarioController(IEntradaInventarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var entradas = await _service.ObtenerTodas();
            return Ok(ResponseHelper.Success(entradas));
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] EntradaInventarioCreateDto dto)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var registrado = await _service.Registrar(dto, usuarioId);
            if (!registrado)
                return BadRequest(ResponseHelper.Fail<object>("No se pudo registrar la entrada"));

            return Ok(ResponseHelper.Success<object>(null, "Entrada registrada correctamente"));
        }
    }
}
