using AdminBack.Data;
using AdminBack.Models.DTOs;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace AdminBack.Service
{
    public class GestionFacturasService : IGestionFacturasService
    {
        private readonly AdminDbContext _context;

        public GestionFacturasService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AnularFactura(int pedidoId)
        {
            var pedido = await _context.PedidosCliente.FindAsync(pedidoId);
            if (pedido == null || pedido.Estado == "Anulada")
                return false;

            pedido.Estado = "Anulada";
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<ReporteClienteDto>> ObtenerReportePorCliente(int clienteId)
        {
            var pedidos = await _context.PedidosCliente
                .Where(p => p.ClienteId == clienteId)
                .ToListAsync();

            var pedidoIds = pedidos.Select(p => p.Id).ToList();

            var pagos = await _context.PagosCliente
                .Where(p => pedidoIds.Contains(p.PedidoId))
                .ToListAsync();

            var notas = await _context.NotasCredito
                .Where(n => pedidoIds.Contains(n.PedidoId))
                .ToListAsync();

            var reporte = pedidos.Select(p =>
            {
                var pagado = pagos.Where(pg => pg.PedidoId == p.Id).Sum(pg => pg.Monto);
                var credito = notas.Where(nc => nc.PedidoId == p.Id).Sum(nc => nc.Monto);
                var saldo = p.Total - pagado - credito;

                return new ReporteClienteDto
                {
                    PedidoId = p.Id,
                    Fecha = p.Fecha,
                    TotalFactura = p.Total,
                    TotalPagado = pagado,
                    TotalNotasCredito = credito,
                    Pendiente = saldo
                };
            }).ToList();

            return reporte;
        }

    }

}
