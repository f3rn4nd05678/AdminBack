using AdminBack.Models.DTOs;
using AdminBack.Models.DTOs.Producto;
using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador,ResponsableBodega")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _service;

        public ProductosController(IProductoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var productos = await _service.ObtenerTodos();
            return Ok(ResponseHelper.Success(productos));
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] ProductoCreateDto dto)
        {
            var creado = await _service.Crear(dto);
            if (!creado)
                return Conflict(ResponseHelper.Fail<object>("Código de producto duplicado"));

            return Ok(ResponseHelper.Success<object>(null, "Producto creado"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ProductoUpdateDto dto)
        {
            var actualizado = await _service.Actualizar(id, dto);
            if (!actualizado)
                return NotFound(ResponseHelper.Fail<object>("Producto no encontrado", 404));

            return Ok(ResponseHelper.Success<object>(null, "Producto actualizado"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var desactivado = await _service.Desactivar(id);
            if (!desactivado)
                return NotFound(ResponseHelper.Fail<object>("Producto no encontrado", 404));

            return Ok(ResponseHelper.Success<object>(null, "Producto desactivado"));
        }

    }
}
