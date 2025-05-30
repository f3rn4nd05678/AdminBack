using AdminBack.Data;
using AdminBack.Models.DTOs;
using AdminBack.Models;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace AdminBack.Service
{
    public class PagoProveedorService : IPagoProveedorService
    {
        private readonly AdminDbContext _context;

        public PagoProveedorService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegistrarPago(PagoProveedorDto dto)
        {
            var orden = await _context.OrdenesCompra
                .Include(o => o.Proveedor)
                .FirstOrDefaultAsync(o => o.Id == dto.OrdenId);

            if (orden == null) return false;

            var pago = new PagoProveedor
            {
                OrdenId = dto.OrdenId,
                Monto = dto.Monto,
                FechaPago = DateTime.UtcNow,
                Referencia = dto.Referencia
            };

            _context.PagosProveedor.Add(pago);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<PagoProveedorDetalleDto>> ObtenerPorOrden(int ordenId)
        {
            return await _context.PagosProveedor
                .Include(p => p.Orden)
                .ThenInclude(o => o.Proveedor)
                .Where(p => p.OrdenId == ordenId)
                .Select(p => new PagoProveedorDetalleDto
                {
                    Id = p.Id,
                    FechaPago = p.FechaPago,
                    Monto = p.Monto,
                    Referencia = p.Referencia,
                    Proveedor = p.Orden.Proveedor.Nombre
                }).ToListAsync();
        }

        public async Task<List<PagoProveedorDetalleDto>> ObtenerPorProveedor(int proveedorId)
        {
            return await _context.PagosProveedor
                .Include(p => p.Orden)
                .ThenInclude(o => o.Proveedor)
                .Where(p => p.Orden.ProveedorId == proveedorId)
                .Select(p => new PagoProveedorDetalleDto
                {
                    Id = p.Id,
                    FechaPago = p.FechaPago,
                    Monto = p.Monto,
                    Referencia = p.Referencia,
                    Proveedor = p.Orden.Proveedor.Nombre
                }).ToListAsync();
        }
        public async Task<EstadoPagoProveedorDto> ObtenerEstadoPagoPorOrden(int ordenId)
        {
            var orden = await _context.OrdenesCompra
                .Include(o => o.Proveedor)
                .FirstOrDefaultAsync(o => o.Id == ordenId);

            if (orden == null) throw new Exception("Orden no encontrada");

            var pagos = await _context.PagosProveedor
                .Where(p => p.OrdenId == ordenId)
                .ToListAsync();

            decimal pagado = pagos.Sum(p => p.Monto);
            decimal pendiente = orden.TotalEstimado - pagado;

            return new EstadoPagoProveedorDto
            {
                OrdenId = orden.Id,
                Proveedor = orden.Proveedor.Nombre,
                TotalOrden = orden.TotalEstimado,
                TotalPagado = pagado,
                Pendiente = pendiente,
                Estado = pendiente <= 0 ? "Pagado" : "Pendiente"
            };
        }

    }

}
