using AdminBack.Data;
using AdminBack.Models;
using AdminBack.Models.DTOs.Almacen;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace AdminBack.Service
{
    public class AlmacenService : IAlmacenService
    {
        private readonly AdminDbContext _context;

        public AlmacenService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<AlmacenDto>> ObtenerTodos()
        {
            return await _context.Almacenes
                .Where(a => a.Activo ?? true)
                .Select(a => new AlmacenDto
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    Ubicacion = a.Ubicacion,
                    Descripcion = a.Descripcion,
                    Activo = a.Activo ?? true
                })
                .ToListAsync();
        }

        public async Task<bool> Crear(AlmacenCreateDto dto)
        {
            _context.Almacenes.Add(new Almacene
            {
                Nombre = dto.Nombre,
                Ubicacion = dto.Ubicacion,
                Descripcion = dto.Descripcion,
                Activo = true
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Actualizar(int id, AlmacenUpdateDto dto)
        {
            var almacen = await _context.Almacenes.FindAsync(id);
            if (almacen == null) return false;

            almacen.Nombre = dto.Nombre;
            almacen.Ubicacion = dto.Ubicacion;
            almacen.Descripcion = dto.Descripcion;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Desactivar(int id)
        {
            var almacen = await _context.Almacenes.FindAsync(id);
            if (almacen == null) return false;

            almacen.Activo = false;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
