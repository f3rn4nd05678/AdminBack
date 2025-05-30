using AdminBack.Data;
using AdminBack.Models.DTOs;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;


namespace AdminBack.Service
{
    public class FinanzasService : IFinanzasService
    {
        private readonly AdminDbContext _context;

        public FinanzasService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<List<DashboardFinanzasDto>> ObtenerResumenFinanciero(DateTime inicio, DateTime fin)
        {
            var pedidos = await _context.PedidosCliente
                .Where(p => p.Fecha >= inicio && p.Fecha <= fin)
                .ToListAsync();

            var pagos = await _context.PagosCliente
                .Where(p => p.FechaPago >= inicio && p.FechaPago <= fin)
                .ToListAsync();

            var notas = await _context.NotasCredito
                .Where(n => n.Fecha >= inicio && n.Fecha <= fin)
                .ToListAsync();

            var resumen = pedidos
                .GroupBy(p => p.Fecha.ToString("yyyy-MM"))
                .Select(g =>
                {
                    var mes = g.Key;
                    var facturado = g.Sum(p => p.Total);

                    var pagosMes = pagos.Where(p => p.FechaPago.ToString("yyyy-MM") == mes).Sum(p => p.Monto);
                    var notasMes = notas.Where(n => n.Fecha.ToString("yyyy-MM") == mes).Sum(n => n.Monto);
                    var pendiente = facturado - pagosMes - notasMes;

                    return new DashboardFinanzasDto
                    {
                        Mes = mes,
                        TotalFacturado = facturado,
                        TotalPagado = pagosMes,
                        TotalNotasCredito = notasMes,
                        Pendiente = pendiente
                    };
                })
                .OrderBy(r => r.Mes)
                .ToList();

            return resumen;
        }

        public async Task<List<DashboardFinanzasDetalladoDto>> ObtenerResumenDetallado(DateTime inicio, DateTime fin)
        {
            var pedidos = await _context.PedidosCliente
                .Include(p => p.Cliente)
                .Where(p => p.Fecha >= inicio && p.Fecha <= fin)
                .ToListAsync();

            var pagos = await _context.PagosCliente
                .Include(p => p.Pedido)
                .ThenInclude(pc => pc.Cliente)
                .Where(p => p.FechaPago >= inicio && p.FechaPago <= fin)
                .ToListAsync();

            var notas = await _context.NotasCredito
                .Include(n => n.Pedido)
                .ThenInclude(pc => pc.Cliente)
                .Where(n => n.Fecha >= inicio && n.Fecha <= fin)
                .ToListAsync();

            var resultado = pedidos
                .GroupBy(p => new { p.Cliente.Nombre, Mes = p.Fecha.ToString("yyyy-MM") })
                .Select(g =>
                {
                    var cliente = g.Key.Nombre;
                    var mes = g.Key.Mes;
                    var totalFacturado = g.Sum(p => p.Total);

                    var pagosMes = pagos.Where(p =>
                        p.Pedido.Cliente.Nombre == cliente &&
                        p.FechaPago.ToString("yyyy-MM") == mes).Sum(p => p.Monto);

                    var notasMes = notas.Where(n =>
                        n.Pedido.Cliente.Nombre == cliente &&
                        n.Fecha.ToString("yyyy-MM") == mes).Sum(n => n.Monto);

                    return new DashboardFinanzasDetalladoDto
                    {
                        Cliente = cliente,
                        Mes = mes,
                        TotalFacturado = totalFacturado,
                        TotalPagado = pagosMes,
                        TotalNotasCredito = notasMes,
                        Pendiente = totalFacturado - pagosMes - notasMes
                    };
                })
                .OrderBy(r => r.Mes)
                .ThenBy(r => r.Cliente)
                .ToList();

            return resultado;
        }

    }

}
