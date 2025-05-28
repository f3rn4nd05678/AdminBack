using AdminBack.Models.DTOs;
using AdminBack.Models.DTOs.Almacen;
using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador,ResponsableBodega")]
    public class AlmacenesController : ControllerBase
    {
        private readonly IAlmacenService _service;

        public AlmacenesController(IAlmacenService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var almacenes = await _service.ObtenerTodos();
            return Ok(ResponseHelper.Success(almacenes));
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] AlmacenCreateDto dto)
        {
            var creado = await _service.Crear(dto);
            return creado
                ? Ok(ResponseHelper.Success<object>(null, "Almacén creado correctamente"))
                : BadRequest(ResponseHelper.Fail<object>("No se pudo crear el almacén"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] AlmacenUpdateDto dto)
        {
            var actualizado = await _service.Actualizar(id, dto);
            return actualizado
                ? Ok(ResponseHelper.Success<object>(null, "Almacén actualizado"))
                : NotFound(ResponseHelper.Fail<object>("Almacén no encontrado", 404));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var desactivado = await _service.Desactivar(id);
            return desactivado
                ? Ok(ResponseHelper.Success<object>(null, "Almacén desactivado"))
                : NotFound(ResponseHelper.Fail<object>("Almacén no encontrado", 404));
        }
    }
}
