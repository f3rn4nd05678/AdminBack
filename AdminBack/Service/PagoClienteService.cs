using AdminBack.Data;
using AdminBack.Models;
using AdminBack.Models.DTOs;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace AdminBack.Service
{
    public class PagoClienteService : IPagoClienteService
    {
        private readonly AdminDbContext _context;

        public PagoClienteService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegistrarPago(PagoClienteDto dto)
        {
            var pedido = await _context.PedidosCliente.FindAsync(dto.PedidoId);
            if (pedido == null) return false;

            var pago = new PagoCliente
            {
                PedidoId = dto.PedidoId,
                Monto = dto.Monto,
                FechaPago = DateTime.UtcNow,
                Referencia = dto.Referencia
            };

            _context.PagosCliente.Add(pago);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<PagoClienteDetalleDto>> ObtenerTodos()
        {
            return await _context.PagosCliente
                .Include(p => p.Pedido)
                .ThenInclude(pc => pc.Cliente)
                .Select(p => new PagoClienteDetalleDto
                {
                    Id = p.Id,
                    PedidoId = p.PedidoId,
                    Cliente = p.Pedido.Cliente.Nombre,
                    Monto = p.Monto,
                    FechaPago = p.FechaPago,
                    Referencia = p.Referencia
                }).ToListAsync();
        }

        public async Task<List<PagoClienteDetalleDto>> ObtenerPorPedido(int pedidoId)
        {
            return await _context.PagosCliente
                .Include(p => p.Pedido)
                .ThenInclude(pc => pc.Cliente)
                .Where(p => p.PedidoId == pedidoId)
                .Select(p => new PagoClienteDetalleDto
                {
                    Id = p.Id,
                    PedidoId = p.PedidoId,
                    Cliente = p.Pedido.Cliente.Nombre,
                    Monto = p.Monto,
                    FechaPago = p.FechaPago,
                    Referencia = p.Referencia
                }).ToListAsync();
        }

        public async Task<List<PagoClienteDetalleDto>> ObtenerPorCliente(int clienteId)
        {
            return await _context.PagosCliente
                .Include(p => p.Pedido)
                .ThenInclude(pc => pc.Cliente)
                .Where(p => p.Pedido.ClienteId == clienteId)
                .Select(p => new PagoClienteDetalleDto
                {
                    Id = p.Id,
                    PedidoId = p.PedidoId,
                    Cliente = p.Pedido.Cliente.Nombre,
                    Monto = p.Monto,
                    FechaPago = p.FechaPago,
                    Referencia = p.Referencia
                }).ToListAsync();
        }

        public async Task<EstadoPagoPedidoDto> ObtenerEstadoPagoPorPedido(int pedidoId)
        {
            var pedido = await _context.PedidosCliente
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null) throw new Exception("Pedido no encontrado");

            var pagos = await _context.PagosCliente
                .Where(p => p.PedidoId == pedidoId)
                .ToListAsync();

            decimal pagado = pagos.Sum(p => p.Monto);
            decimal pendiente = pedido.Total - pagado;

            return new EstadoPagoPedidoDto
            {
                PedidoId = pedidoId,
                TotalFactura = pedido.Total,
                TotalPagado = pagado,
                Pendiente = pendiente,
                Estado = pendiente <= 0 ? "Pagado" : "Pendiente"
            };
        }


    }
}
