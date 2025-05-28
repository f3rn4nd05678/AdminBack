using AdminBack.Data;
using AdminBack.Models;
using AdminBack.Models.DTOs.Producto;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;
using System;

namespace AdminBack.Service
{
    public class ProductoService : IProductoService
    {
        private readonly AdminDbContext _context;

        public ProductoService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductoDto>> ObtenerTodos()
        {
            return await _context.Productos
                .Where(p => p.Activo ?? true)
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Codigo = p.Codigo,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    UnidadMedida = p.UnidadMedida,
                    PrecioUnitario = p.PrecioUnitario ?? 0,
                    Activo = p.Activo ?? true
                })
                .ToListAsync();
        }

        public async Task<bool> Crear(ProductoCreateDto dto)
        {
            var existe = await _context.Productos.AnyAsync(p => p.Codigo == dto.Codigo);
            if (existe) return false;

            _context.Productos.Add(new Producto
            {
                Codigo = dto.Codigo,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                UnidadMedida = dto.UnidadMedida,
                PrecioUnitario = dto.PrecioUnitario,
                Activo = true
            });

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Actualizar(int id, ProductoUpdateDto dto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;

            producto.Nombre = dto.Nombre;
            producto.Descripcion = dto.Descripcion;
            producto.UnidadMedida = dto.UnidadMedida;
            producto.PrecioUnitario = dto.PrecioUnitario;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Desactivar(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;

            producto.Activo = false;
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
