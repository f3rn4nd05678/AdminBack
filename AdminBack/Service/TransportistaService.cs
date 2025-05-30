using AdminBack.Data;
using AdminBack.Models.DTOs;
using AdminBack.Models;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace AdminBack.Service
{
    public class TransportistaService : ITransportistaService
    {
        private readonly AdminDbContext _context;

        public TransportistaService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<TransportistaDto>> ObtenerTodos()
        {
            return await _context.Transportistas
                .Select(t => new TransportistaDto
                {
                    Id = t.Id,
                    Nombre = t.Nombre,
                    Telefono = t.Telefono,
                    Activo = t.Activo
                }).ToListAsync();
        }

        public async Task<TransportistaDto?> ObtenerPorId(int id)
        {
            var t = await _context.Transportistas.FindAsync(id);
            if (t == null) return null;

            return new TransportistaDto
            {
                Id = t.Id,
                Nombre = t.Nombre,
                Telefono = t.Telefono,
                Activo = t.Activo
            };
        }

        public async Task<bool> Crear(TransportistaCreateDto dto)
        {
            var t = new Transportista
            {
                Nombre = dto.Nombre,
                Telefono = dto.Telefono
            };

            _context.Transportistas.Add(t);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Actualizar(int id, TransportistaCreateDto dto)
        {
            var t = await _context.Transportistas.FindAsync(id);
            if (t == null) return false;

            t.Nombre = dto.Nombre;
            t.Telefono = dto.Telefono;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Desactivar(int id)
        {
            var t = await _context.Transportistas.FindAsync(id);
            if (t == null) return false;

            t.Activo = false;
            return await _context.SaveChangesAsync() > 0;
        }
    }

}
