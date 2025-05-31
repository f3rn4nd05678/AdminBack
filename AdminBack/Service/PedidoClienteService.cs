using AdminBack.Data;
using AdminBack.Models;
using AdminBack.Models.DTOs.PedidoCliente;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace AdminBack.Service
{
    public class PedidoClienteService : IPedidoClienteService
    {
        private readonly AdminDbContext _context;

        public PedidoClienteService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<PedidoClienteDto>> ObtenerTodos()
        {
            return await _context.PedidosCliente
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
                .OrderByDescending(p => p.Fecha)
                .Select(p => new PedidoClienteDto
                {
                    Id = p.Id,
                    Cliente = p.Cliente.Nombre,
                    Estado = p.Estado,
                    Fecha = p.Fecha,
                    Total = p.Total,
                    Detalles = p.Detalles.Select(d => new DetallePedidoDto
                    {
                        ProductoId = d.ProductoId,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<bool> Crear(PedidoClienteCreateDto dto, int usuarioId, int almacenId)
        {
            foreach (var item in dto.Detalles)
            {
                var stock = await _context.InventarioActuals
                    .FirstOrDefaultAsync(i => i.ProductoId == item.ProductoId && i.AlmacenId == almacenId);

                if (stock == null || stock.Cantidad < item.Cantidad)
                    throw new Exception($"Stock insuficiente para producto {item.ProductoId}");
            }

            var pedido = new PedidoCliente
            {
                ClienteId = dto.ClienteId,
                Fecha = DateTime.UtcNow,
                Estado = "Procesado",
                Total = dto.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario)
            };

            _context.PedidosCliente.Add(pedido);

            var detalles = dto.Detalles.Select(d => new DetallePedidoCliente
            {
                Pedido = pedido, // puedes usar navigation property directamente
                ProductoId = d.ProductoId,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario
            }).ToList();

            _context.DetallesPedidoCliente.AddRange(detalles);

            foreach (var d in detalles)
            {
                _context.SalidasInventarios.Add(new SalidasInventario
                {
                    AlmacenId = almacenId,
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    FechaSalida = DateTime.UtcNow,
                    UsuarioId = usuarioId,
                    Referencia = $"Pedido Cliente #{pedido.Id}"
                });

                var inventario = await _context.InventarioActuals
                    .FirstOrDefaultAsync(i => i.ProductoId == d.ProductoId && i.AlmacenId == almacenId);

                if (inventario != null)
                    inventario.Cantidad -= d.Cantidad;
            }

            // Solo un SaveChanges al final, garantiza que todo esté disponible en BD
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
