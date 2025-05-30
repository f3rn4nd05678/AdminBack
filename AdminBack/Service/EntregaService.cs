using AdminBack.Data;
using AdminBack.Models.DTOs;
using AdminBack.Models;
using AdminBack.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace AdminBack.Service
{
    public class EntregaService : IEntregaService
    {
        private readonly AdminDbContext _context;

        public EntregaService(AdminDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Asignar(EntregaAsignarDto dto)
        {
            var entrega = new EntregaPedido
            {
                PedidoId = dto.PedidoId,
                TransportistaId = dto.TransportistaId,
                RutaId = dto.RutaId,
                Estado = "Pendiente",
                FechaAsignacion = DateTime.UtcNow
            };

            _context.EntregaPedidos.Add(entrega);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CambiarEstado(int entregaId, string nuevoEstado)
        {
            var entrega = await _context.EntregaPedidos.FindAsync(entregaId);
            if (entrega == null) return false;

            entrega.Estado = nuevoEstado;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<EntregaDetalleDto>> ObtenerTodas()
        {
            return await _context.EntregaPedidos
                .Include(e => e.Transportista)
                .Include(e => e.Ruta)
                .Select(e => new EntregaDetalleDto
                {
                    Id = e.Id,
                    PedidoId = e.PedidoId,
                    Transportista = e.Transportista.Nombre,
                    Ruta = e.Ruta.Nombre,
                    Estado = e.Estado,
                    FechaAsignacion = e.FechaAsignacion
                }).ToListAsync();
        }

        public async Task<List<EntregaDetalleDto>> ObtenerPorPedido(int pedidoId)
        {
            return await _context.EntregaPedidos
                .Where(e => e.PedidoId == pedidoId)
                .Include(e => e.Transportista)
                .Include(e => e.Ruta)
                .Select(e => new EntregaDetalleDto
                {
                    Id = e.Id,
                    PedidoId = e.PedidoId,
                    Transportista = e.Transportista.Nombre,
                    Ruta = e.Ruta.Nombre,
                    Estado = e.Estado,
                    FechaAsignacion = e.FechaAsignacion
                }).ToListAsync();
        }
    }

}
