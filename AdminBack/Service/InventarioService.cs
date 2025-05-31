using AdminBack.Data;
using AdminBack.Models;
using AdminBack.Models.DTOs.Inventario;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace AdminBack.Service
{
    public class InventarioService : IInventarioService
    {
        private readonly AdminDbContext _context;

        public InventarioService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<InventarioActualDto>> ConsultarStock()
        {
            return await _context.InventarioActuals
                .Include(i => i.Producto)
                .Include(i => i.Almacen)
                .Where(i => i.Cantidad > 0)
                .Select(i => new InventarioActualDto
                {
                    ProductoId = i.ProductoId,
                    Producto = i.Producto.Nombre,
                    AlmacenId = i.AlmacenId,
                    Almacen = i.Almacen.Nombre,
                    Cantidad = i.Cantidad
                })
                .OrderBy(i => i.Almacen)
                .ThenBy(i => i.Producto)
                .ToListAsync();
        }

        public async Task<List<EntradaInventarioDto>> ObtenerEntradas()
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
                    FechaEntrada = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
                    Referencia = e.Referencia,
                    Usuario = e.Usuario != null ? e.Usuario.NombreCompleto : "Sistema"
                })
                .ToListAsync();
        }

        public async Task<List<SalidaInventarioDto>> ObtenerSalidas()
        {
            return await _context.SalidasInventarios
                .Include(e => e.Producto)
                .Include(e => e.Almacen)
                .Include(e => e.Usuario)
                .OrderByDescending(e => e.FechaSalida)
                .Select(e => new SalidaInventarioDto
                {
                    Id = e.Id,
                    Producto = e.Producto.Nombre,
                    Almacen = e.Almacen.Nombre,
                    Cantidad = e.Cantidad,
                    FechaSalida = e.FechaSalida ?? DateTime.MinValue,
                    Referencia = e.Referencia,
                    Usuario = e.Usuario != null ? e.Usuario.NombreCompleto : "Sistema"
                })
                .ToListAsync();
        }

        public async Task<bool> RegistrarEntrada(EntradaInventarioCreateDto dto, int usuarioId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
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

                var inventario = await _context.InventarioActuals
                    .FirstOrDefaultAsync(i => i.ProductoId == dto.ProductoId && i.AlmacenId == dto.AlmacenId);

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

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                Console.WriteLine("INNER: " + ex.InnerException?.Message);
                // NO intentamos rollback si ya falló automáticamente
                return false;
            }
        }




        public async Task<bool> RegistrarSalida(SalidaInventarioCreateDto dto, int usuarioId)
        {
            var inventario = await _context.InventarioActuals.FirstOrDefaultAsync(i =>
                i.ProductoId == dto.ProductoId && i.AlmacenId == dto.AlmacenId);

            if (inventario == null || inventario.Cantidad < dto.Cantidad)
                return false;

            var salida = new SalidasInventario
            {
                ProductoId = dto.ProductoId,
                AlmacenId = dto.AlmacenId,
                Cantidad = dto.Cantidad,
                FechaSalida = DateTime.UtcNow,
                Referencia = dto.Referencia,
                UsuarioId = usuarioId
            };

            _context.SalidasInventarios.Add(salida);
            inventario.Cantidad -= dto.Cantidad;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
