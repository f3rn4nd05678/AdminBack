using AdminBack.Data;
using AdminBack.Models.DTOs;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace AdminBack.Service
{
    public class ReporteService : IReporteService
    {
        private readonly AdminDbContext _context;

        public ReporteService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReporteVentaPagoDto>> ObtenerReporteVentas(DateTime inicio, DateTime fin)
        {
            var pedidos = await _context.PedidosCliente
                .Include(p => p.Cliente)
                .Where(p => p.Fecha >= inicio && p.Fecha <= fin)
                .ToListAsync();

            var pagos = await _context.PagosCliente
                .Where(p => p.FechaPago >= inicio && p.FechaPago <= fin)
                .ToListAsync();

            var reporte = pedidos.Select(p =>
            {
                var totalPagado = pagos
                    .Where(pg => pg.PedidoId == p.Id)
                    .Sum(pg => pg.Monto);

                return new ReporteVentaPagoDto
                {
                    Fecha = p.Fecha,
                    Cliente = p.Cliente.Nombre,
                    TotalFactura = p.Total,
                    TotalPagado = totalPagado,
                    Pendiente = p.Total - totalPagado
                };
            }).ToList();

            return reporte;
        }
    }
}
