using AdminBack.Data;
using AdminBack.Models;
using AdminBack.Models.DTOs.Usuario;
using AdminBack.Service.IService;
using AdminBack.Utils;
using Microsoft.EntityFrameworkCore;

namespace AdminBack.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AdminDbContext _context;

        public UsuarioService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<UsuarioDto>> ObtenerTodos()
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    NombreCompleto = u.NombreCompleto,
                    Email = u.Email,
                    Rol = u.Rol != null ? u.Rol.Nombre : "Sin rol",
                    Activo = u.Activo ?? false
                }).ToListAsync();
        }

        public async Task<UsuarioDto?> ObtenerPorId(int id)
        {
            var u = await _context.Usuarios.Include(r => r.Rol).FirstOrDefaultAsync(x => x.Id == id);
            if (u == null) return null;

            return new UsuarioDto
            {
                Id = u.Id,
                NombreCompleto = u.NombreCompleto,
                Email = u.Email,
                Rol = u.Rol?.Nombre ?? "Sin rol",
                Activo = u.Activo ?? false

            };
        }

        public async Task<bool> Crear(UsuarioCreateDto dto)
        {
            var existe = await _context.Usuarios.AnyAsync(x => x.Email == dto.Email);
            if (existe) return false;

            var nuevo = new Usuario
            {
                NombreCompleto = dto.NombreCompleto,
                Email = dto.Email,
                Contrasena = PasswordHelper.HashPassword(dto.Contrasena),
                RolId = dto.RolId
            };

            _context.Usuarios.Add(nuevo);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Desactivar(int id)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null) return false;

            user.Activo = false;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
