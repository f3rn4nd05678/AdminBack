using AdminBack.Data;
using AdminBack.Models;
using AdminBack.Models.DTOs.Inventario;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace AdminBack.Service
{
    public class EntradaInventarioService : IEntradaInventarioService
    {
        private readonly AdminDbContext _context;

        public EntradaInventarioService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<EntradaInventarioDto>> ObtenerTodas()
        {
            return await _context.EntradasInventarios
                .Include(e => e.Producto)
                .Include(e => e.Almacen)
                .Include(e => e.Usuario)
                .OrderByDescending(e => e.FechaEntrada)
                .Select(e => new EntradaInventarioDto
                {
                    Id = e.Id,
                    Producto = e.Producto.Nombre,
                    Almacen = e.Almacen.Nombre,
                    Cantidad = e.Cantidad,
                    FechaEntrada = e.FechaEntrada ?? DateTime.MinValue,
                    Referencia = e.Referencia,
                    Usuario = e.Usuario != null ? e.Usuario.NombreCompleto : "Sistema"
                })
                .ToListAsync();
        }

        public async Task<bool> Registrar(EntradaInventarioCreateDto dto, int usuarioId)
        {
            var entrada = new EntradasInventario
            {
                ProductoId = dto.ProductoId,
                AlmacenId = dto.AlmacenId,
                Cantidad = dto.Cantidad,
                FechaEntrada = DateTime.UtcNow,
                Referencia = dto.Referencia,
                UsuarioId = usuarioId
            };

            _context.EntradasInventarios.Add(entrada);

            var inventario = await _context.InventarioActuals.FirstOrDefaultAsync(i =>
                i.ProductoId == dto.ProductoId && i.AlmacenId == dto.AlmacenId);

            if (inventario != null)
            {
                inventario.Cantidad += dto.Cantidad;
            }
            else
            {
                _context.InventarioActuals.Add(new InventarioActual
                {
                    ProductoId = dto.ProductoId,
                    AlmacenId = dto.AlmacenId,
                    Cantidad = dto.Cantidad
                });
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
