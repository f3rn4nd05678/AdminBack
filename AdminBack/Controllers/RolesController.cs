using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AdminBack.Service.IService;
using AdminBack.Models.DTOs;

namespace AdminBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Todos los endpoints requieren autenticación
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Obtener todos los roles
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleService.GetAllRolesAsync();

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Obtener rol por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var result = await _roleService.GetRoleByIdAsync(id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Crear nuevo rol
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<string>.Fail("Datos inválidos", errors));
            }

            var result = await _roleService.CreateRoleAsync(createRoleDto);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result);
            }

            return CreatedAtAction(nameof(GetRoleById), new { id = result.Data?.Id }, result);
        }

        /// <summary>
        /// Actualizar rol existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<string>.Fail("Datos inválidos", errors));
            }

            var result = await _roleService.UpdateRoleAsync(id, updateRoleDto);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Eliminar rol
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleService.DeleteRoleAsync(id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Obtener usuarios asignados a un rol
        /// </summary>
        [HttpGet("{id}/usuarios")]
        public async Task<IActionResult> GetUsersByRole(int id)
        {
            var result = await _roleService.GetUsersByRoleAsync(id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
    }
}