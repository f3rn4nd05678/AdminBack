using AdminBack.Models.DTOs;
using AdminBack.Models.DTOs.Proveedor;
using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador,EncargadoCompras")]
    public class ProveedoresController : ControllerBase
    {
        private readonly IProveedorService _service;

        public ProveedoresController(IProveedorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await _service.ObtenerTodos();
            return Ok(ResponseHelper.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] ProveedorCreateDto dto)
        {
            var creado = await _service.Crear(dto);
            return creado
                ? Ok(ResponseHelper.Success<object>(null, "Proveedor creado"))
                : BadRequest(ResponseHelper.Fail<object>("No se pudo crear el proveedor"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ProveedorUpdateDto dto)
        {
            var actualizado = await _service.Actualizar(id, dto);
            return actualizado
                ? Ok(ResponseHelper.Success<object>(null, "Proveedor actualizado"))
                : NotFound(ResponseHelper.Fail<object>("Proveedor no encontrado"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var desactivado = await _service.Desactivar(id);
            return desactivado
                ? Ok(ResponseHelper.Success<object>(null, "Proveedor desactivado"))
                : NotFound(ResponseHelper.Fail<object>("Proveedor no encontrado"));
        }
    }
}
