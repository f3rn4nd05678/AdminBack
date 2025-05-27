using AdminBack.Data;
using AdminBack.Models;
using AdminBack.Models.DTOs;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace AdminBack.Service
{
    public class RoleService : IRoleService
    {
        private readonly AdminDbContext _context;

        public RoleService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<Role>>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _context.Roles
                    .Include(r => r.Usuarios)
                    .ToListAsync();

                return ApiResponse<List<Role>>.Success(roles);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<Role>>.Fail($"Error interno: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<Role>> GetRoleByIdAsync(int id)
        {
            try
            {
                var role = await _context.Roles
                    .Include(r => r.Usuarios)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (role == null)
                {
                    return ApiResponse<Role>.Fail("Rol no encontrado", null, 404);
                }

                return ApiResponse<Role>.Success(role);
            }
            catch (Exception ex)
            {
                return ApiResponse<Role>.Fail($"Error interno: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<Role>> CreateRoleAsync(Role role)
        {
            try
            {
                var existeNombre = await _context.Roles.AnyAsync(r => r.Nombre.ToLower() == role.Nombre.ToLower());
                if (existeNombre)
                {
                    return ApiResponse<Role>.Fail("Ya existe un rol con ese nombre");
                }

                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                return ApiResponse<Role>.Success(role, "Rol creado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<Role>.Fail($"Error interno: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<Role>> UpdateRoleAsync(int id, Role roleUpdate)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                {
                    return ApiResponse<Role>.Fail("Rol no encontrado", null, 404);
                }

                // Verificar si el nombre ya existe en otro rol
                var existeNombre = await _context.Roles
                    .AnyAsync(r => r.Id != id && r.Nombre.ToLower() == roleUpdate.Nombre.ToLower());

                if (existeNombre)
                {
                    return ApiResponse<Role>.Fail("Ya existe otro rol con ese nombre");
                }

                role.Nombre = roleUpdate.Nombre;
                role.Descripcion = roleUpdate.Descripcion;

                await _context.SaveChangesAsync();

                return ApiResponse<Role>.Success(role, "Rol actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<Role>.Fail($"Error interno: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteRoleAsync(int id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                {
                    return ApiResponse<bool>.Fail("Rol no encontrado", null, 404);
                }

                // Verificar si hay usuarios asignados a este rol
                var tieneUsuarios = await _context.Usuarios.AnyAsync(u => u.RolId == id);
                if (tieneUsuarios)
                {
                    return ApiResponse<bool>.Fail("No se puede eliminar el rol porque tiene usuarios asignados");
                }

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Success(true, "Rol eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error interno: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<List<Usuario>>> GetUsersByRoleAsync(int roleId)
        {
            try
            {
                var roleExists = await _context.Roles.AnyAsync(r => r.Id == roleId);
                if (!roleExists)
                {
                    return ApiResponse<List<Usuario>>.Fail("Rol no encontrado", null, 404);
                }

                var usuarios = await _context.Usuarios
                    .Where(u => u.RolId == roleId)
                    .Include(u => u.Rol)
                    .ToListAsync();

                return ApiResponse<List<Usuario>>.Success(usuarios);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<Usuario>>.Fail($"Error interno: {ex.Message}", null, 500);
            }
        }
    }
}