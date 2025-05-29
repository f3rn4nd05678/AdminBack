using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Mvc;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresExternosController : ControllerBase
    {
        private readonly IProveedorMongoService _mongoService;

        public ProveedoresExternosController(IProveedorMongoService mongoService)
        {
            _mongoService = mongoService;
        }

        [HttpPost("guardar-catalogo")]
        public async Task<IActionResult> GuardarCatalogoSimulado()
        {
            var respuestaSimulada = new
            {
                productos = new[]
                {
                    new { codigo = "ABC123", nombre = "Producto A", stock = 100, precio = 50.5 },
                    new { codigo = "XYZ999", nombre = "Producto B", stock = 10, precio = 300.0 }
                },
                origen = "Proveedor Simulado"
            };

            await _mongoService.GuardarCatalogo("Proveedor 1", respuestaSimulada);

            return Ok("Catálogo simulado guardado en Mongo.");
        }

        [HttpGet("{id}/catalogo")]
        public async Task<IActionResult> ObtenerCatalogo(int id, [FromServices] IProveedorHttpService httpService)
        {
            try
            {
                var productos = await httpService.ObtenerYGuardarCatalogo(id);
                return Ok(ResponseHelper.Success(productos, "Catálogo obtenido y guardado"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.Fail<object>($"Error: {ex.Message}"));
            }
        }

        [HttpGet("{id}/track")]
        public async Task<IActionResult> Track(int id, [FromQuery] int ordenId, [FromServices] IProveedorHttpService httpService)
        {
            try
            {
                var result = await httpService.ConsultarTracking(id, ordenId);
                return Ok(ResponseHelper.Success(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseHelper.Fail<object>(ex.Message));
            }
        }

    }
}
