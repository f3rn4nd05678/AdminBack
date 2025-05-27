using AdminBack.Data;
using AdminBack.Models;
using AdminBack.Models.DTOs;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AdminBack.Service
{
    public class UserService : IUserService
    {
        private readonly AdminDbContext _context;

        public UserService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<Usuario>>> GetAllUsersAsync()
        {
            try
            {
                var usuarios = await _context.Usuarios
                    .Include(u => u.Rol)
                    .ToListAsync();

                return ApiResponse<List<Usuario>>.Success(usuarios);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<Usuario>>.Fail($"Error interno: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<Usuario>> GetUserByIdAsync(int id)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (usuario == null)
                {
                    return ApiResponse<Usuario>.Fail("Usuario no encontrado", null, 404);
                }

                return ApiResponse<Usuario>.Success(usuario);
            }
            catch (Exception ex)
            {
                return ApiResponse<Usuario>.Fail($"Error interno: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<Usuario>> UpdateUserAsync(int id, Usuario usuarioUpdate)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return ApiResponse<Usuario>.Fail("Usuario no encontrado", null, 404);
                }

                // Verificar si el email ya existe en otro usuario
                var existeEmail = await _context.Usuarios
                    .AnyAsync(u => u.Id != id && u.Email == usuarioUpdate.Email);

                if (existeEmail)
                {
                    return ApiResponse<Usuario>.Fail("El correo electrónico ya está en uso por otro usuario");
                }

                // Verificar que el rol existe si se proporciona
                if (usuarioUpdate.RolId.HasValue)
                {
                    var rolExiste = await _context.Roles.AnyAsync(r => r.Id == usuarioUpdate.RolId.Value);
                    if (!rolExiste)
                    {
                        return ApiResponse<Usuario>.Fail("El rol especificado no existe");
                    }
                }

                usuario.NombreCompleto = usuarioUpdate.NombreCompleto;
                usuario.Email = usuarioUpdate.Email;
                usuario.RolId = usuarioUpdate.RolId;

                await _context.SaveChangesAsync();

                // Cargar el rol para la respuesta
                await _context.Entry(usuario)
                    .Reference(u => u.Rol)
                    .LoadAsync();

                return ApiResponse<Usuario>.Success(usuario, "Usuario actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<Usuario>.Fail($"Error interno: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<bool>> ToggleUserStatusAsync(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return ApiResponse<bool>.Fail("Usuario no encontrado", null, 404);
                }

                usuario.Activo = !usuario.Activo;
                await _context.SaveChangesAsync();

                var mensaje = usuario.Activo == true ? "Usuario activado" : "Usuario desactivado";
                return ApiResponse<bool>.Success(true, mensaje);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error interno: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteUserAsync(int id)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return ApiResponse<bool>.Fail("Usuario no encontrado", null, 404);
                }

                // Verificar si el usuario tiene registros de movimientos de inventario
                var tieneMovimientos = await _context.EntradasInventarios.AnyAsync(e => e.UsuarioId == id) ||
                                     await _context.SalidasInventarios.AnyAsync(s => s.UsuarioId == id);

                if (tieneMovimientos)
                {
                    return ApiResponse<bool>.Fail("No se puede eliminar el usuario porque tiene movimientos de inventario asociados");
                }

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Success(true, "Usuario eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error interno: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<bool>> ResetPasswordAsync(int id, string nuevaContrasena)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return ApiResponse<bool>.Fail("Usuario no encontrado", null, 404);
                }

                usuario.Contrasena = HashPassword(nuevaContrasena);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.Success(true, "Contraseña restablecida exitosamente");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error interno: {ex.Message}", null, 500);
            }
        }

        public async Task<ApiResponse<List<Usuario>>> SearchUsersAsync(string searchTerm)
        {
            try
            {
                var usuarios = await _context.Usuarios
                    .Include(u => u.Rol)
                    .Where(u => u.NombreCompleto.Contains(searchTerm) || u.Email.Contains(searchTerm))
                    .ToListAsync();

                return ApiResponse<List<Usuario>>.Success(usuarios);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<Usuario>>.Fail($"Error interno: {ex.Message}", null, 500);
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}